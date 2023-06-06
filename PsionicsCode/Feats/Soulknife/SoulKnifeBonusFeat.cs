using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class SoulKnifeBonusFeat
    {
        public static BlueprintFeatureSelection BlueprintInstance = null;
        private static readonly string FeatName = "SoulknifeBonusFeat";
        private static readonly string FeatGUID = "0aa604f3-adbf-4c41-ab07-d9506427b910";

        [Translate("Soulknife Bonus Feat")]
        private static readonly string DisplayName = "SoulknifeBonusFeat.Name";
        [Translate("The soulknife may choose Power Attack, Two-Weapon Fighting, or Weapon Focus (mind blade) as a bonus feat at 1st level.", true)]
        private static readonly string Description = "SoulknifeBonusFeat.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureSelectionConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddToAllFeatures(
                    SoulknifeKineticBlastFeature.BlueprintInstance,
                    "9972f33f977fc724c838e59641b2fca5",
                    "ac8aaf29054f5b74eb18f2af950e752d"
                )
                .SetIsClassFeature(true)
                .Configure();
        }
    }
}
