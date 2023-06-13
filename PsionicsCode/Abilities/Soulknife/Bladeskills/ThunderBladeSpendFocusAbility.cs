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
    public class ThunderBladeSpendFocusAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "ThunderBladeSpendFocusAbility";
        private static readonly string AbilityGUID = "a6af8483-2121-454b-a8a9-1ba06aca7fcd";

        private static readonly string DisplayName = "ThunderBladeSpendFocusAbility.Name".Translate("Thunder Blade (Spend Focus)");
        private static readonly string Description = "ThunderBladeFeat.Description";
        private static readonly string Icon = "assets/icons/thunderblade.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(ThunderBladeSpendFocusBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
