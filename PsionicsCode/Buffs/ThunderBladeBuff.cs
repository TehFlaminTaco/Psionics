using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Equipment;
using Psionics.Feats.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;

namespace Psionics.Buffs
{
    public class ThunderBladeBuff
    {
        private static readonly string FeatName = "ThunderBladeBuff";
        private static readonly string FeatGUID = "91c662ec-1a64-43d5-abf1-c9603bedf79b";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "ThunderBladeFeat.Name";
        private static readonly string Description = "ThunderBladeFeat.Description";
        private static readonly string Icon = "assets/icons/thunderblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<ElementalBlade>(
                    c => c.m_Energy = DamageEnergyType.Sonic
                )
                .Configure();
        }

    }
}
