using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Psionics.Feats.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Classes
{
    public class Soulknife
    {
        private static readonly string ClassName = "SoulknifeClass";
        public static readonly string ClassGUID = "c8cf221e-ccd0-4a2b-96c0-261efd0c7e79";
        private static readonly string DisplayName = "Soulknife.Name";
        private static readonly string Description = "Soulknife.Description";
        public static BlueprintCharacterClass ClassBlueprint = null;

        public static void Configure()
        {
            ClassBlueprint = CharacterClassConfigurator.New(ClassName, ClassGUID)
                .SetLocalizedName(DisplayName)
                .SetLocalizedDescription(Description)
                .SetLocalizedDescriptionShort(Description)
                .SetClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillMobility,
                                Kingmaker.EntitySystem.Stats.StatType.SkillPersuasion,
                                Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana,
                                Kingmaker.EntitySystem.Stats.StatType.SkillPerception,
                                Kingmaker.EntitySystem.Stats.StatType.SkillStealth)
                .SetRecommendedAttributes(Kingmaker.EntitySystem.Stats.StatType.Strength,
                                          Kingmaker.EntitySystem.Stats.StatType.Dexterity)
                .SetSkillPoints(3)
                .SetHitDie(Kingmaker.RuleSystem.DiceType.D10)
                .SetBaseAttackBonus("b3057560ffff3514299e8b93e7648a9d") // BABFull
                .SetFortitudeSave("dc0c7c1aba755c54f96c089cdf7d14a3")   // SavesLow
                .SetReflexSave("ff4662bde9e75f145853417313842751")      // SavesHigh
                .SetWillSave("ff4662bde9e75f145853417313842751")        // SavesHigh
                .SetDifficulty(1)
                .SetPrimaryColor(61)
                .SetSecondaryColor(54)
                .SetProgression(SoulknifeProgression.ProgressionBlueprint)
                .SetHideIfRestricted(false)
                .SetIsDivineCaster(false)
                .SetIsArcaneCaster(false)
                .SetIsMythic(false)
                .SetPrestigeClass(false)
                .AddPrerequisiteIsPet(false, Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All, true, true)
                .SetMaleEquipmentEntities("1f538abc2802c5649b7ce177183f88c8", "54de61e669f916543b96da841357d2ff")   // Rogue Clothes
                .SetFemaleEquipmentEntities("dc822f0446c675a45809202953fa52a7", "67d82fc7662a522449d5dc8ed622e33a")
                .SetStartingItems("afbe88d27a0eb544583e00fa78ffb2c7", // Studded Leather
                                  "d52566ae8cbe8dc4dae977ef51c27d91", // Potion of Cure Light Wounds
                                  "d52566ae8cbe8dc4dae977ef51c27d91", // ^
                                  "d52566ae8cbe8dc4dae977ef51c27d91"  // ^

                )
                .SetStartingGold(411)
                .Configure();
            BlueprintCharacterClassReference classref = ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
            BlueprintRoot root = BlueprintTool.Get<BlueprintRoot>("2d77316c72b9ed44f888ceefc2a131f6"); // Blueprint root... yeah.
            root.Progression.m_CharacterClasses = CommonTool.Append(root.Progression.m_CharacterClasses, classref);
        }
    }
}
