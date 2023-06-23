using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Psionics;
using System;
using System.Linq;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using HarmonyLib;
using BlueprintCore.Blueprints.References;
using Psionics.Buffs;
using Kingmaker.RuleSystem.Rules.Damage;
using static Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription;
using Kingmaker.Enums.Damage;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Enums;
using Kingmaker.Controllers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker;
using UnityEngine.Serialization;
using UnityEngine;
using Psionics.AreaEffect;

namespace Psionics.Powers.Level1
{
    [TypeId("6fb4c276-0b45-46c2-8015-8856e5dab4e9")]
    public class ContextActionSpawnAreaEffectRanks : ContextAction
    {
        [SerializeField]
        [FormerlySerializedAs("AreaEffect")]
        public BlueprintAbilityAreaEffect m_AreaEffect;

        public Func<UnitEntityData, int> CalculateDC = null;

        public ContextDurationValue DurationValue;

        public bool OnUnit;

        public BlueprintAbilityAreaEffect AreaEffect => m_AreaEffect;

        public override string GetCaption()
        {
            string arg = ((AreaEffect != null) ? AreaEffect.ToString() : "<undefined>");
            return $"Spawn {arg} for {DurationValue}";
        }

        public override void RunAction()
        {
            TimeSpan seconds = DurationValue.Calculate(base.Context).Seconds;
            AreaEffectEntityData areaEffectEntityData = AreaEffectsController.Spawn(base.Context, AreaEffect, base.Target, seconds);
            if (areaEffectEntityData == null || base.AbilityContext == null)
            {
                return;
            }
            areaEffectEntityData.Context.Ranks[(int)AbilityRankType.DamageDice] = CalculateDC != null ? CalculateDC(Context.MaybeCaster) : 1;

            foreach (UnitEntityData u in Game.Instance.State.Units)
            {
                if (!u.Descriptor.State.IsDead && u.IsInGame && areaEffectEntityData.Blueprint.Shape != AreaEffectShape.AllArea && areaEffectEntityData.View.Contains(u) && (areaEffectEntityData.AffectEnemies || !base.AbilityContext.Caster.IsEnemy(u)))
                {
                    EventBus.RaiseEvent(delegate (IApplyAbilityEffectHandler h)
                    {
                        h.OnTryToApplyAbilityEffect(base.AbilityContext, u);
                    });
                }
            }
        }
    }

    public class EctoplasmicSheen
    {
        private static readonly AbilityRange Range = AbilityRange.Close;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "EctoplasmicSheen";
        private static readonly string AbilityGUID = "bcae35d7-45ec-4e81-80c5-05568af258c6";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment DCAugment = new Augment()
        {
            BaseName = AbilityName,
            Name = "DC",
            Description = $"{AbilityName}_DC.Description".Translate("For every two additional power points you spend augmenting this power, the power’s save DC increases by 1."),
            Cost = 2,
            MaxRank = 15
        };

        [Translate("Ectoplasmic Sheen")]
        private static readonly string DisplayName = $"{AbilityName}.Name";
        [Translate("You draw forth ectoplasm in an area, causing the surface to become slick. Any creature in the area when the power is manifested must make a successful Reflex save or fall. A creature can walk within or through the area of ectoplasm at half normal speed with a DC 10 Acrobatics check. Failure means it can’t move that round (and must then make a Reflex save or fall), while failure by 5 or more means it falls (see the Acrobatics skill for details). Creatures that do not move on their turn do not need to make this check and are not considered flat-footed.\nAugment For every two additional power points you spend augmenting this power, the power’s save DC increases by 1.",true)]
        private static readonly string Description = $"{AbilityName}.Description";
        private static readonly string Icon = "assets/icons/ectoplasmicsheen.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddSpellComponent(SpellSchool.Conjuration)
                .AddSpellDescriptorComponent(SpellDescriptor.MovementImpairing | SpellDescriptor.Ground)
                .AddComponent<ConstantCountText>(cct=>cct.Text = -1)
                .Augmentable(
                    a =>
                    {
                        a.Castable = CastBlueprint = AbilityConfigurator.New($"{AbilityName}_Cast".HashGUID(), HashGUID.Last)
                            .SetDisplayName(DisplayName)
                            .SetDescription(Description)
                            .SetIcon(Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .AddSpellComponent(SpellSchool.Conjuration)
                            .AddSpellDescriptorComponent(SpellDescriptor.MovementImpairing | SpellDescriptor.Ground)
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.Directional)
                            .SetCanTargetFriends(false)
                            .SetCanTargetSelf(false)
                            .SetCanTargetEnemies(true)
                            .SetCanTargetPoint(true)
                            .SetSpellResistance(true)
                            .AddAbilityAoERadius(false, 0, null, BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge.Fail, 10.Feet(), TargetType.Any)
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ContextActionSpawnAreaEffectRanks>(casaer =>
                                {
                                    casaer.m_AreaEffect = EctoplasmicSheenAreaEffect.BlueprintInstance;
                                    casaer.DurationValue = ContextDuration.Variable(ContextValues.Rank(AbilityRankType.Default), DurationRate.Rounds, true);
                                    casaer.OnUnit = false;
                                    casaer.CalculateDC = unit =>
                                        Augmentable.GetRank(unit, DCAugment);
                                })
                            )
                            .AddComponent<AbilityCostPowerpoints>(c => {
                                c.m_Cost = 1;
                                c.AugmentableHolder = a;
                            })
                            .AddContextRankConfig(ContextRankConfigs.CasterLevel(false, AbilityRankType.Default))
                            .Configure();
                        a.Augments = new[] {
                            DCAugment
                        };
                        a.BaseName = AbilityName;
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(Icon)
                .Configure();
        }
    }
}
