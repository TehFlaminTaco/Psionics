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
    public class ElementalBlade : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>
    {
        [FormerlySerializedAs("EnergyType")]
        public DamageEnergyType m_Energy;
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            if (!MindBladeItem.TypeInstances.Contains(evt.Weapon.Blueprint.Type))
                return;
            var dmg = evt.DamageDescription[0];
            var typ = dmg.TypeDescription.Copy();
            typ.Type = DamageType.Energy;
            typ.Energy = m_Energy;
            dmg.TypeDescription = typ;
        }
    }

    public class FireBladeBuff
    {
        private static readonly string FeatName = "FireBladeBuff";
        private static readonly string FeatGUID = "df27dfe4-8477-44ef-a9fb-de5ec85c4cab";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "FireBladeFeat.Name";
        private static readonly string Description = "FireBladeFeat.Description";
        private static readonly string Icon = "assets/icons/fireblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<ElementalBlade>(
                    c => c.m_Energy = DamageEnergyType.Fire
                )
                .Configure();
        }

    }
}
