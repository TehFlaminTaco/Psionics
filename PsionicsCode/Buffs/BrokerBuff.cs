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
using Kingmaker.Enums;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.EntitySystem.Stats;

namespace Psionics.Buffs
{
    [ComponentName("Scaling BuffMechanics/Bonus to Stealth skill")]
    [AllowMultipleComponents]
    [TypeId("faae4319-0723-4086-9102-1e43f89c5b80")]
    [AllowedOn(typeof(BlueprintBuff), false)]
    public class BuffSkillBonusScaling : UnitBuffComponentDelegate
    {
        public StatType Stat;

        public ModifierDescriptor Descriptor;

        public int Value;
        public int RankMultiplier = 1;

        public override void OnTurnOn()
        {
            base.OnTurnOn();
            base.Owner.Stats.GetStat(Stat).AddModifierUnique(Value + this.Buff.Rank*RankMultiplier, base.Runtime, Descriptor);
        }

        public override void OnTurnOff()
        {
            base.OnTurnOff();
            base.Owner.Stats.GetStat(Stat).RemoveModifiersFrom(base.Runtime);
        }
    }

    public class BrokerBuff
    {
        public static BlueprintBuff BlueprintInstance;
        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"BrokerBuff", "d747b0ce-8014-43cf-a9b5-42a8ed5899e8")
                .SetDisplayName("BrokerBuff.Name".Translate("Broker"))
                .SetDescription("BrokerBuff.Description".Translate("You gain temporary, intuitive insight into dealing equitably with others. You gain a +2 insight bonus to Pursuasion.", true))
                .SetIcon("assets/icons/broker.png")
                .AddComponent<BuffSkillBonusScaling>(bsbs => {
                    bsbs.Stat = StatType.SkillPersuasion;
                    bsbs.Value = 1;
                    bsbs.RankMultiplier = 1;
                    bsbs.Descriptor = ModifierDescriptor.Insight;
                })
                .Configure();
        }
    }
}
