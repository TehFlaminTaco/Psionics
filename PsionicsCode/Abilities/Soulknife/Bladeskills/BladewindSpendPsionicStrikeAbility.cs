using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    public class BladewindSpendPsionicStrikeAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "BladewindSpendPsionicStrikeAbility";
        private static readonly string AbilityGUID = "2589c3f4-fdc2-4462-b591-6efb79dca1b4";

        private static readonly string DisplayName = "BladewindSpendPsionicStrikeAbility.Name".Translate("Bladewind Spend Psionic Strike");
        private static readonly string Description = "BladewindSpendPsionicStrikeAbility.Description".Translate("As part of a Bladewind attack, a Soulknife can opt to use their Psychic Strike to reroll a missed attack, rather than for damage.");
        private static readonly string Icon = "assets/icons/bladewindtoggle.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .Configure();
        }
    }
}
