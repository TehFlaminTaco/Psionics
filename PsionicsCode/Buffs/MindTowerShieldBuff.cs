using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Newtonsoft.Json;
using Owlcat.QA.Validation;
using Psionics.Abilities.Soulknife;
using Psionics.Equipment;
using Psionics.Feats.Soulknife.BladeSkills;
using Psionics.Feats.Soulknife;
using Psionics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Psionics.Buffs.MindBladeBuff;

namespace Psionics.Buffs
{
    public class AddMindTowerShieldData
    {
        [JsonProperty]
        public ItemEntityShield Applied;
    }

    [TypeId("9f60c4c8-bdca-4e20-8cba-cb2e755257d3")]
    public class AddMindTowerShield : UnitBuffComponentDelegate<AddMindTowerShieldData>, IAreaActivationHandler, IGlobalSubscriber, ISubscriber
    {
        public override void OnActivate()
        {
            /*Main.Logger.Info("Trying to add mind blade!");
            base.OnActivate();
            if (!base.Owner.HasMoveAction() && !base.Owner.HasFact(SoulknifeQuickDraw.BlueprintInstance))
            {
                Main.Logger.Info("No move action :(");
                var formAbility = this.Owner.ActivatableAbilities.Enumerable.Where(c => c.Blueprint == FormMindBladeAbility.BlueprintInstance).FirstOrDefault();
                if (formAbility is not null && formAbility.IsOn)
                    formAbility.IsTurnedOn = false;
                return;
            }*/
            base.Owner.MarkNotOptimizableInSave();
            base.Data.Applied = MindTowerShieldItem.BlueprintInstance.CreateEntity<ItemEntityShield>();
            base.Data.Applied.MakeNotLootable();
            if (!base.Owner.Body.SecondaryHand.CanInsertItem(base.Data.Applied))
            {
                Main.Logger.Info("Failed to add mind shield! CanInserItem returned false!");
                base.Data.Applied = null;
                PFLog.Default.Error("Can't insert shield blade to off hand");
                return;
            }
            using (ContextData<ItemsCollection.SuppressEvents>.Request())
            {
                base.Owner.Body.SecondaryHand.InsertItem(base.Data.Applied);
                Main.Logger.Info("Finished to adding mind shield!");
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

    public class MindTowerShieldBuff
    {
        private static readonly string BuffGUID = "75aa55b3-31ce-4591-80c3-c76b81e4e567";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "MindTowerShieldBuff.Name".Translate("Formed Mind Tower Shield");
        private static readonly string Description = "MindTowerShieldBuff.Description".Translate("A Mind Tower Shield has been formed", true);
        private static readonly string Icon = "assets/icons/mindtowershield.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"MindTowerShieldBuff", BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<AddMindTowerShield>()
                .SetIcon(Icon)
                .Configure();
        }
    }
}
