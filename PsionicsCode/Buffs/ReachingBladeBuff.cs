using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
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
    public class ReachingBladeBuff
    {
        private static readonly string FeatGUID = "0b7a2642-f559-4118-bf40-e8ea14159640";
        public static BlueprintBuff BlueprintInstance = null;
        private static readonly string Icon = "assets/icons/reachingblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"ReachingBladeBuff", FeatGUID)
                .SetDisplayName($"ReachingBladeFeat.Name")
                .SetDescription($"ReachingBladeFeat.Description")
                .SetIcon(Icon)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, null, Kingmaker.EntitySystem.Stats.StatType.Reach, 5)
                .Configure();
        }
    }
}
