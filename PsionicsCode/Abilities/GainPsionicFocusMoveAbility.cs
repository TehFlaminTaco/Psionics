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
    public class GainPsionicFocusMoveAbility
    {
        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "GainPsionicFocusMove";
        private static readonly string AbilityGUID = "592733bf-6509-466a-935c-2d1b0c9c7424";


        [Translate("Gain Psionic Focus (Move)")]
        private static readonly string DisplayName = "GainPsionicFocusMOve.Name";
        private static readonly string Description = "GainPsionicFocus.Description";
        private static readonly string Icon = "assets/icons/psionicfocus.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .RequirePsionicFocus(true)
                .AddComponent<HideDCFromTooltip>()
                .SetIcon(Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuffPermanent(PsionicFocus.BlueprintInstance, false, true, false, true, true, false, true)
                )
                .Configure();
        }
    }
}
