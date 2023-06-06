using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class ShapeMindBladeFeat
    {
        private static readonly string FeatName = "ShapeMindBladeFeat";
        private static readonly string FeatGUID = "71b9e85e-71c6-473d-8c44-86f337152a12";
        public static BlueprintFeature BlueprintInstance = null;

        private static readonly string DisplayName = "ShapeMindBladeFeat.Name";
        private static readonly string Description = "ShapeMindBladeFeat.Description";
        private static readonly string Icon = "assets/icons/shapemindblade.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetIsClassFeature(true)
                .AddFeatureIfHasFact(ShapeMindBladeAbility.BlueprintInstance, ShapeMindBladeAbility.BlueprintInstance, true)
                .Configure();
        }
    }
}
