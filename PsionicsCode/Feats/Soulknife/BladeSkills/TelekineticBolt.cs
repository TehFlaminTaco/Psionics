using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Psionics.Abilities.Soulknife;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class TelekineticBolt
    {
        private static readonly string FeatName = "TelekineticBolt";
        private static readonly string FeatGUID = "49fd5ace-026f-4968-bdf1-f98f685d3c8f";
        private static readonly string[] FeatGUIDs = new[] {
            "13876a0f-ec58-4b39-87b6-5aa09bdd6b1a",
            "db35f3d1-2e1f-4b45-bc84-995ed8532acf",
            "9d8f1cda-a56a-4136-953d-48be9df52d9a"
        };
        public static BlueprintFeature[] BlueprintInstances = new BlueprintFeature[3];

        [Translate("Telekinetic Bolt")]
        private static readonly string DisplayName = "TelekineticBolt.Name";
        [Translate("The soulknife learns to manifest her mind blade as a variety of ranged weapons as well as the forms her mind blade normally may take. Selection of this blade skill grants the form mind bolt and launch mind bolt class features (see soulbolt archetype). The soulknife is always considered proficient with her mind bolt. The mind bolt gains the enhancement bonus of the soulknife’s mind blade and may select its own enhancements (such as distance or flaming) as if it were a separate weapon from her mind blade. The soulknife may now also take soulbolt specific blade skills. This cannot be taken if you possess the form mind bolt class feature previously. The soulknife must be at least 4th level to select this blade skill.", true)]
        private static readonly string Description = "TelekineticBolt.Description";
        private static readonly string Icon = "assets/icons/mindbolt.png";

        public static BlueprintFeatureSelection SelectionInstance = null;

        public static void Configure()
        {
            for (int fsIndex = 0; fsIndex < 3; fsIndex++)
            {
                BlueprintInstances[fsIndex] = FeatureConfigurator.New($"TelekineticBolt{MindBoltItem.MindBoltShapes[fsIndex]}", FeatGUIDs[fsIndex])
                    .AddPrerequisiteNoFeature(FormMindBoltFeat.BlueprintInstance, false, null, false)
                    .OnConfigure(bp =>
                    {
                        PrerequisiteClassLevel prerequisiteClassLevel = new PrerequisiteClassLevel();
                        prerequisiteClassLevel.m_CharacterClass = Psionics.Classes.Soulknife.ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
                        prerequisiteClassLevel.Level = 4;
                        prerequisiteClassLevel.CheckInProgression = false;
                        bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                    })
                    .SetDisplayName($"TelekineticBolt{MindBoltItem.MindBoltShapes[fsIndex]}.Name".Translate($"Telekinetic Bolt ({MindBoltItem.MindBoltShapes[fsIndex]})"))
                    .SetDescription(Description)
                    .AddFacts(new() { FormMindBoltFeat.BlueprintInstance, MindBoltShapeFeat.BlueprintInstances[fsIndex] })
                    .SetIcon(Icon)
                    .Configure(true);
            }

            SelectionInstance = FeatureSelectionConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .OnConfigure(bp =>
                {
                    PrerequisiteClassLevel prerequisiteClassLevel = new PrerequisiteClassLevel();
                    prerequisiteClassLevel.m_CharacterClass = Psionics.Classes.Soulknife.ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
                    prerequisiteClassLevel.Level = 4;
                    prerequisiteClassLevel.CheckInProgression = false;
                    bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                })
                .SetIcon(Icon)
                .SetAllFeatures(
                    BlueprintInstances[0],
                    BlueprintInstances[1],
                    BlueprintInstances[2]
                )
                .Configure(true);
        }

    }
}
