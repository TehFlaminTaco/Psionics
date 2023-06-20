using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using HarmonyLib;
using Kingmaker.Items.Slots;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.View.Equipment;
using Psionics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Blueprints;
using UnityEngine.Serialization;
using Psionics.Resources;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker;
using static Kingmaker.Visual.CharacterSystem.CharacterStudio;
using Psionics.Buffs;

namespace Psionics.Powers
{
    [ComponentName("Costs Power Points")]
    [TypeId("d5c9becf-bca1-43b9-b2f7-3426d4096c36")]
    public class AbilityCostPowerpoints : BlueprintComponent
    {
        public Augmentable AugmentableHolder = null;
        [FormerlySerializedAs("Cost")]
        public int m_Cost = 1;
    }

    [TypeId("dd67d2b3-499e-4252-95ac-002e557c01e4")]
    public class ConstantCountText : BlueprintComponent
    {
        public int Text;
    }

    [HarmonyPatch(typeof(SlotConversion), nameof(SlotConversion.GetMechanicSlots))]
    static class SlotConversion_GetMechanicSlots_OnlyShowVisibleAbilities
    {
        static void Postfix(SlotConversion __instance, ref IEnumerable<MechanicActionBarSlot> __result)
        {
            __result = __result.Where(c => c is MechanicActionBarSlotSpontaneusConvertedSpell mabsscs ? mabsscs.Spell.IsVisible() : true);
        }
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.SpendFromSpellbook))]
    static class AbilityCastRateUtils_SpendResources_HackyAlternativeCosts
    {
        static bool Prefix(AbilityData __instance)
        {
            AbilityCostPowerpoints spender;
            if ((spender = __instance.Blueprint?.GetComponent<AbilityCostPowerpoints>()) != null)
            {
                __instance.Caster.Resources.Spend(PowerPoints.BlueprintInstance, spender.AugmentableHolder.GetCost(__instance.Caster));
                return false;
            }
            if (__instance.Blueprint?.GetComponent<ConstantCountText>() != null)
                return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(AbilityCastRateUtils), nameof(AbilityCastRateUtils.GetAvailableForCastCount))]
    static class AbilityCastRateUtils_GetAvailableCastCount_PowerpointPatch
    {
        static bool Prefix(ref int __result, AbilityData ability)
        {
            AbilityCostPowerpoints spender;
            if ((spender = ability.Blueprint?.GetComponent<AbilityCostPowerpoints>())!=null)
            {
                int augmentCost = spender.m_Cost;
                if(spender.AugmentableHolder != null)
                {
                    augmentCost = spender.AugmentableHolder.GetCost(ability.Caster);
                }
                int pointsLeft = ability.Caster.Resources.GetResourceAmount(PowerPoints.BlueprintInstance);
                __result = pointsLeft / augmentCost;
                return false;
            }
            ConstantCountText count;
            if((count = ability.Blueprint?.GetComponent<ConstantCountText>()) != null)
            {
                __result = count.Text;
                return false;
            }

            return true;
        }
    }
}
