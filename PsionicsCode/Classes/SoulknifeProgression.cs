using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Buffs;
using Psionics.Feats;
using Psionics.Feats.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Classes
{
    public class SoulknifeProgression
    {
        private static readonly string ProgressionName = "SoulknifeProgression";
        private static readonly string ProgressionGUID = "89e77641-3510-44a1-9d95-555f3ce6f60c";
        public static BlueprintProgression ProgressionBlueprint = null;

        public static void Configure()
        {
            ProgressionBlueprint = ProgressionConfigurator.New(ProgressionName, ProgressionGUID)
               .SetLevelEntry(01, SoulknifeProficiencies.SoulknifeProficienciesBlueprint,
                                 FormMindBladeFeat.FormMindBladeBlueprint,
                                 ShapeMindBladeFeat.BlueprintInstance,
                                 SoulKnifeBonusFeat.BlueprintInstance,
                                 WildTalentFeat.BlueprintInstance
               )
               .SetLevelEntry(02, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(03, PsychicStrikeFeat.BlueprintInstance)
               .SetLevelEntry(04, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(05, SoulknifeQuickDraw.BlueprintInstance)
               .SetLevelEntry(06, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(07, PsychicStrikeFeat.BlueprintInstance)
               .SetLevelEntry(08, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(10, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(11, PsychicStrikeFeat.BlueprintInstance)
               .SetLevelEntry(12, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(14, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(15, PsychicStrikeFeat.BlueprintInstance)
               .SetLevelEntry(16, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(18, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(19, PsychicStrikeFeat.BlueprintInstance)
               .SetLevelEntry(20, BladeSkillsFeat.BlueprintInstance)
               .Configure();
        }
    }
}
