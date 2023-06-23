using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._PCView.GroupChanger;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Psionics.Buffs;
using Psionics.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics
{
    public static class PsionicsHelpers
    {
        public static bool TryGet<A,B>(this UnitEntityData unit, A bp, out B fact) where A : BlueprintUnitFact where B : UnitFact
        {
            if (unit.HasFact(bp))
            {
                fact = unit.GetFact<B>(bp);
            }
            fact = null;
            return false;
        }
        public static bool TryGetFeature(this UnitEntityData unit, BlueprintFeature bp, out Feature fact)
        {
            fact = unit.GetFeature(bp);
            return fact != null;
        }

        public static string GetFocusType(this UnitEntityData unit)
        {
            for(int i=0; i < 4; i++)
            {
                if (unit.Buffs.GetBuff(ElementalFocus.BlueprintInstances[i]) != null)
                    return ElementalFocus.ElementNames[i];
            }
            return "None";
        }

        public static WeaponVisualParameters Clone(this WeaponVisualParameters th)
        {
            return new()
            {
                m_Projectiles = th.m_Projectiles,
                m_WeaponAnimationStyle = th.m_WeaponAnimationStyle,
                m_SpecialAnimation = th.m_SpecialAnimation,
                m_WeaponModel = th.m_WeaponModel,
                m_WeaponBeltModelOverride = th.m_WeaponBeltModelOverride,
                m_WeaponSheathModelOverride = th.m_WeaponSheathModelOverride,
                m_OverrideAttachSlots = th.m_OverrideAttachSlots,
                m_PossibleAttachSlots = th.m_PossibleAttachSlots,
                m_ReachFXThresholdBonus = th.m_ReachFXThresholdBonus,
                m_CachedBeltModel = th.m_CachedBeltModel,
                m_CachedSheathModel = th.m_CachedSheathModel,
                m_CachedEquipLinksUpToDate = th.m_CachedEquipLinksUpToDate,
                m_SoundSize = th.m_SoundSize,
                m_SoundType = th.m_SoundType,
                m_WhooshSound = th.m_WhooshSound,
                m_MissSoundType = th.m_MissSoundType,
                m_EquipSound = th.m_EquipSound,
                m_UnequipSound = th.m_UnequipSound,
                m_InventoryEquipSound = th.m_InventoryEquipSound,
                m_InventoryPutSound = th.m_InventoryPutSound,
                m_InventoryTakeSound = th.m_InventoryTakeSound
            };
        }

        public static Dictionary<string, string> cachedGUIDS = new();
        public static string HashGUID(this string t)
        {
            if (cachedGUIDS.TryGetValue(t, out string cached))
            {
                Psionics.HashGUID.Last = cached;
                return t;
            }
            var rng = new Random(t.GetHashCode());
            var bytes = new byte[16];
            rng.NextBytes(bytes);
            var guid = new Guid(bytes).ToString();
            if (cachedGUIDS.ContainsValue(guid))
            {
                throw new Exception($"HashGUID Collision! Collides between {t} and {cachedGUIDS.First(c => c.Value == guid).Key}");
            }
            Psionics.HashGUID.Last = guid;
            cachedGUIDS[t] = guid;
            return t;
        }

        public static AbilityConfigurator Augmentable(this AbilityConfigurator ac, Action<Augmentable> action)
        {
            var augmentable = new Augmentable();
            if (action != null) action(augmentable);
            ac.AddComponent(augmentable);
            ac.AddAbilityVariants(augmentable.Configure().Append(augmentable.Castable).Select(c => (BlueprintCore.Utils.Blueprint<BlueprintAbilityReference>)c.ToReference<BlueprintAbilityReference>()).ToList());
            return ac;
        }
    }

    public static class HashGUID
    {
        public static string Last;
    }
}
