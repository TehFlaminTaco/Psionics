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
    public class IceBladeBuff
    {
        private static readonly string FeatName = "IceBladeBuff";
        private static readonly string FeatGUID = "935490d5-924c-4c7d-9279-e37cf119a4fa";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "IceBladeFeat.Name";
        private static readonly string Description = "IceBladeFeat.Description";
        private static readonly string Icon = "assets/icons/iceblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<ElementalBlade>(
                    c => c.m_Energy = DamageEnergyType.Cold
                )
                .Configure();
        }

    }
}
