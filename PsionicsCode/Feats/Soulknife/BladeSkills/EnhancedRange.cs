using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class EnhancedRange
    {
        private static readonly string FeatName = "EnhancedRange";
        private static readonly string FeatGUID = "fb86ac65-ad7f-4f72-89f7-c444c338e286";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Enhanced Range")]
        private static readonly string DisplayName = "EnhancedRange.Name";
        [Translate("The soulknife’s range increment when throwing her mind blade in any form doubles.", true)]
        private static readonly string Description = "EnhancedRange.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .Configure();
        }

    }
}
