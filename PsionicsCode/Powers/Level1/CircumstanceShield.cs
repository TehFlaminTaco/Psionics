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
    public class CircumstanceShield
    {
        private static readonly AbilityRange Range = AbilityRange.Personal;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "CircumstanceShieldPower";
        private static readonly string AbilityGUID = "23970673-1987-48c2-b1cb-e1f63872ae74";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment EnhancementAugment = new Augment()
        {
            BaseName = "CircumstanceShieldPower",
            Name = "Bonus",
            Description = "CircumstanceShieldPower_Bonus.Description".Translate("For every 5 additional power points you spend, the insight bonus increases by +1."),
            Cost = 5,
            MaxRank = 6
        };

        [Translate("Circumstance Shield")]
        private static readonly string DisplayName = "CircumstanceShieldPower.Name";
        [Translate("Your shield of insight alerts you to potential dangers and supercharges your reaction time. You gain a +1 insight bonus on your Initiative checks for the duration of the effect.")]
        private static readonly string Description = "CircumstanceShieldPower.Description";
        private static readonly string Icon = "assets/icons/circumstanceshield.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Divination)
                .AddComponent<HideDCFromTooltip>()
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
                            .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Divination)
                            .AddComponent<HideDCFromTooltip>()
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.Self)
                            .SetCanTargetSelf(true)
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ApplyBuffAugmentRanks>(abar =>
                                {
                                    abar.buff = CircumstanceShieldBuff.BlueprintInstance;
                                    abar.augment = EnhancementAugment;
                                    abar.toCaster = true;
                                    abar.duration = ContextDuration.Variable(ContextValues.Rank(AbilityRankType.Default), DurationRate.Hours);
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
                        a.BaseName = "CircumstanceShieldPower";
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(Icon)
                .Configure();
        }
    }
}
