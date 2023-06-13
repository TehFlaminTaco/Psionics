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
    public class ThunderBladeAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "ThunderBladeAbility";
        private static readonly string AbilityGUID = "b20bdde6-46d3-4e61-bda8-211de19aaa1b";

        private static readonly string DisplayName = "ThunderBladeFeat.Name";
        private static readonly string Description = "ThunderBladeFeat.Description";
        private static readonly string Icon = "assets/icons/thunderblade.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(ThunderBladeBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
