using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.Items;
using Kingmaker.UI.GenericSlot;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using static Kingmaker.UI.GenericSlot.EquipSlotBase;
using BlueprintCore.Utils;
using Kingmaker.Items.Slots;

namespace Psionics.Buffs
{
    [TypeId("3ef09820-456b-4403-bc02-f6511c72cebe")]
    [AllowMultipleComponents]
    public class BuffEnchantWornItemScaling : UnitBuffComponentDelegate<BuffEnchantWornItemData>
    {
        [SerializeField]
        [FormerlySerializedAs("Enchantment")]
        public BlueprintItemEnchantmentReference m_EnchantmentBlueprint;

        public bool AllWeapons;

        [HideIf("AllWeapons")]
        public EquipSlotBase.SlotType Slot;

        private static Blueprint<BlueprintReference<BlueprintWeaponEnchantment>>[] Enchantments = new[] {
            WeaponEnchantmentRefs.Enhancement1,
            WeaponEnchantmentRefs.Enhancement2,
            WeaponEnchantmentRefs.Enhancement3,
            WeaponEnchantmentRefs.Enhancement4,
            WeaponEnchantmentRefs.Enhancement5,
            WeaponEnchantmentRefs.Enhancement6,
            WeaponEnchantmentRefs.Enhancement7,
            WeaponEnchantmentRefs.Enhancement8
        };
        public BlueprintItemEnchantment Enchantment => Enchantments[this.Buff.Rank > 7 ? 7 : this.Buff.Rank-1].Reference.Get();

        public override void OnActivate()
        {
            if (AllWeapons)
            {
                ItemEntityWeapon maybeWeapon = base.Owner.Body.PrimaryHand.MaybeWeapon;
                if (maybeWeapon != null)
                {
                    base.Data.Enchantments.Add(maybeWeapon.AddEnchantment(Enchantment, base.Context));
                }

                maybeWeapon = base.Owner.Body.SecondaryHand.MaybeWeapon;
                if (maybeWeapon != null && maybeWeapon != base.Owner.Body.PrimaryHand.MaybeWeapon)
                {
                    base.Data.Enchantments.Add(maybeWeapon.AddEnchantment(Enchantment, base.Context));
                }

                foreach (WeaponSlot additionalLimb in base.Owner.Body.AdditionalLimbs)
                {
                    maybeWeapon = additionalLimb.MaybeWeapon;
                    if (maybeWeapon != null)
                    {
                        base.Data.Enchantments.Add(maybeWeapon.AddEnchantment(Enchantment, base.Context));
                    }
                }

                return;
            }

            ItemEntity itemEntity = ((Slot == EquipSlotBase.SlotType.PrimaryHand) ? base.Owner.Body.PrimaryHand.MaybeWeapon : EquipSlotBase.ExtractSlot(Slot, base.Owner.Body).MaybeItem);
            if (itemEntity != null)
            {
                ItemEntityShield itemEntityShield = itemEntity as ItemEntityShield;
                if (itemEntityShield != null && itemEntityShield.WeaponComponent != null)
                {
                    itemEntity = itemEntityShield.WeaponComponent;
                }

                base.Data.Enchantments.Add(itemEntity.AddEnchantment(Enchantment, base.Context));
            }
        }

        public override void OnDeactivate()
        {
            foreach (ItemEnchantment enchantment in base.Data.Enchantments)
            {
                enchantment?.Owner?.RemoveEnchantment(enchantment);
            }

            base.Data.Enchantments.Clear();
        }
    }

    public class BoltBuff
    {
        public static BlueprintBuff BlueprintInstance;
        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"BoltBuff", "68d66a93-9b22-431a-af80-89302e08b488")
                .SetDisplayName("BoltBuff.Name".Translate("Bolt"))
                .SetDescription("BoltBuff.Description".Translate("You create ectoplasmic crossbow bolts, arrows, or sling bullets, appropriate to your size, which dissipate into their constituent ectoplasmic particles when the duration ends. Ammunition you create has a +1 enhancement bonus on attack rolls and damage rolls.", true))
                .SetIcon("assets/icons/bolt.png")
                .AddComponent<BuffEnchantWornItemScaling>(bewis =>
                {
                    bewis.AllWeapons = false;
                    bewis.Slot = SlotType.PrimaryHand;
                })
                .Configure();
        }
    }
}
