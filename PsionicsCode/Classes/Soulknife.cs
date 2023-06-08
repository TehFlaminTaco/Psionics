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
                .SetClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillMobility,
                                Kingmaker.EntitySystem.Stats.StatType.SkillPersuasion,
                                Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana,
                                Kingmaker.EntitySystem.Stats.StatType.SkillPerception,
                                Kingmaker.EntitySystem.Stats.StatType.SkillStealth)
                .SetRecommendedAttributes(Kingmaker.EntitySystem.Stats.StatType.Strength,
                                          Kingmaker.EntitySystem.Stats.StatType.Dexterity)
                .SetSkillPoints(3)
                .SetHitDie(Kingmaker.RuleSystem.DiceType.D10)
                .SetBaseAttackBonus(StatProgressionRefs.BABFull.Reference.Get())
                .SetFortitudeSave(StatProgressionRefs.SavesLow.Reference.Get())
                .SetReflexSave(StatProgressionRefs.SavesHigh.Reference.Get())      // SavesHigh
                .SetWillSave(StatProgressionRefs.SavesHigh.Reference.Get())        // SavesHigh
                .SetDifficulty(2)
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
                .SetStartingItems(ItemArmorRefs.StuddedStandard.Reference.Get(),
                                  ItemEquipmentUsableRefs.PotionOfCureLightWounds.Reference.Get(),
                                  ItemEquipmentUsableRefs.PotionOfCureLightWounds.Reference.Get(),
                                  ItemEquipmentUsableRefs.PotionOfCureLightWounds.Reference.Get()
                )
                .SetStartingGold(411)
                .Configure();
            BlueprintCharacterClassReference classref = ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
            BlueprintRoot root = BlueprintTool.Get<BlueprintRoot>("2d77316c72b9ed44f888ceefc2a131f6"); // Blueprint root... yeah.
            root.Progression.m_CharacterClasses = CommonTool.Append(root.Progression.m_CharacterClasses, classref);
        }
    }
}
