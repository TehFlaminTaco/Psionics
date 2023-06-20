using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Abilities
{
    [TypeId("78fc6e24-d6d3-4881-aa0d-e3ab747eebfc")]
    public class StripElementalFocus : ContextAction
    {
        public override string GetCaption()
        {
            return "Strip Elemental Focus";
        }

        public override void RunAction()
        {
            var enumerator = Context.MaybeCaster?.Buffs?.Enumerable?.Where(c => ElementalFocus.BlueprintInstances.Contains(c.Blueprint));
            if (enumerator != null) {
                foreach (var buff in enumerator)
                {
                    buff.Deactivate();
                    buff.Remove();
                }
            }
        }
    }

    public class GainPsionicFocusMoveAbility
    {
        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "GainPsionicFocusMove";
        private static readonly string AbilityGUID = "592733bf-6509-466a-935c-2d1b0c9c7424";


        [Translate("Gain Psionic Focus (Move)")]
        private static readonly string DisplayName = "GainPsionicFocusMove.Name";
        private static readonly string Description = "GainPsionicFocus.Description";
        private static readonly string Icon = "assets/icons/psionicfocus.png";

        private static readonly string[] ElementalGUIDs = new[]
        {
            "3ddd9076-572e-4fc0-a9b7-01729681db33",
            "0ca26b66-430a-45f0-b969-6ec605bf636c",
            "b4600058-6214-4d1e-b279-b301b7894a8f",
            "eb89849f-fe68-43cd-b7e4-9891067f1e9b"
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
            for (int i = 0; i < 4; i++)
            {
                ElementalBlueprints[i] = AbilityConfigurator.New($"{AbilityName}{ElementNames[i]}", ElementalGUIDs[i])
                    .SetDisplayName($"GainPsionicFocus{ElementNames[i]}.Name".Translate($"Gain Psionic Focus ({ElementNames[i]})"))
                    .SetDescription("GainPsionicFocus.Description")
                    .SetIcon(ElementIcons[i])
                    .AddComponent<HideDCFromTooltip>()
                    .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
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
