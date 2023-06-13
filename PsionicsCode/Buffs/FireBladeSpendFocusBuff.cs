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
    public class FireBladeSpendFocus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RulePrepareDamage>
    {
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if (!MindBladeItem.TypeInstances.Contains(evt.ParentRule.AttackRoll.Weapon.Blueprint.Type))
                return;
            if (evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == PsionicFocus.BlueprintInstance) && evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == FireBladeBuff.BlueprintInstance))
            {
                DiceFormula num = new DiceFormula()
                {
                    m_Dice = DiceType.D10,
                    m_Rolls = 1
                };

                BaseDamage damage = new DamageDescription
                {
                    TypeDescription = new DamageTypeDescription()
                    {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Fire
                    },
                    Dice = num,
                    Bonus = 0,
                    SourceFact = base.Fact
                }.CreateDamage();
                evt.Add(damage);
                PsionicFocus.Spend(evt.Initiator);
            }
        }

        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
            
        }
    }

    public class FireBladeSpendFocusBuff
    {
        private static readonly string FeatName = "FireBladeSpendFocusBuff";
        private static readonly string FeatGUID = "5b95bf4a-283f-49e4-beec-36e891a239f0";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "FireBladeSpendFocusAbility.Name";
        private static readonly string Description = "FireBladeFeat.Description";
        private static readonly string Icon = "assets/icons/fireblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<FireBladeSpendFocus>()
                .Configure();
        }

    }
}
