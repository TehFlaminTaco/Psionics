using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
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
    [TypeId("59dfbfd0-3fa9-4bcb-8e4a-3c0d68ab990f")]
    public class ThunderBladeSpendFocus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RulePrepareDamage>
    {
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if (!MindBladeItem.TypeInstances.Contains(evt.ParentRule.AttackRoll.Weapon.Blueprint.Type))
                return;
            if (evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == PsionicFocus.BlueprintInstance) && evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == ThunderBladeBuff.BlueprintInstance))
            {
                var saveRule = new RuleSavingThrow(evt.Target, Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, 10 + evt.Initiator.Stats.BaseAttackBonus);
                saveRule = Context.TriggerRule(saveRule);
                if (!saveRule.IsPassed)
                    evt.Target.AddBuff(ThunderBladeDebuff.BlueprintInstance, this.Context, TimeSpan.FromSeconds(12f));
                PsionicFocus.Spend(evt.Initiator);
            }
        }

        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
            
        }
    }

    public class ThunderBladeSpendFocusBuff
    {
        private static readonly string FeatName = "ThunderBladeSpendFocusBuff";
        private static readonly string FeatGUID = "9c0ccbcb-9a2c-44ef-96f9-7f4e804df181";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "ThunderBladeSpendFocusAbility.Name";
        private static readonly string Description = "ThunderBladeFeat.Description";
        private static readonly string Icon = "assets/icons/thunderblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<ThunderBladeSpendFocus>()
                .Configure();
        }

    }
}
