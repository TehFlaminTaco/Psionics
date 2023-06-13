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
    public class LightningBladeAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "LightningBladeAbility";
        private static readonly string AbilityGUID = "c3d41aca-271d-440f-ba73-e2f3595aa675";

        private static readonly string DisplayName = "LightningBladeFeat.Name";
        private static readonly string Description = "LightningBladeFeat.Description";
        private static readonly string Icon = "assets/icons/lightningblade.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(LightningBladeBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
