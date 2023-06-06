using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Buffs
{
    public class BladeRushBuff
    {
        private static readonly string BuffName = "BladeRushBuff";
        private static readonly string BuffGUID = "b4116e38-d793-4c8f-a16f-e80220c73de2";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "BladeRushAbility.Name";
        private static readonly string Description = "BladeRushAbility.Description";
        private static readonly string Icon = "assets/icons/bladerush.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(BuffName, BuffGUID)
                .AddCondition(Kingmaker.UnitLogic.UnitCondition.ImmuneToAttackOfOpportunity)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .Configure();
        }
    }
}
