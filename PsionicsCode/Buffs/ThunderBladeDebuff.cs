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
    public class ThunderBladeDebuff
    {
        private static readonly string FeatGUID = "ef53c0ff-fbde-4230-89bd-5eb89e0ce2f3";
        public static BlueprintBuff BlueprintInstance = null;
        private static readonly string Icon = "assets/icons/thunderblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"ThunderBladeDebuff", FeatGUID)
                .SetDisplayName($"ThunderBladeDebuff.Name".Translate($"Thunder Blade Debuff"))
                .SetDescription($"ThunderBladeFeat.Description")
                .SetIcon(Icon)
                .AddCondition(UnitCondition.Staggered)
                .Configure();
        }
    }
}
