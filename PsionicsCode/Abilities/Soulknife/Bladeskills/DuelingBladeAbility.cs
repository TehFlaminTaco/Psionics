using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    public class DuelingBladeAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "DuelingBladeAbility";
        private static readonly string AbilityGUID = "4a09832b-2e3a-4ac9-bfaf-f549f3780693";

        private static readonly string DisplayName = "DuelingBladeAbility.Name".Translate("Dueling Blade (Toggle)");
        private static readonly string Description = "DuelingBladeFeat.Description";
        private static readonly string Icon = "assets/icons/duelingblade.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(DuelingBladeBuff.BlueprintInstance)
                .Configure();
        }
    }
}
