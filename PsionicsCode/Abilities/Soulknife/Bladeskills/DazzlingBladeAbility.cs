using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Psionics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace Psionics.Abilities.Soulknife.Bladeskills
{

    public class DazzlingBladeAbility
    {
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Physical;

        private static readonly string AbilityName = "DazzlingBladeAbility";
        private static readonly string AbilityGUID = "8077341a-a2eb-4000-a2db-6971446426f2";

        public static BlueprintAbility BlueprintInstance = null;

        private static readonly string DisplayName = "DazzlingBladeFeat.Name";
        private static readonly string Description = "DazzlingBladeFeat.Description";
        private static readonly string Icon = "assets/icons/dazzlingblade.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<AbilityTargetsAround>(c =>
                {
                    c.m_Radius = 30.Feet();
                    c.m_TargetType = TargetType.Enemy;
                    c.m_IncludeDead = false;
                    c.m_SpreadSpeed = 0.Feet();
                    c.m_Flags = 0;
                    c.m_Condition = new Kingmaker.ElementsSystem.ConditionsChecker();
                })
                .SetActionType(ActionType)
                .SetType(TypeAbility)
                .SetLocalizedSavingThrow(SavingThrow.FortNegates)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .SetHasFastAnimation(true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, null, new ContextValue()
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = Kingmaker.Enums.AbilityRankType.StatBonus
                    }, false, ActionsBuilder.New()
                    .ApplyBuff(BuffRefs.DazzledBuff.Reference.Get(), new ContextDurationValue() { Rate = DurationRate.Rounds, DiceType = Kingmaker.RuleSystem.DiceType.One, DiceCountValue = 1 }, false, true, false, true, true, false, false))
                )
                .AddContextRankConfig(new Kingmaker.UnitLogic.Mechanics.Components.ContextRankConfig()
                {
                    m_BaseValueType = Kingmaker.UnitLogic.Mechanics.Components.ContextRankBaseValueType.BaseAttack,
                    m_Progression = Kingmaker.UnitLogic.Mechanics.Components.ContextRankProgression.BonusValue,
                    m_StepLevel = 10,
                    m_Type = Kingmaker.Enums.AbilityRankType.StatBonus
                })
                .AddContextSetAbilityParams(true, null, null, new ContextValue()      
                {
                    ValueType = ContextValueType.CasterProperty,
                    Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.Level
                }, null, BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge.Replace, null)
                .Configure();
        }
    }
}
