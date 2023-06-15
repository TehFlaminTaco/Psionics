using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
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
    public class ReachingBladeAbility
    {
        private static readonly AbilityRange Range = AbilityRange.Personal;
        private static readonly CommandType ActionType = CommandType.Free;
        private static readonly AbilityType TypeAbility = AbilityType.Physical;

        private static readonly string AbilityName = "ReachingBladeAbility";
        private static readonly string AbilityGUID = "c534274d-6d1c-4a89-8480-5f4eb6ab5beb";

        public static BlueprintAbility BlueprintInstance = null;

        private static readonly string DisplayName = "ReachingBladeFeat.Name";
        private static readonly string Description = "ReachingBladeFeat.Description";
        private static readonly string Icon = "assets/icons/reachingblade.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetRange(Range)
                .SetActionType(ActionType)
                .SetType(TypeAbility)
                .AddComponent<RequireMindBlade>()
                .AddComponent<PrerequisiteHasPsionicFocus>()
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .ApplyBuff(ReachingBladeBuff.BlueprintInstance, new Kingmaker.UnitLogic.Mechanics.ContextDurationValue()
                        {
                            DiceCountValue = 1,
                            DiceType = Kingmaker.RuleSystem.DiceType.One,
                            Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds,
                            BonusValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                            {
                                Value = 0
                            }
                        }, false, true, false, true, true, false, true)
                        .Add<SpendPsionicFocus>()
                )
                .Configure();
        }
    }
}
