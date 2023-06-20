using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints;
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
    public class GainPsionicFocusAbility
    {
        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "GainPsionicFocus";
        private static readonly string AbilityGUID = "a85f2a11-b5cf-4c33-906e-8742a9d2b2f2";


        [Translate("Gain Psionic Focus")]
        private static readonly string DisplayName = "GainPsionicFocus.Name";
        [Translate("Merely having the ability to hold a reservoir of psionic power points in mind gives psionic characters a special energy. Psionic characters can put that energy to work without actually paying a power point cost—they can become psionically focused.\nIf you have a power point pool or the ability to manifest psi-like abilities, you can meditate to become psionically focused. Meditating is a full-round action that provokes attacks of opportunity.\nWhen you are psionically focused, you can expend your focus on any single concentration check you make thereafter. When you expend your focus in this manner, your concentration check is treated as if you rolled a 15. It’s like taking 10, except that the number you add to your concentration modifier is 15. You can also expend your focus to gain the benefit of a psionic feat—many psionic feats are activated in this way.\nOnce you are psionically focused, you remain focused until you expend your focus, become unconscious, or go to sleep (or enter a meditative trance, in cases such as elans or elves).\r\n\r\nYou may still gain psionic focus even if you have depleted all of your power points.\nExpending your psionic focus to power a feat, class feature, or any other ability only powers a single effect. You cannot gain the benefit of multiple abilities that require expending focus by expending your psionic focus once; each effect requires its own instance of expending psionic focus.")]
        private static readonly string Description = "GainPsionicFocus.Description";
        private static readonly string Icon = "assets/icons/psionicfocus.png";

        private static readonly string[] ElementalGUIDs = new[]
        {
            "94410ad7-1094-4177-948a-e8abec383beb",
            "2dd2dfcf-964f-4b33-b905-29e2ad47d79c",
            "9aa9662f-d2d6-4d04-8e5a-a8da728937a1",
            "f64a481f-0543-44df-bcc3-0290488a0ccb"
        };
        public static BlueprintAbility[] ElementalBlueprints = new BlueprintAbility[4];

        private static string[] ElementNames = new[]
        {
            "Fire",
            "Cold",
            "Electricity",
            "Sonic"
        };
        private static string[] ElementIcons = new[]
        {
            "assets/icons/firefocus.png",
            "assets/icons/coldfocus.png",
            "assets/icons/electricfocus.png",
            "assets/icons/sonicfocus.png"
        };

        public static void Configure()
        {
            for(int i=0; i < 4; i++)
            {
                ElementalBlueprints[i] = AbilityConfigurator.New($"{AbilityName}{ElementNames[i]}", ElementalGUIDs[i])
                    .SetDisplayName($"GainPsionicFocus{ElementNames[i]}.Name".Translate($"Gain Psionic Focus ({ElementNames[i]})"))
                    .SetDescription("GainPsionicFocus.Description")
                    .SetIcon(ElementIcons[i])
                    .AddComponent<HideDCFromTooltip>()
                    .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                    .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                    .SetIsFullRoundAction(true)
                    .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuffPermanent(PsionicFocus.BlueprintInstance, false, true, false, true, true, false, true)
                        .Add<StripElementalFocus>()
                        .ApplyBuffPermanent(ElementalFocus.BlueprintInstances[i], false, true, false, true, true, false, true)
                    )
                    .Configure();
            }

            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<HideDCFromTooltip>()
                .SetAbilityVariants(
                    new Kingmaker.Utility.Cacheable<Kingmaker.UnitLogic.Abilities.Components.AbilityVariants>()
                )
                .AddAbilityVariants(ElementalBlueprints.Select(c => (BlueprintCore.Utils.Blueprint<BlueprintAbilityReference>)c.ToReference<BlueprintAbilityReference>()).ToList())
                .Configure();
        }
    }
}
