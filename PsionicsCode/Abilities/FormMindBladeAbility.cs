using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.MiscEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using Kingmaker.Items.Slots;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.TurnBasedMode.Controllers;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.View.Equipment;
using Microsoft.Build.Utilities;
using Psionics.Buffs;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using Kingmaker.UnitLogic;
using Psionics.Feats.Soulknife.BladeSkills;
using Psionics.Feats.Soulknife;
using Kingmaker.Blueprints.Items.Weapons;

namespace Psionics.Abilities
{
    public class FormMindBladeAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityGUID = "ddb5b99f-ccf9-4e73-af65-d184e53630a0";

        private static readonly string Description = "FormMindBladeAbility.Description";
        private static readonly string Icon = "assets/icons/formmindblade.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New($"FormMindBladeAbility", AbilityGUID)
                .SetDisplayName($"FormMindBladeAbility.Name")
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(MindBladeBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }

        [HarmonyPatch(typeof(UnitViewHandsEquipment), nameof(UnitViewHandsEquipment.HandleEquipmentSlotUpdated))]
        static class UnitViewHandsEquipment_HandleEquipmentSlotUpdated_MindBlade_Patch
        {
            static bool Prefix(UnitViewHandsEquipment __instance, HandSlot slot, ItemEntity previousItem)
            {
                //Main.Logger.Info($"HandleEquipmentSlotUpdated Change Weapon: SlotDataNull: {__instance.GetSlotData(slot) == null} Had: {MindBladeItem.TypeInstances.Contains((previousItem?.Blueprint as BlueprintItemWeapon)?.Type) && !(slot.HasWeapon && MindBladeItem.TypeInstances.Contains(slot.Weapon.Blueprint.Type))} IsPrimary: {slot.IsPrimaryHand} MindBlade: {slot.HasWeapon && MindBladeItem.TypeInstances.Contains(slot.Weapon.Blueprint.Type)}");
                if (!__instance.Active || __instance.GetSlotData(slot) == null) { return true; }
                    
                if (((MindBladeItem.TypeInstances.Contains((previousItem?.Blueprint as BlueprintItemWeapon)?.Type) && !(slot.HasWeapon && MindBladeItem.TypeInstances.Contains(slot.Weapon.Blueprint.Type))) || (__instance.Owner.HasFact(SoulknifeQuickDraw.BlueprintInstance) && (slot.IsPrimaryHand ? slot.HasWeapon && MindBladeItem.TypeInstances.Contains(slot.Weapon.Blueprint.Type) : true)))
                    && __instance.InCombat
                    && (__instance.Owner.State.CanAct || __instance.IsDollRoom)
                    && slot.Active)
                {
                    __instance.ChangeEquipmentWithoutAnimation();
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(UnitViewHandsEquipment), nameof(UnitViewHandsEquipment.HandleEquipmentSetChanged))]
        static class UnitViewHandsEquipment_HandleEquipmentSetChanged_MindBlade_Patch
        {
            static bool Prefix(UnitViewHandsEquipment __instance)
            {
                //Main.Logger.Info($"HandleEquipmentSetChanged Change Weapon! Has: {__instance.Owner.Body.PrimaryHand.HasItem} Buff: {__instance.Owner.Buffs.Enumerable.Select(c => c.Blueprint).Any(c => c == MindBladeBuff.BlueprintInstance)} MindBlade: {__instance.Owner.Body.PrimaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(__instance.Owner.Body.PrimaryHand.Weapon.Blueprint.Type)}");
                if (!__instance.Active) { return true; }
                if (((!__instance.Owner.Body.PrimaryHand.HasItem && __instance.Owner.Buffs.Enumerable.Select(c=>c.Blueprint).Any(c=>c==MindBladeBuff.BlueprintInstance)) || (__instance.Owner.HasFact(SoulknifeQuickDraw.BlueprintInstance) && __instance.Owner.Body.PrimaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(__instance.Owner.Body.PrimaryHand.Weapon.Blueprint.Type)))
                    && __instance.InCombat
                    && (__instance.Owner.State.CanAct || __instance.IsDollRoom))
                {
                    __instance.ChangeEquipmentWithoutAnimation();
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(TurnController), nameof(TurnController.CalculatePredictionForWeaponSetChange))]
        static class TurnController_CalculatePredictionForWeaponSetChange_MindBlade_Patch
        {
            static bool Prefix(TurnController __instance, bool state, bool alreadyApplyed)
            {
                //Main.Logger.Info($"TurnController Change Weapon! State: {state} Has: {__instance.SelectedUnit.Body.PrimaryHand.HasItem} Buff: {__instance.SelectedUnit.Buffs.Enumerable.Select(c => c.Blueprint).Any(c => c == MindBladeBuff.BlueprintInstance)} MindBlade: {__instance.SelectedUnit.Body.PrimaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(__instance.SelectedUnit.Body.PrimaryHand.Weapon.Blueprint.Type)}");
                if (state
                    && ((!__instance.SelectedUnit.Body.PrimaryHand.HasItem && __instance.SelectedUnit.Buffs.Enumerable.Select(c => c.Blueprint).Any(c => c == MindBladeBuff.BlueprintInstance)) || (__instance.SelectedUnit.HasFact(SoulknifeQuickDraw.BlueprintInstance) && __instance.SelectedUnit.Body.PrimaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(__instance.SelectedUnit.Body.PrimaryHand.Weapon.Blueprint.Type))))
                {
                    __instance.GetActionsStates(__instance.SelectedUnit).Clear();
                    __instance.GetActionsStates(__instance.SelectedUnit).Free
                        .SetPrediction(CombatAction.UsageType.ChangeWeapon, CombatAction.ActivityType.Ability, CombatAction.ActivityState.WillBeUsed, null, null);
                    EventBus.RaiseEvent(delegate (IActionsPredictionHandler h) {
                        h.PredictionChanged();
                    }, true);
                    return false;
                }
                return true;
            }
        }
    }
}
