using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
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
    }
}
