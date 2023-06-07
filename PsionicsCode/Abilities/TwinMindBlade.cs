using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Abilities
{
    public class TwinMindBlade
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "TwinMindBlade";
        private static readonly string AbilityGUID = "da0a272c-923b-4f47-ad85-d84c305eddeb";

        private static readonly string DisplayName = "TwinMindBlade.Name".Translate("Twin Mind Blade");
        private static readonly string Description = "TwinMindBlade.Description".Translate("Summon two Mind Blades instead of one. The enhancement bonus of each is reduced by 1.");
        private static readonly string Icon = "assets/icons/twinmindblade.png";

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
