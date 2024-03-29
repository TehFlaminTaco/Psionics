﻿using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Class.Kineticist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker;
using Owlcat.QA.Validation;
using Kingmaker.Blueprints.JsonSystem;
using Psionics.Equipment;
using Psionics.Feats.Soulknife;
using Newtonsoft.Json;
using Psionics.Feats.Soulknife.BladeSkills;
using Psionics.Resources;
using BlueprintCore.Utils;
using Psionics.Abilities.Soulknife;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.EntitySystem;
using static Psionics.Buffs.MindBoltBuff;

namespace Psionics.Buffs
{
    public class MindBladeBuff
    {
        public class AddMindBladeData
        {
            [JsonProperty]
            public ItemEntityWeapon Applied;
            [JsonProperty]
            public ItemEntityWeapon Twin;


            [JsonProperty]
            public int m_Enhancement;
            [JsonProperty]
            public string[] m_Enchantments;
        }

        [TypeId("9f60c4c8-bdca-4e20-8cba-cb2e755257d3")]
        public class AddMindBlades : UnitBuffComponentDelegate<AddMindBladeData>, IAreaActivationHandler, IGlobalSubscriber, ISubscriber, IUnitBuffHandler
        {
            public override void OnActivate()
            {
                if (base.Owner.TryGet(FormMindBoltAbility.BlueprintInstance, out ActivatableAbility ab) && ab.IsTurnedOn)
                    ab.TurnOffImmediately();
                if (Data.m_Enchantments == null) // Hasn't been initialized yet!
                    this.HandleBuffDidAdded(this.Fact as Buff);
                base.OnActivate();
                if (!base.Owner.HasMoveAction() && !base.Owner.HasFact(SoulknifeQuickDraw.BlueprintInstance))
                {
                    var formAbility = this.Owner.ActivatableAbilities.Enumerable.Where(c => c.Blueprint == FormMindBladeAbility.BlueprintInstance).FirstOrDefault();
                    if (formAbility is not null && formAbility.IsOn)
                        formAbility.IsTurnedOn = false;
                    return;
                }
                base.Owner.MarkNotOptimizableInSave();
                BlueprintItemWeapon Blade = MindBladeItem.BlueprintInstances[1];
                var Shape = "Sword";
                for(int fsIndex = 0; fsIndex < 3; fsIndex++)
                {
                    if (base.Owner.HasFact(MindBladeShapeBuff.BlueprintInstances[fsIndex]))
                    {
                        Shape = ShapeMindBladeAbility.ShapeNames[fsIndex];
                        Blade = MindBladeItem.BlueprintInstances[fsIndex];
                        break;
                    }
                }

                base.Data.Applied = Blade.CreateEntity<ItemEntityWeapon>();
                base.Data.Applied.MakeNotLootable();
                var twinMindBlade = base.Owner.ActivatableAbilities.Enumerable.Where(c => c.Blueprint == TwinMindBlade.BlueprintInstance).FirstOrDefault();
                bool twinned = twinMindBlade is not null && twinMindBlade.IsOn;
                Enchant(base.Data.Applied, twinned);
                if (!base.Owner.Body.PrimaryHand.CanInsertItem(base.Data.Applied))
                {
                    base.Data.Applied = null;
                    PFLog.Default.Error("Can't insert mind blade to main hand");
                    return;
                }
                var hasAlterBlade = base.Owner.HasFact(AlterBladeFeat.BlueprintInstance);
                if(Shape == "Light" || (Shape == "Sword" && hasAlterBlade))
                {
                    if(twinned)
                    {
                        base.Data.Twin = MindBladeItem.BlueprintInstances[0].CreateEntity<ItemEntityWeapon>();
                        base.Data.Twin.MakeNotLootable();
                        Enchant(base.Data.Twin, twinned);
                    }
                }
                using (ContextData<ItemsCollection.SuppressEvents>.Request())
                {
                    base.Owner.Body.PrimaryHand.InsertItem(base.Data.Applied);
                    if (base.Data.Twin is not null && base.Owner.Body.SecondaryHand.CanInsertItem(base.Data.Twin))
                    {
                        base.Owner.Body.SecondaryHand.InsertItem(base.Data.Twin);
                    }
                    else if(base.Data.Twin is not null)
                    {
                        base.Data.Twin.Collection?.Remove(base.Data.Twin);
                        base.Data.Twin = null;
                    }
                }
            }
            public override void OnDeactivate()
            {
                base.OnDeactivate();
                if (base.Data.Applied != null)
                {
                    base.Data.Applied.HoldingSlot?.RemoveItem();
                    using (ContextData<ItemsCollection.SuppressEvents>.Request())
                    {
                        base.Data.Applied.Collection?.Remove(base.Data.Applied);
                    }

                    base.Data.Applied = null;
                }
                if(base.Data.Twin != null)
                {
                    base.Data.Twin.HoldingSlot?.RemoveItem();
                    using (ContextData<ItemsCollection.SuppressEvents>.Request())
                    {
                        base.Data.Twin.Collection?.Remove(base.Data.Twin);
                    }
                    base.Data.Twin = null;
                }
            }

