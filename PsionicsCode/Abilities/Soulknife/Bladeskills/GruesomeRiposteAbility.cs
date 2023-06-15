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
    public class GruesomeRiposteAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "GruesomeRiposteAbility";
        private static readonly string AbilityGUID = "e3b40a51-f3ea-41d1-ab8d-44f3c28d66e8";

        private static readonly string DisplayName = "GruesomeRiposteAbility.Name".Translate("Gruesome Riposte (Toggle)");
        private static readonly string Description = "GruesomeRiposteFeat.Description";
        private static readonly string Icon = "assets/icons/gruesomeriposte.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(GruesomeRiposteBuff.BlueprintInstance)
                .Configure();
        }
    }
}
