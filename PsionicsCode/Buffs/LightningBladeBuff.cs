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
    public class LightningBladeBuff
    {
        private static readonly string FeatName = "LightningBladeBuff";
        private static readonly string FeatGUID = "ef76f4de-0d09-41c2-858b-238c05958710";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "LightningBladeFeat.Name";
        private static readonly string Description = "LightningBladeFeat.Description";
        private static readonly string Icon = "assets/icons/lightningblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<ElementalBlade>(
                    c => c.m_Energy = DamageEnergyType.Electricity
                )
                .Configure();
        }

    }
}
