﻿using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    [ComponentName("Mind Shield AC Bonus")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("575381ab-dc4f-4430-a0d4-0d455cf3a50d")]
    public class MindShieldBonus : UnitFactComponentDelegate, IUnitActiveEquipmentSetHandler, IGlobalSubscriber, ISubscriber, IUnitEquipmentHandler
    {
        public override void OnTurnOn()
        {
            this.CheckShield();
        }
        public override void OnTurnOff()
        {
            this.DeactivateModifier();
        }
        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            if (base.Owner == unit.Unit)
            {
                this.CheckShield();
            }
        }
        private void CheckShield()
        {
            if (!base.Owner.Body.SecondaryHand.HasItem || !base.Owner.Body.PrimaryHand.HasItem)
            {
                this.ActivateModifier();
                return;
            }
            this.DeactivateModifier();
        }
        private void ActivateModifier()
        {
            base.Owner.Stats.AC.AddModifierUnique(2, base.Runtime, ModifierDescriptor.Shield);
        }
        private void DeactivateModifier()
        {
            base.Owner.Stats.AC.RemoveModifiersFrom(base.Runtime);
        }
        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            if (slot.Owner != base.Owner)
            {
                return;
            }
            this.CheckShield();
        }
    }

    public class MindShield
    {
        private static readonly string FeatName = "MindShield";
        private static readonly string FeatGUID = "0ce18460-341d-4387-b1d6-81b0d92d74bd";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Mind Shield")]
        private static readonly string DisplayName = "MindShield.Name";
        [Translate("The soulknife gains a +2 shield bonus to armor class, as long as she has a hand free.", true)]
        private static readonly string Description = "MindShield.Description";

        private static string Icon = "assets/icons/mindshield.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<MindShieldBonus>()
                .Configure();
        }

    }
}
