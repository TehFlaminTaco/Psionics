using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Class.Kineticist;
using Psionics.Abilities;
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
        }

        [TypeId("9f60c4c8-bdca-4e20-8cba-cb2e755257d3")]
        public class AddMindBlades : UnitBuffComponentDelegate<AddMindBladeData>, IAreaActivationHandler, IGlobalSubscriber, ISubscriber
        {
            public override void OnActivate()
            {
                Main.Logger.Info("Trying to add mind blade!");
                base.OnActivate();
                if (!base.Owner.HasMoveAction() && !base.Owner.HasFact(SoulknifeQuickDraw.BlueprintInstance))
                {
                    Main.Logger.Info("No move action :(");
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
                if (!base.Owner.Body.PrimaryHand.CanInsertItem(base.Data.Applied))
                {
                    Main.Logger.Info("Failed to add mind blade! CanInserItem returned false!");
                    base.Data.Applied = null;
                    PFLog.Default.Error("Can't insert mind blade to main hand");
                    return;
                }
                var hasAlterBlade = base.Owner.HasFact(AlterBladeFeat.BlueprintInstance);
                if(Shape == "Light" || (Shape == "Sword" && hasAlterBlade))
                {
                    var twinMindBlade = base.Owner.ActivatableAbilities.Enumerable.Where(c => c.Blueprint == TwinMindBlade.BlueprintInstance).FirstOrDefault();
                    if(twinMindBlade is not null && twinMindBlade.IsOn)
                    {
                        base.Data.Twin = MindBladeItem.BlueprintInstances[0].CreateEntity<ItemEntityWeapon>();
                        base.Data.Twin.MakeNotLootable();
                    }
                }
                if (Shape != "Heavy")
                    base.Owner.AddFact(ThrowMindBladeAbility.BlueprintInstances[Shape == "Sword" ? 1 : 0]);

                using (ContextData<ItemsCollection.SuppressEvents>.Request())
                {
                    base.Owner.Body.PrimaryHand.InsertItem(base.Data.Applied);
                    Main.Logger.Info("Finished to adding mind blade!");
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
                base.Owner.RemoveFact(ThrowMindBladeAbility.BlueprintInstances[0]);
                base.Owner.RemoveFact(ThrowMindBladeAbility.BlueprintInstances[1]);
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


        private static readonly string BuffGUID = "014db534-1f7a-4850-938a-98afd7d6ea2c";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "MindBladeBuff.Name";
        private static readonly string Description = "MindBladeBuff.Description";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"MindBladeBuff", BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent(new AddMindBlades())
                .Configure();
        }
    }
}
