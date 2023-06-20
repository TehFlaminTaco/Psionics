using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities;
using Psionics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class EnhancedMindBladeFeat
    {
        private static readonly string FeatName = "EnhancedMindBladeFeat";
        private static readonly string FeatGUID = "6b5c79d6-635b-43a7-bc34-4763bb8cac48";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Enhanced Mind Blade")]
        private static readonly string DisplayName = "EnhancedMindBladeFeat.Name";
        [Translate("A soulknife’s mind blade improves as the character gains higher levels. At 3rd level and every odd level thereafter, the mind blade gains a cumulative +1 enhancement bonus that she may spend on an actual enhancement bonus or on weapon special abilities. A soulknife’s level determines her maximum enhancement bonus. The soulknife may (and must, when her total enhancement is higher than her maximum bonus) apply any special ability instead of an enhancement bonus, as long as she meets the level requirements. A soulknife can choose any combination of weapon special abilities and/or enhancement bonus enhancement bonus before assigning any special abilities. If the soulknife shapes her mind blade into two items, the enhancement bonus of her mind blade (if any) is reduced by 1 (to a minimum of 0). If this would reduce the enhancement bonus on the mind blades to 0 and weapon special abilities are applied, the soulknife must reshape her mind blade to make the options valid. Both mind blades have the same selection of enhancement bonus that does not exceed the total allowed by the soulknife’s level, but she must assign at least a +1 and weapon special abilities (if any). This penalty does not apply when using the Mind Shield blade skill.", true)]
        private static readonly string Description = "EnhancedMindBladeFeat.Description";
        private static readonly string Icon = "assets/icons/enhancedmindblade.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetIsClassFeature(true)
                .AddAbilityResources(0, MindbladeEnhancement.BlueprintInstance, true, true, false)
                .SetRanks(10)
                .Configure();
        }
    }
}
