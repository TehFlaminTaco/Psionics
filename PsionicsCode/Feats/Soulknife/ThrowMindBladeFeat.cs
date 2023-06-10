using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities.Soulknife;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class ThrowMindBladeFeat
    {
        private static readonly string FeatName = "ThrowMindBladeFeat";
        private static readonly string FeatGUID = "fb01a2d7-ac06-494c-aad2-e082961332f9";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Throw Mind Blade")]
        private static readonly string DisplayName = "ThrowMindBladeFeat.Name";
        [Translate("All soulknives have some knowledge of how to throw their mind blades, though the range increment varies by form and the largest of blade forms cannot be thrown. Light weapon mind blades have a range increment of 20 ft. One-handed weapon mind blades have a range increment of 15 ft. Two-handed weapon mind blades cannot be thrown without the Two-Handed Throw blade skill. Whether or not the attack hits, a thrown mind blade then dissipates.", true)]
        private static readonly string Description = "ThrowMindBladeFeat.Description";
        private static readonly string Icon = "assets/icons/throwmindblade.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(ThrowMindBladeAbility.BlueprintInstance, ThrowMindBladeAbility.BlueprintInstance, true)
                .Configure();
        }

    }
}
