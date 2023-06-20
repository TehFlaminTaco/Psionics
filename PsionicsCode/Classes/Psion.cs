using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Psionics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Psionics.Feats.Psion;

namespace Psionics.Classes
{
    public class Psion
    {
        private static readonly string ClassName = "PsionClass";
        public static readonly string ClassGUID = "6ff3c4a7-958e-487f-b0eb-fdf20084c81b";
        private static readonly string DisplayName = "PsionClass.Name".Translate("Psion");
        private static readonly string Description = "PsionClass.Description".Translate("The powers of the mind are varied and limitless, and the psion learns how to unlock them. Whether he is a shaper or a telepath, an egoist or a nomad, or even a generalist, the psion learns to manifest psionic powers that alter himself and the world around him. Due to the limited powers that any one psion knows, each psion is unique in his capabilities, as his latent abilities are drawn out and shaped into the psionic powers that define the psion.\nEach psion also gains unique abilities depending on his choice of disciplines: the egoist excels at altering his own physiology, while the nomad learns to manipulate the very fabric of space and time, and the generalist becomes a master of the overall principles of psionics, while sacrificing some of the unique abilities of the other disciplines.", true);
        public static BlueprintCharacterClass ClassBlueprint = null;

        public static void ConfigureRequirements()
        {
            PsionPowerPoints.Configure();
            PsionProficiencies.Configure();
            PsionSpellList.Configure();
            PsionSpellbook.Configure();
            PsionProgression.Configure();
        }

        public static void Configure()
        {
            ConfigureRequirements();

            ClassBlueprint = CharacterClassConfigurator.New(ClassName, ClassGUID)
                .SetLocalizedName(DisplayName)
                .SetLocalizedDescription(Description)
                .SetClassSkills(StatType.SkillPerception,
                                StatType.SkillKnowledgeArcana,
                                StatType.SkillKnowledgeWorld,
                                StatType.SkillLoreNature,
                                StatType.SkillLoreReligion
                )
                .SetRecommendedAttributes(StatType.Intelligence)
                .SetNotRecommendedAttributes(StatType.Strength, StatType.Charisma)
                .SetSkillPoints(2)
                .SetHitDie(DiceType.D6)
                .SetBaseAttackBonus(StatProgressionRefs.BABMedium.Reference.Get())
                .SetFortitudeSave(StatProgressionRefs.SavesLow.Reference.Get())
                .SetReflexSave(StatProgressionRefs.SavesLow.Reference.Get())
                .SetWillSave(StatProgressionRefs.SavesHigh.Reference.Get())
                .SetDifficulty(3)
                .SetPrimaryColor(61)
                .SetSecondaryColor(54)
                .SetProgression(PsionProgression.ProgressionBlueprint)
                .SetHideIfRestricted(false)
                .SetIsDivineCaster(true)
                .SetIsArcaneCaster(false)
                .SetIsMythic(false)
                .SetPrestigeClass(false)
                .SetSpellbook(PsionSpellbook.BlueprintInstance)
                .AddPrerequisiteIsPet(false, Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All, true, true)
                .SetMaleEquipmentEntities("5470be096a195b0408115392fc742b41", "04244d527b8a1f14db79374bc802aaaa")   // Rogue Clothes
                .SetFemaleEquipmentEntities("1e4797f3425461946a0ae2986d16c85d", "64abd9c4d6565de419f394f71a2d496f")
                .SetStartingItems(ItemWeaponRefs.StandardLightCrossbow.Reference.Get(),
                                  ItemWeaponRefs.ColdIronDagger.Reference.Get(),
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
