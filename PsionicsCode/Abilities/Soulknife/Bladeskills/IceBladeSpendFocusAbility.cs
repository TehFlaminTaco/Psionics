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
    public class IceBladeSpendFocusAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "IceBladeSpendFocusAbility";
        private static readonly string AbilityGUID = "5808df23-4c16-4d22-9ef7-3a5987fe69dc";

        private static readonly string DisplayName = "IceBladeSpendFocusAbility.Name".Translate("Ice Blade (Spend Focus)");
        private static readonly string Description = "IceBladeFeat.Description";
        private static readonly string Icon = "assets/icons/iceblade.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(IceBladeSpendFocusBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
