using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class MindBoltShapeFeat
    {
        private static readonly string[] FeatNames = new[]{
            "MindBoltShapeShortFeat",
            "MindBoltShapeMediumFeat",
            "MindBoltShapeLongFeat"
        };
        private static readonly string[] FeatGUIDs = new[]{
            "e5c462f1-4e31-4d2f-a74a-797e01068c17",
            "74e1134b-779a-4d12-8cf9-bc10e0d95715",
            "c213d9e9-8319-4aca-9b33-ee6d396f717e"
        };
        public static BlueprintFeature[] BlueprintInstances = new BlueprintFeature[3];

        private static readonly string[] DisplayNames = new[]{
            "MindBoltShapeShortFeat.Name".Translate("Mind Bolt Shape (Short)"),
            "MindBoltShapeMediumFeat.Name".Translate("Mind Bolt Shape (Medium)"),
            "MindBoltShapeLongFeat.Name".Translate("Mind Bolt Shape (Long)"),
        };
        [Translate("Selected Range for the wielder's Mind Bolt.", true)]
        private static readonly string Description = "MindBoltShapeFeat.Description";

        public static BlueprintFeatureSelection SelectionInstance = null;
        private static readonly string DisplayName = "SelectMindBoltShape.Name".Translate("Select Mind Bolt Shape");

        private static readonly string Icon = "assets/icons/mindbolt.png";

        public static void Configure()
        {
            for (int fsIndex = 0; fsIndex < 3; fsIndex++)
            {
                BlueprintInstances[fsIndex] = FeatureConfigurator.New(FeatNames[fsIndex], FeatGUIDs[fsIndex])
                    .SetDisplayName(DisplayNames[fsIndex])
                    .SetDescription(Description)
                    .SetIcon(Icon)
                    .Configure();
            }

            SelectionInstance = FeatureSelectionConfigurator.New($"SelectMindBoltShape", "eb6dad32-890c-40b2-9195-7d4eedc37374")
                .SetDisplayName(DisplayName)
                .SetDescription("FormMindBoltFeat.Description")
                .SetIcon(Icon)
                .SetAllFeatures(
                    BlueprintInstances[0],
                    BlueprintInstances[1],
                    BlueprintInstances[2]
                )
                .Configure();
        }

    }
}
