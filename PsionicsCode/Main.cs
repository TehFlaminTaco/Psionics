using Psionics.Feats;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using UnityModManagerNet;
using Psionics.Resources;
using Psionics.Classes;
using Psionics.Feats.Soulknife;
using System.Diagnostics;
using Kingmaker;
using System.Linq;
using Psionics.Abilities;
using Psionics.Buffs;
using Psionics.Equipment;
using System.Reflection;
using Psionics.Feats.Soulknife.BladeSkills;
using Psionics.Abilities.Soulknife;
using Psionics.Abilities.Soulknife.Bladeskills;

namespace Psionics
{
    public static class Main
    {
        public static bool Enabled;
        public static readonly LogWrapper Logger = LogWrapper.Get("Psionics");

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                modEntry.OnToggle = OnToggle;
                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll();
                Logger.Info("Finished patching.");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to patch", e);
            }


            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        [HarmonyPatch(typeof(BlueprintsCache))]
        static class BlueprintsCaches_Patch
        {
            private static bool Initialized = false;

            [HarmonyPriority(Priority.First)]
            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
            static void Init()
            {
                try
                {
                    if (Initialized)
                    {
                        Logger.Info("Already configured blueprints.");
                        return;
                    }
                    Initialized = true;

                    Logger.Info("Configuring blueprints.");

                    PsionicFocus.Configure();
                    GainPsionicFocusAbility.Configure();

                    PowerPoints.Configure();
                    PowerPointPool.Configure();

                    MindbladeEnhancement.Configure();

                    EnhanceMindBladeAbility.Configure();
                    MindbladeEnchantmentFeats.Configure();
                    EnhancedMindBladeFeat.Configure();

                    WildTalentFeat.Configure();

                    MindBladeItem.Configure();
                    SoulknifeProficiencies.Configure();
                    MindBladeBuff.Configure();
                    MindBladeShapeBuff.Configure();
                    FormMindBladeAbility.Configure();
                    ShapeMindBladeAbility.Configure();
                    ShapeMindBladeFeat.Configure();
                    TwinMindBlade.Configure();
                    FormMindBladeFeat.Configure();
                    ThrowMindBladeAbility.Configure();
                    SoulknifeKineticBlastFeature.Configure();
                    SoulKnifeBonusFeat.Configure();
                    SoulknifeQuickDraw.Configure();
                    PsychicStrikeBuff.Configure();
                    PsychicStrikeAbility.Configure();
                    PsychicStrikeFeat.Configure();

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
                    MindShield.Configure();

                    BladeSkillsFeat.Configure();

                    SoulknifeProgression.Configure();
                    Soulknife.Configure();

                    Logger.Info("Done! Successfully?");
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to configure blueprints.", e);
                }
            }
        }

        [HarmonyPatch(typeof(StartGameLoader))]
        static class StartGameLoader_Patch
        {
            private static bool Initialized = false;

            [HarmonyPatch(nameof(StartGameLoader.LoadPackTOC)), HarmonyPostfix]
            static void LoadPackTOC()
            {
                try
                {
                    if (Initialized)
                    {
                        Logger.Info("Already configured delayed blueprints.");
                        return;
                    }
                    Initialized = true;
                    RootConfigurator.ConfigureDelayedBlueprints();
                    Debug.Assert(Game.Instance.BlueprintRoot.Progression.m_CharacterClasses.Any(c => c.Get().AssetGuid.Equals(Soulknife.ClassBlueprint.AssetGuid)), "Failed to register Soulknife!");
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to configure delayed blueprints.", e);
                }

                Logger.Info("Adding Translations!");
                int translationsCount = 0;
                try
                {
                    foreach (var c in typeof(Main).Assembly.GetTypes().SelectMany(c => c.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)))
                    {
                        var translation = c.GetCustomAttribute<TranslateAttribute>();
                        if (translation != null && typeof(string).IsAssignableFrom(c.FieldType))
                        {
                            if (translation.EnGB is null || string.IsNullOrEmpty(c.GetValue(null) as string))
                            {
                                Logger.Info("Failed to add translation for: " + c.GetValue(null));
                                continue;
                            }
                            LocalizationTool.CreateString((string)c.GetValue(null), translation.EnGB, translation.AddEncyclopedia);
                            translationsCount++;
                        }
                        else if (translation != null)
                        {
                            Logger.Info($"Can't translate non-string field {c.Name}!");
                        }
                    }
                    foreach (var c in typeof(Main).Assembly.GetTypes().SelectMany(c => c.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)))
                    {
                        var translation = c.GetCustomAttribute<TranslateAttribute>();
                        if (translation != null && typeof(string).IsAssignableFrom(c.PropertyType))
                        {
                            if (translation.EnGB is null || string.IsNullOrWhiteSpace(c.GetValue(null) as string))
                            {
                                Logger.Info("Failed to add translation for: " + c.GetValue(null));
                                continue;
                            }
                            LocalizationTool.CreateString((string)c.GetValue(null), translation.EnGB, translation.AddEncyclopedia);
                            translationsCount++;
                        }
                        else if (translation != null)
                        {
                            Logger.Info($"Can't translate non-string property {c.Name}!");
                        }
                    }
                    foreach (var c in TranslateHelper.translations)
                    {
                        LocalizationTool.CreateString(c.Key, c.Value.Item1, c.Value.Item2);
                    }
                }
                catch (Exception e)
                {
                    Logger.Info("Failed adding translations!\n" + e);
                }
                Logger.Info($"Added {translationsCount} Translations!");
            }
        }
    }
}
