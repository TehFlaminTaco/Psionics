using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Psionics.Buffs;
using Psionics.Feats;
using Psionics.Feats.Soulknife;
using Psionics.Feats.Soulknife.BladeSkills;
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
               .OnConfigure(c=>c.m_Classes = new[]{new BlueprintProgression.ClassWithLevel()
               {
                   m_Class = Soulknife.ClassBlueprint.ToReference<BlueprintCharacterClassReference>(),
                   AdditionalLevel = 0
               }})
               .SetLevelEntry(01, SoulknifeProficiencies.SoulknifeProficienciesBlueprint,
                                 FormMindBladeFeat.FormMindBladeBlueprint,
                                 ShapeMindBladeFeat.BlueprintInstance,
                                 SoulKnifeBonusFeat.BlueprintInstance,
                                 WildTalentFeat.BlueprintInstance,
                                 ThrowMindBladeFeat.BlueprintInstance
               )
               .SetLevelEntry(02, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(03, PsychicStrikeFeat.BlueprintInstance, EnhancedMindBladeFeat.BlueprintInstance)
               .SetLevelEntry(04, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(05, SoulknifeQuickDraw.BlueprintInstance, EnhancedMindBladeFeat.BlueprintInstance, MindbladeEnchantmentFeats.BlueprintInstances[0])
               .SetLevelEntry(06, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(07, PsychicStrikeFeat.BlueprintInstance, EnhancedMindBladeFeat.BlueprintInstance)
               .SetLevelEntry(08, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(09, EnhancedMindBladeFeat.BlueprintInstance, MindbladeEnchantmentFeats.BlueprintInstances[1])
               .SetLevelEntry(10, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(11, PsychicStrikeFeat.BlueprintInstance, EnhancedMindBladeFeat.BlueprintInstance)
               .SetLevelEntry(12, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(13, EnhancedMindBladeFeat.BlueprintInstance)
               .SetLevelEntry(14, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(15, PsychicStrikeFeat.BlueprintInstance, EnhancedMindBladeFeat.BlueprintInstance, MindbladeEnchantmentFeats.BlueprintInstances[2])
               .SetLevelEntry(16, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(17, EnhancedMindBladeFeat.BlueprintInstance)
               .SetLevelEntry(18, BladeSkillsFeat.BlueprintInstance)
               .SetLevelEntry(19, PsychicStrikeFeat.BlueprintInstance, EnhancedMindBladeFeat.BlueprintInstance)
               .SetLevelEntry(20, BladeSkillsFeat.BlueprintInstance)
               .Configure(true);
        }
    }
}
