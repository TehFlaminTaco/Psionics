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
    public class IceBladeSpendFocus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RulePrepareDamage>
    {
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if (!MindBladeItem.TypeInstances.Contains(evt.ParentRule.AttackRoll.Weapon.Blueprint.Type))
                return;
            if (evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == PsionicFocus.BlueprintInstance) && evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == IceBladeBuff.BlueprintInstance))
            {
                evt.Target.AddBuff(IceBladeSlowedBuff.BlueprintInstance, this.Context, TimeSpan.FromSeconds(12f));
                PsionicFocus.Spend(evt.Initiator);
            }
        }

        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
            
        }
    }

    public class IceBladeSpendFocusBuff
    {
        private static readonly string FeatName = "IceBladeSpendFocusBuff";
        private static readonly string FeatGUID = "9471be40-3d3e-480a-b6c2-4f7ff10034ef";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "IceBladeSpendFocusAbility.Name";
        private static readonly string Description = "IceBladeFeat.Description";
        private static readonly string Icon = "assets/icons/iceblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<IceBladeSpendFocus>()
                .Configure();
        }

    }
}
