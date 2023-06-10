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
    public class TwoHandedThrow
    {
        private static readonly string FeatName = "TwoHandedThrow";
        private static readonly string FeatGUID = "e706f000-a78a-4907-abdc-0cdfba0cd734";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Two-Handed Throw")]
        private static readonly string DisplayName = "TwoHandedThrow.Name";
        [Translate("The soulknife gains the ability to throw her mind blade if it is in two-handed form, with a range increment of 10 ft.", true)]
        private static readonly string Description = "TwoHandedThrow.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .Configure();
        }

    }
}
