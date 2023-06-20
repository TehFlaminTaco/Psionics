using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Abilities.Soulknife;
using Psionics.Buffs;
using Psionics.Equipment;
using Psionics.Feats.Soulknife;
using Psionics.Feats.Soulknife.BladeSkills;
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

        public static void ConfigureRequirements()
        {
            MindBladeItem.Configure();
            MindBoltItem.Configure();
            SoulknifeProficiencies.Configure();
            MindBladeBuff.Configure();
            MindBladeShapeBuff.Configure();
            MindBoltBuff.Configure();
            MindBoltShapeFeat.Configure();
            FormMindBladeAbility.Configure();
            FormMindBoltAbility.Configure();
            ShapeMindBladeAbility.Configure();
            ShapeMindBladeFeat.Configure();
            TwinMindBlade.Configure();
            FormMindBladeFeat.Configure();
            FormMindBoltFeat.Configure();
            ThrowMindBladeAbility.Configure();
            ThrowMindBladeFeat.Configure();
            SoulknifeKineticBlastFeature.Configure();
            SoulKnifeBonusFeat.Configure();
            SoulknifeQuickDraw.Configure();
            PsychicStrikeBuff.Configure();
            PsychicStrikeAbility.Configure();
            PsychicStrikeFreeAbility.Configure();
            PsychicStrikeFeat.Configure();

            // Pulled up because a few blade skills need this.
            MindShield.Configure();

            ShapeMindBladeFreeAbility.Configure();
            AlterBladeFeat.Configure();
            BladeRushBuff.Configure();
            BladeRushAbility.Configure();
            BladeRushFeat.Configure();
            BladestormAbility.Configure();
            BladestormFeat.Configure();
            BladewindSpendPsionicStrikeAbility.Configure();
            BladewindAbility.Configure();
            BladewindFeat.Configure();
            DazzlingBladeAbility.Configure();
            DazzlingBladeFeat.Configure();
            DeadlyBlow.Configure();
            DispellingStrikeAbility.Configure();
            DispellingStrikeFeat.Configure();
            DisruptedBuff.Configure();
            DisruptingStrikeAbility.Configure();
            DisruptingStrikeFeat.Configure();
            DuelingBladeBuff.Configure();
            DuelingBladeAbility.Configure();
            DuelingBladeFeat.Configure();
            EnhancedRange.Configure();
            MindTowerShieldItem.Configure();
            MindTowerShieldBuff.Configure();
            ExpandShieldAbility.Configure();
            ExpandShieldFeat.Configure();
            ExplodingCriticalBuff.Configure();
            ExplodingCriticalAbility.Configure();
            ExplodingCriticalFeat.Configure();
            FireBladeBuff.Configure();
            FireBladeSpendFocusBuff.Configure();
            FireBladeAbility.Configure();
            FireBladeSpendFocusAbility.Configure();
            FireBladeFeat.Configure();
            FullEnhancement.Configure();
            GruesomeRiposteBuff.Configure();
            GruesomeRiposteAbility.Configure();
            GruesomeRiposteFeat.Configure();
            IceBladeSlowedBuff.Configure();
            IceBladeBuff.Configure();
            IceBladeSpendFocusBuff.Configure();
            IceBladeAbility.Configure();
            IceBladeSpendFocusAbility.Configure();
            IceBladeFeat.Configure();
            ImprovedEnhancement.Configure();
            ImprovedMindShield.Configure();
            LaunchMultibolt.Configure();
            LightningBladeDebuff.Configure();
            LightningBladeBuff.Configure();
            LightningBladeSpendFocusBuff.Configure();
            LightningBladeAbility.Configure();
            LightningBladeSpendFocusAbility.Configure();
            LightningBladeFeat.Configure();
            MindBladeFinesse.Configure();
            PowerfulStrikes.Configure();
            PsionicTraining.Configure();
            ReachingBladeBuff.Configure();
            ReachingBladeAbility.Configure();
            ReachingBlade.Configure();
            ReapersBladeFeat.Configure();
            RendingBlades.Configure();
            TelekineticBolt.Configure();
            ThunderBladeDebuff.Configure();
            ThunderBladeBuff.Configure();
            ThunderBladeSpendFocusBuff.Configure();
            ThunderBladeAbility.Configure();
            ThunderBladeSpendFocusAbility.Configure();
            ThunderBladeFeat.Configure();
            FormTowerMindShield.Configure();
            TowerMindShieldFeat.Configure();
            TwoHandedThrow.Configure();

            BladeSkillsFeat.Configure();

            SoulknifeProgression.Configure();
        }

        public static void Configure()
        {
            ConfigureRequirements();
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
                .SetSignatureAbilities(FormMindBladeFeat.FormMindBladeBlueprint, BladeSkillsFeat.BlueprintInstance)
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
