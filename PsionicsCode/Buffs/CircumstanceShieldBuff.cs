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
    public class CircumstanceShieldBuff
    {
        public static BlueprintBuff BlueprintInstance;
        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"CircumstanceShieldBuff", "3fc29c33-06d2-44fb-b1fd-41ea95f6a7eb")
                .SetDisplayName("CircumstanceShieldBuff.Name".Translate("Circumstance Shield"))
                .SetDescription("CircumstanceShieldBuff.Description".Translate("Your shield of insight alerts you to potential dangers and supercharges your reaction time. You gain a +1 insight bonus on your Initiative checks for the duration of the effect. ", true))
                .SetIcon("assets/icons/circumstanceshield.png")
                .AddComponent<BuffSkillBonusScaling>(bsbs => {
                    bsbs.Stat = StatType.Initiative;
                    bsbs.Value = 1;
                    bsbs.RankMultiplier = 1;
                    bsbs.Descriptor = ModifierDescriptor.Insight;
                })
                .Configure();
        }
    }
}
