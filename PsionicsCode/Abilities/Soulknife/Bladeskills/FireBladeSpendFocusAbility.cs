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
    public class FireBladeSpendFocusAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "FireBladeSpendFocusAbility";
        private static readonly string AbilityGUID = "fe727867-5134-44b4-98b0-1fbfa5e8a31b";

        private static readonly string DisplayName = "FireBladeSpendFocusAbility.Name".Translate("Fire Blade (Spend Focus)");
        private static readonly string Description = "FireBladeFeat.Description";
        private static readonly string Icon = "assets/icons/fireblade.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(FireBladeSpendFocusBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