            public void Enchant(ItemEntityWeapon wep, bool twinned)
            {
                int sparePoints = Data.m_Enhancement;
                if (twinned) sparePoints--;
                if (sparePoints <= 0)
                {
                    return;
                }
                foreach (var ability in Data.m_Enchantments)
                {
                    wep.AddEnchantment(BlueprintTool.GetRef<BlueprintItemEnchantmentReference>(ability).Get(), Context, null);
                }
                wep.AddEnchantment(BlueprintTool.GetRef<BlueprintItemEnchantmentReference>(EnhanceMindBladeAbility.Enhancement[Math.Min(sparePoints - 1, 4)]).Get(), Context, null);
            }

            public override void ApplyValidation(ValidationContext context, int parentIndex)
            {
                base.ApplyValidation(context, parentIndex);
            }

            public override void OnTurnOn()
            {
                base.Data.Applied?.HoldingSlot.Lock.Retain();
                base.Data.Twin?.HoldingSlot.Lock.Retain();
            }

            public override void OnTurnOff()
            {
                base.Data.Applied?.HoldingSlot.Lock.Release();
                base.Data.Twin?.HoldingSlot.Lock.Release();
            }

            public void OnAreaActivated()
            {
                if (base.Data.Applied == null)
                {
                    OnActivate();
                    OnTurnOn();
                }
            }

            public void HandleBuffDidAdded(Buff buff)
            {
                if (buff == this.Fact)
                {
                    var data = buff.GetData<AddMindBlades, AddMindBladeData>();
                    if (!buff.Owner.HasFact(EnhancedMindBladeFeat.BlueprintInstance))
                    {
                        data.m_Enhancement = 0;
                        data.m_Enchantments = new string[0];
                        return;
                    }
                    int sparePoints = buff.Owner.Resources.GetResource(MindbladeEnhancement.BlueprintInstance).Amount;
                    if (sparePoints == 0)
                    {
                        data.m_Enhancement = 0;
                        data.m_Enchantments = new string[0];
                        return;
                    }
                    if (sparePoints > 0)
                    {
                        List<string> enchantments = new();
                        foreach (var ability in buff.Owner.ActivatableAbilities.Enumerable.Where(c => c.IsOn).Select(c => c.Blueprint).Where(c => EnhanceMindBladeAbility.enchantmentByBlueprint.ContainsKey(c)).Distinct())
                        {
                            var ench = EnhanceMindBladeAbility.enchantmentByBlueprint[ability];
                            enchantments.Add(ench.Target);
                        }
                        data.m_Enhancement = Math.Min(sparePoints, 5);
                        data.m_Enchantments = enchantments.ToArray();
                    }
                }
            }

            public void HandleBuffDidRemoved(Buff buff)
            {

            }
        }


        private static readonly string BuffGUID = "014db534-1f7a-4850-938a-98afd7d6ea2c";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "MindBladeBuff.Name".Translate("Formed Mind Blade");
        private static readonly string Description = "MindBladeBuff.Description".Translate("A Mind Blade has been formed", true);

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"MindBladeBuff", BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<AddMindBlades>()
                .Configure();
        }
    }
}
