using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Psionics;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    public class ExpandShieldAbility
    {
        private static readonly AbilityRange Range = AbilityRange.Personal;
        private static readonly CommandType ActionType = CommandType.Free;
        private static readonly AbilityType TypeAbility = AbilityType.Physical;

        private static readonly string AbilityName = "ExpandShieldAbility";
        private static readonly string AbilityGUID = "f0448f8b-7aae-4cab-99e6-1477ba834824";

        public static BlueprintAbility BlueprintInstance = null;

        private static readonly string DisplayName = "ExpandShieldFeat.Name";
        private static readonly string Description = "ExpandShieldFeat.Description";
        private static readonly string Icon = "assets/icons/mindtowershield.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetRange(Range)
                .SetActionType(ActionType)
                .SetType(TypeAbility)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.SelfTouch)
                .SetHasFastAnimation(true)
                .AddAbilityTargetHasConditionOrBuff(new List<BlueprintCore.Utils.Blueprint<Kingmaker.Blueprints.BlueprintBuffReference>>() { BuffRefs.FightDefensivelyBuff.Reference.Get() })
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuff(MindTowerShieldBuff.BlueprintInstance, new Kingmaker.UnitLogic.Mechanics.ContextDurationValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.One,
                        DiceCountValue = 1,
                        Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds,
                        BonusValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        }
                    }, true, true, false, false, true, false, true)
                )
                .Configure();
        }
    }
}
