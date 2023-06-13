using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
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
    [TypeId("b0c69adf-7889-42fe-93a7-741a7c67bd0a")]
    public class LightningBladeSpendFocus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RulePrepareDamage>
    {
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if (!MindBladeItem.TypeInstances.Contains(evt.ParentRule.AttackRoll.Weapon.Blueprint.Type))
                return;
            if (evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == PsionicFocus.BlueprintInstance) && evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == LightningBladeBuff.BlueprintInstance))
            {
                evt.Target.AddBuff(LightningBladeDebuff.BlueprintInstance, this.Context, TimeSpan.FromSeconds(12f));
                PsionicFocus.Spend(evt.Initiator);
            }
        }

        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
            
        }
    }

    public class LightningBladeSpendFocusBuff
    {
        private static readonly string FeatName = "LightningBladeSpendFocusBuff";
        private static readonly string FeatGUID = "edc74965-ed82-4dc0-9296-6085018a59d2";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "LightningBladeSpendFocusAbility.Name";
        private static readonly string Description = "LightningBladeFeat.Description";
        private static readonly string Icon = "assets/icons/lightningblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<LightningBladeSpendFocus>()
                .Configure();
        }

    }
}
