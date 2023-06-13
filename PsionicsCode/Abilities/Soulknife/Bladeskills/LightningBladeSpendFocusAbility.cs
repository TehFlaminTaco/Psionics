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
    public class LightningBladeSpendFocusAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "LightningBladeSpendFocusAbility";
        private static readonly string AbilityGUID = "1e15caaa-11f6-48a5-b442-54a811099ac6";

        private static readonly string DisplayName = "LightningBladeSpendFocusAbility.Name".Translate("Lightning Blade (Spend Focus)");
        private static readonly string Description = "LightningBladeFeat.Description";
        private static readonly string Icon = "assets/icons/lightningblade.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(LightningBladeSpendFocusBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
