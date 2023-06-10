using HarmonyLib;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using Kingmaker.View.Equipment;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics
{
    public interface IAbilityCustomRange
    {
        public Feet GetRange(AbilityData data, bool reach);
    }

    [HarmonyPatch(typeof(BlueprintAbility), nameof(BlueprintAbility.GetRange))]
    static class BlueprintAbility_GetRange_BladeRush_Patch
    {
        static bool Prefix(UnitViewHandsEquipment __instance, ref Feet __result, ref bool reach, ref AbilityData abilityData)
        {
            if (abilityData.Blueprint == BladeRushAbility.BlueprintInstance)
            {
                __result = BlueprintAbility.GetDoubleMoveRange(abilityData) / 2;
                return false;
            }

            foreach (var handler in abilityData.Blueprint.Components.OfType<IAbilityCustomRange>())
            {
                var res = handler.GetRange(abilityData, reach);
                if (res.Value < 0) continue;
                __result = res;
                return false;
            }

            return true;
        }
    }
}
