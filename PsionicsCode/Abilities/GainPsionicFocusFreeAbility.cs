using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Abilities
{
    public class GainPsionicFocusFreeAbility
    {
        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "GainPsionicFocusFree";
        private static readonly string AbilityGUID = "9c082bdd-38a1-4e29-b145-e984b88e8c7d";


        [Translate("Gain Psionic Focus (Free)")]
        private static readonly string DisplayName = "GainPsionicFocusFree.Name";
        private static readonly string Description = "GainPsionicFocus.Description";
        private static readonly string Icon = "assets/icons/psionicfocus.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .RequirePsionicFocus(true)
                .AddComponent<HideDCFromTooltip>()
                .AddComponent<PrerequisiteHasPsionicFocus>()
                .SetIcon(Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .SetIsFullRoundAction(true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuffPermanent(PsionicFocus.BlueprintInstance, false, true, false, true, true, false, true)
                    .Add<SpendPsionicFocus>()
                )
                .Configure();
        }
    }
}
