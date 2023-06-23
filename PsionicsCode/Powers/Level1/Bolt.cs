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
using Kingmaker.UnitLogic.Mechanics.Components;

namespace Psionics.Powers.Level1
{
    public class ApplyBuffAugmentRanks : ContextAction
    {
        public bool toCaster = true;
        public BlueprintBuff buff;
        public ContextDurationValue duration;
        public Augment augment;
        public override string GetCaption()
        {
            return "Apply Augmented Buff";
        }

        public override void RunAction()
        {
            UnitEntityData t = toCaster ? Context.MaybeCaster : Context.MainTarget.Unit;
            if (t == null) return;
            var b = t.AddBuff(buff, Context, duration.Calculate(Context).Seconds);
            b.Rank = Augmentable.GetRank(Context.MaybeCaster, augment);
        }
    }

    public class Bolt
    {
        private static readonly AbilityRange Range = AbilityRange.Personal;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "BoltPower";
        private static readonly string AbilityGUID = "ab642bf3-8f35-43e3-bd7a-c396aa0ca477";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment EnhancementAugment = new Augment()
        {
            BaseName = "BoltPower",
            Name = "Enhancement",
            Description = "BoltPower_Enhancement.Description".Translate("For every 3 additional power points you spend, this power improves the ammunition’s enhancement bonus on attack rolls and damage rolls by 1"),
            Cost = 3,
            MaxRank = 6
        };

        [Translate("Bolt")]
        private static readonly string DisplayName = "BoltPower.Name";
        [Translate("You create ectoplasmic crossbow bolts, arrows, or sling bullets, appropriate to your size, which dissipate into their constituent ectoplasmic particles when the duration ends. Ammunition you create has a +1 enhancement bonus on attack rolls and damage rolls.\nAugment For every 3 additional power points you spend, this power improves the ammunition’s enhancement bonus on attack rolls and damage rolls by 1")]
        private static readonly string Description = "BoltPower.Description";
        private static readonly string Icon = "assets/icons/bolt.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Conjuration)
                .AddComponent<ConstantCountText>(cct=>cct.Text = -1)
                .AddComponent<HideDCFromTooltip>()
                .Augmentable(
                    a =>
                    {
                        a.Castable = CastBlueprint = AbilityConfigurator.New($"{AbilityName}_Cast".HashGUID(), HashGUID.Last)
                            .SetDisplayName(DisplayName)
                            .SetDescription(Description)
                            .SetIcon(Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Conjuration)
                            .AddComponent<HideDCFromTooltip>()
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.Self)
                            .SetCanTargetSelf(true)
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ApplyBuffAugmentRanks>(abar =>
                                {
                                    abar.buff = BoltBuff.BlueprintInstance;
                                    abar.augment = EnhancementAugment;
                                    abar.toCaster = true;
                                    abar.duration = ContextDuration.Variable(ContextValues.Rank(AbilityRankType.Default), DurationRate.Minutes);
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
                        a.BaseName = "BoltPower";
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(Icon)
                .Configure();
        }
    }
}
