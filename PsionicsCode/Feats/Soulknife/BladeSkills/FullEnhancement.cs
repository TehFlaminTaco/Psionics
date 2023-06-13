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
    public class FullEnhancement
    {
        private static readonly string FeatName = "FullEnhancement";
        private static readonly string FeatGUID = "f222f75f-a094-4825-9ba0-f1b197572ef2";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Full Enhancement")]
        private static readonly string DisplayName = "FullEnhancement.Name";
        [Translate("When forming her mind blade into multiple items, the soulknife suffers no reduction in enhancement bonus.", true)]
        private static readonly string Description = "FullEnhancement.Description";
        private static readonly string Icon = "assets/icons/fullenhancement.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .Configure();
        }

    }
}
