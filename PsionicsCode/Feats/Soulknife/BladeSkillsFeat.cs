using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Psionics.Feats.Soulknife.BladeSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class BladeSkillsFeat
    {
        private static readonly string FeatName = "BladeSkillsFeat";
        private static readonly string FeatGUID = "b2695eaa-86b2-420a-8ad9-05b9bf0874c9";
        public static BlueprintFeatureSelection BlueprintInstance = null;

        [Translate("Blade Skill")]
        private static readonly string DisplayName = "BladeSkillsFeat.Name";
        [Translate("Beginning at 2nd level and every even soulknife level thereafter, a soulknife may choose one of a number of abilities to add to her repertoire. Some blade skills have prerequisites that must be met before they can be chosen. All blade skills may only be chosen once and require the soulknife to be using her mind blade unless otherwise stated in the skill’s description.")]
        private static readonly string Description = "BladeSkillsFeat.Description"; 

        public static void Configure()
        {
            BlueprintInstance = FeatureSelectionConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetAllFeatures(
                    AlterBladeFeat.BlueprintInstance,
                    BladeRushFeat.BlueprintInstance,
                    BladestormFeat.BlueprintInstance,
                    BladewindFeat.BlueprintInstance,
                    MindShield.BlueprintInstance
                )
                .SetIsClassFeature(true)
                .Configure();
        }


    }
}
