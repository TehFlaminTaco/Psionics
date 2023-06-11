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
    public class ExpandShieldFeat
    {
        private static readonly string FeatName = "ExpandShieldFeat";
        private static readonly string FeatGUID = "5df73520-521a-438c-9241-23a6041a1fa5";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Expand Shield")]
        private static readonly string DisplayName = "ExpandShieldFeat.Name";
        [Translate("When using the total defense action or attacking defensively, as a free action on her turn the soulknife can transform her mind shield into a tower shield until the start of her next turn. All of the standard penalties for having a mind shield shaped as a tower shield apply. The soulknife must have the Mind Shield blade skill to choose this blade skill.", true)]
        private static readonly string Description = "ExpandShieldFeat.Description";
        private static readonly string Icon = "assets/icons/mindtowershield.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddPrerequisiteFeature(MindShield.BlueprintInstance, false)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(ExpandShieldAbility.BlueprintInstance, ExpandShieldAbility.BlueprintInstance, true)
                .Configure();
        }

    }
}
