using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Enums;
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

namespace Psionics.Buffs
{
    public class ExplodingCritical : UnitFactComponentDelegate, IInitiatorRulebookHandler<RulePrepareDamage>
    {
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if (!MindBladeItem.TypeInstances.Contains(evt.ParentRule.AttackRoll.Weapon.Blueprint.Type))
                return;
            if (evt.ParentRule.AttackRoll.IsCriticalConfirmed && evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == PsionicFocus.BlueprintInstance))
            {
                if (evt.Initiator.TryGetFeature(PsychicStrikeFeat.BlueprintInstance, out Feature feat))
                {
                    DiceFormula num = new DiceFormula()
                    {
                        m_Dice = DiceType.D8,
                        m_Rolls = feat.Rank
                    };

                    BaseDamage damage = new DamageDescription
                    {
                        TypeDescription = new DamageTypeDescription(),
                        Dice = num,
                        Bonus = 0,
                        SourceFact = base.Fact
                    }.CreateDamage();
                    evt.Add(damage);
                    PsionicFocus.Spend(evt.Initiator);
                }
            }
        }

        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
            
        }
    }

    public class ExplodingCriticalBuff
    {
        private static readonly string FeatName = "ExplodingCriticalBuff";
        private static readonly string FeatGUID = "a4405240-5cd7-47cd-8fba-52866ee478d0";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "ExplodingCriticalFeat.Name";
        private static readonly string Description = "ExplodingCriticalFeat.Description";
        private static readonly string Icon = "assets/icons/explodingcritical.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<ExplodingCritical>()
                .Configure();
        }

    }
}
