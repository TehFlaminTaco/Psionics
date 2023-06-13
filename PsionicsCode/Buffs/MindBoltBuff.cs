using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
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

namespace Psionics.Buffs
{
    public class MindBoltBuff
    {
        public class AddMindBoltData
        {
            [JsonProperty]
            public ItemEntityWeapon Applied;
        }

        [TypeId("df168dc8-bcd4-4cb0-b5dd-20d1c523a854")]
        public class AddMindBolts : UnitBuffComponentDelegate<AddMindBoltData>, IAreaActivationHandler, IGlobalSubscriber, ISubscriber
        {
            public override void OnActivate()
            {
                if (base.Owner.TryGet(FormMindBladeAbility.BlueprintInstance, out ActivatableAbility ab) && ab.IsTurnedOn)
                    ab.TurnOffImmediately();
                Main.Logger.Info("Trying to add mind blade!");
                base.OnActivate();
                if (!base.Owner.HasMoveAction() && !base.Owner.HasFact(SoulknifeQuickDraw.BlueprintInstance))
                {
                    Main.Logger.Info("No move action :(");
                    var formAbility = this.Owner.ActivatableAbilities.Enumerable.Where(c => c.Blueprint == FormMindBoltAbility.BlueprintInstance).FirstOrDefault();
                    if (formAbility is not null && formAbility.IsOn)
                        formAbility.IsTurnedOn = false;
                    return;
                }
                base.Owner.MarkNotOptimizableInSave();
                BlueprintItemWeapon Bolt = MindBoltItem.BlueprintInstances[1];
                int shapeIndex = 1;
                for(int fsIndex = 0; fsIndex < 3; fsIndex++)
                {
                    if (base.Owner.HasFact(MindBoltShapeFeat.BlueprintInstances[fsIndex]))
                    {
                        shapeIndex = fsIndex;
                        Bolt = MindBoltItem.BlueprintInstances[fsIndex];
                        break;
                    }
                }

                base.Data.Applied = Bolt.CreateEntity<ItemEntityWeapon>();
                base.Data.Applied.MakeNotLootable();
                Enchant(base.Data.Applied);
                if (!base.Owner.Body.PrimaryHand.CanInsertItem(base.Data.Applied))
                {
                    Main.Logger.Info("Failed to add mind bolt! CanInsertItem returned false!");
                    base.Data.Applied = null;
                    PFLog.Default.Error("Can't insert mind bolt to main hand");
                    return;
                }
                using (ContextData<ItemsCollection.SuppressEvents>.Request())
                {
                    base.Owner.Body.PrimaryHand.InsertItem(base.Data.Applied);
                    Main.Logger.Info("Finished to adding mind blade!");
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
            }

            public void Enchant(ItemEntityWeapon wep)
            {
                if (base.Owner.HasFact(EnhancedMindBladeFeat.BlueprintInstance))
                {
                    int sparePoints = base.Owner.Resources.GetResource(MindbladeEnhancement.BlueprintInstance).Amount;
                    if (sparePoints == 0)
                        return;
                    if(sparePoints > 0) {
                        foreach(var ability in base.Owner.ActivatableAbilities.Enumerable.Where(c=>c.IsOn).Select(c=>c.Blueprint).Where(c => EnhanceMindBladeAbility.enchantmentByBlueprint.ContainsKey(c)).Distinct())
                        {
                            var ench = EnhanceMindBladeAbility.enchantmentByBlueprint[ability];
                            wep.AddEnchantment(BlueprintTool.GetRef<BlueprintItemEnchantmentReference>(ench.Target).Get(), null, null);
                        }
                        wep.AddEnchantment(BlueprintTool.GetRef<BlueprintItemEnchantmentReference>(EnhanceMindBladeAbility.Enhancement[Math.Min(sparePoints - 1, 4)]).Get(), null, null);
                    }
                }
            }

            public override void ApplyValidation(ValidationContext context, int parentIndex)
            {
                base.ApplyValidation(context, parentIndex);
            }

            public override void OnTurnOn()
            {
                base.Data.Applied?.HoldingSlot.Lock.Retain();
            }

            public override void OnTurnOff()
            {
                base.Data.Applied?.HoldingSlot.Lock.Release();
            }

            public void OnAreaActivated()
            {
                if (base.Data.Applied == null)
                {
                    OnActivate();
                    OnTurnOn();
                }
            }
        }


        private static readonly string BuffGUID = "75d3d757-1000-4ade-beb1-0df47bcc787c";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "MindBoltBuff.Name".Translate("Formed Mind Bolt");
        private static readonly string Description = "MindBoltBuff.Description".Translate("A Mind Bolt has been formed", true);

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"MindBoltBuff", BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<AddMindBolts>()
                .Configure();
        }
    }
}
