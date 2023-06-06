using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities;
using Psionics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats
{
    public class PowerPointPool
    {
        private static readonly string FeatName = "PowerPointPool";
        private static readonly string FeatGUID = "246385d8-071f-4aac-b0fe-fa35ecb894e9";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Power Point Pool")]
        private static readonly string DisplayName = "PowerPointPool.Name";
        [Translate("You have a pool of Power Points")]
        private static readonly string Description = "PowerPointPool.Description";
        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .AddAbilityResources(0, PowerPoints.BlueprintInstance, true, false, false)
                .AddFeatureIfHasFact(GainPsionicFocusAbility.BlueprintInstance, GainPsionicFocusAbility.BlueprintInstance, true)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .Configure();
        }

    }
}
