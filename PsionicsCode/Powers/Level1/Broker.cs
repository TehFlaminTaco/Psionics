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
    public class Broker
    {
        private static readonly AbilityRange Range = AbilityRange.Personal;
        private static readonly CommandType ActionType = CommandType.Swift;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "BrokerPower";
        private static readonly string AbilityGUID = "d7f93cf4-cc90-45d1-b344-857fcd16ec97";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment EnhancementAugment = new Augment()
        {
            BaseName = "BrokerPower",
            Name = "Bonus",
            Description = "BrokerPower_Bonus.Description".Translate("For each 2 additional power points you spend, the insight bonus to Pursuasion increases by +1."),
            Cost = 2,
            MaxRank = 15
        };

        [Translate("Broker")]
        private static readonly string DisplayName = "BrokerPower.Name";
        [Translate("You gain temporary, intuitive insight into dealing equitably with others. You gain a +2 insight bonus to Pursuasion.\nAugment For each 2 additional power points you spend, the insight bonus to Pursuasion increases by +1.")]
        private static readonly string Description = "BrokerPower.Description";
        private static readonly string Icon = "assets/icons/broker.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Transmutation)
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
                            .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Transmutation)
                            .AddComponent<HideDCFromTooltip>()
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.Self)
                            .SetCanTargetSelf(true)
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ApplyBuffAugmentRanks>(abar =>
                                {
                                    abar.buff = BrokerBuff.BlueprintInstance;
                                    abar.augment = EnhancementAugment;
                                    abar.toCaster = true;
                                    abar.duration = ContextDuration.Variable(ContextValues.Rank(AbilityRankType.Default), DurationRate.Rounds);
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
                        a.BaseName = "BrokerPower";
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(Icon)
                .Configure();
        }
    }
}
