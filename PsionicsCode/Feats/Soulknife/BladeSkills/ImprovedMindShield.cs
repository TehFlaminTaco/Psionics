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
    public class ImprovedMindShield
    {
        private static readonly string FeatName = "ImprovedMindShield";
        private static readonly string FeatGUID = "3fafb2d4-86ba-45df-98bf-12137e145f00";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Improved Mind Shield")]
        private static readonly string DisplayName = "ImprovedMindShield.Name";
        [Translate("The shield bonus to AC granted by the soulknife’s mind shield increases by 1. The soulknife must have the Mind Shield blade skill in order to select this blade skill.", true)]
        private static readonly string Description = "ImprovedMindShield.Description";
        private static string Icon = "assets/icons/mindshield.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddPrerequisiteFeature(MindShield.BlueprintInstance)
                .SetIcon(Icon)
                .Configure();
        }

    }
}
