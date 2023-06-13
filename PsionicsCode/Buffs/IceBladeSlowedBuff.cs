using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Psionics.Abilities.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Buffs
{

    [ComponentName("Iceblade Slowdown")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("728c4151-0168-4415-b9ea-c7a2890c5ae8")]
    public class Slowdown : UnitFactComponentDelegate
    {
        public override void OnTurnOn()
        {
            int num3 = base.Owner.Stats.Speed.BaseValue / 2;
            base.Owner.Stats.Speed.AddModifierUnique(-num3, base.Runtime, ModifierDescriptor.UntypedStackable);
        }

        public override void OnTurnOff()
        {
            base.Owner.Stats.Speed.RemoveModifiersFrom(base.Runtime);
        }
    }

    public class IceBladeSlowedBuff
    {
        private static readonly string FeatGUID = "18f18371-2a13-48dc-91a9-ed759563d994";
        public static BlueprintBuff BlueprintInstance = null;
        private static readonly string Icon = "assets/icons/iceblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"IceBladeSlowedBuff", FeatGUID)
                .SetDisplayName($"IceBladeSlowedBuff.Name".Translate($"Slowed"))
                .SetDescription($"IceBladeFeat.Description")
                .SetIcon(Icon)
                .AddComponent<Slowdown>()
                .Configure();
        }
    }
}
