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
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.EntitySystem.Stats;
using JetBrains.Annotations;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker;
using UnityEngine;

namespace Psionics.Powers.Level1
{
    public class Demoralize
    {
        private static readonly AbilityRange Range = AbilityRange.Custom;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "DemoralizePower";
        private static readonly string AbilityGUID = "cf672904-1abd-4ae0-9235-77dc8d743f00";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment EnhancementAugment = new Augment()
        {
            BaseName = "DemoralizePower",
            Name = "DC",
            Description = "DemoralizePower_DC.Description".Translate("For every 2 additional power points you spend, this power’s save DC increases by 1"),
            Cost = 2,
            MaxRank = 15
        };

        [Translate("Demoralize")]
        private static readonly string DisplayName = "DemoralizePower.Name";
        [Translate("You fill your enemies with self-doubt. Any enemy in the area that fails its save becomes shaken for the duration of the power. Allies and creatures without an Intelligence score are unaffected.")]
        private static readonly string Description = "DemoralizePower.Description";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<ConstantCountText>(cct=>cct.Text = -1)
                .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Enchantment)
                .Augmentable(
                    a =>
                    {
                        a.Castable = CastBlueprint = AbilityConfigurator.New($"{AbilityName}_Cast".HashGUID(), HashGUID.Last)
                            .SetDisplayName(DisplayName)
                            .SetDescription(Description)
                            .SetIcon(AbilityRefs.PersuasionUseAbility.Reference.Get().Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Enchantment)
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.BreathWeapon)
                            .SetCanTargetSelf(false)
                            .SetCanTargetEnemies(true)
                            .SetCanTargetPoint(true)
                            .SetSpellResistance(true)
                            .SetCustomRange(30)
                            .AddAbilityTargetsAround(null, false, null, BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge.Fail, 30.Feet(), 0.Feet(), TargetType.Enemy)
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ContextActionSavingThrowAugmentDC>(castad=> {
                                    castad.Type = SavingThrowType.Will;
                                    castad.FromBuff = false;
                                    castad.CustomDC = null;
                                    castad.HasCustomDC = false;
                                    castad.m_ConditionalDCIncrease = new ContextActionSavingThrowAugmentDC.ConditionalDCIncrease[0];
                                    castad.UseDCFromContextSavingThrow = false;
                                    castad.ScaleWith = EnhancementAugment;
                                    castad.Actions = ActionsBuilder.New()
                                                        .Add<ApplyBuffAugmentRanks>(abar =>
                                                        {
                                                            abar.buff = BuffRefs.Shaken.Reference.Get();
                                                            abar.augment = EnhancementAugment;
                                                            abar.toCaster = false;
                                                            abar.duration = ContextDuration.Variable(ContextValues.Rank(AbilityRankType.Default), DurationRate.Minutes);
                                                        })
                                                        .Build();
                                })
                            )
                            .AddComponent<AbilityCostPowerpoints>(c => {
                                c.m_Cost = 1;
                                c.AugmentableHolder = a;
                            })
                            .AddContextRankConfig(ContextRankConfigs.CasterLevel(false, AbilityRankType.Default))
                            .Configure();
                        a.Augments = new[] {
                            EnhancementAugment
                        };
                        a.BaseName = "DemoralizePower";
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(AbilityRefs.PersuasionUseAbility.Reference.Get().Icon)
                .Configure();
        }
    }
}
