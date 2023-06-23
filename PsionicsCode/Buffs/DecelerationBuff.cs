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
using Kingmaker.Blueprints.Classes.Spells;

namespace Psionics.Buffs
{
    public class DecelerationBuff
    {
        public static BlueprintBuff BlueprintInstance;
        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"DecelerationBuff", "8d899a57-49a3-4860-9cac-689d3ad8d86e")
                .SetDisplayName("DecelerationBuff.Name".Translate("Deceleration"))
                .SetDescription("DecelerationBuff.Description".Translate("You warp space around an individual, hindering the subject’s ability to move. The subject’s speed (in any movement mode it possesses) is halved. A subsequent manifestation of deceleration on the subject does not further decrease its speed.", true))
                .SetIcon("assets/icons/deceleration.png")
                .AddSpellDescriptorComponent(SpellDescriptor.MovementImpairing)
                .AddComponent<Slowdown>()
                .Configure();
        }
    }
}
