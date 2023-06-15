using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class PsionicTraining
    {
        private static readonly string FeatName = "PsionicTraining";
        private static readonly string FeatGUID = "ecb94f90-ac2a-4dba-946b-eaaa937cf00d";
        public static BlueprintFeatureSelection BlueprintInstance = null;

        [Translate("Psionic Training")]
        private static readonly string DisplayName = "PsionicTraining.Name";
        [Translate("Gain a psionic feat instead of selecting a blade skill. The soulknife must meet all prerequisites of the psionic feat selected. She may not select the feat Extra Blade Skill with this blade skill. This blade skill may be selected multiple times.", true)]
        private static readonly string Description = "PsionicTraining.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureSelectionConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetRanks(10)
                .SetAllFeatures(Main.PsionicFeats.Where(c=>c != ExtraBladeSkill.BlueprintInstance).Select(c=>(Blueprint<BlueprintFeatureReference>)c).ToArray())
                .Configure();
        }

    }
}
