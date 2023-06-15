using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using Kingmaker.EntitySystem.Entities;

namespace Psionics.Buffs
{
    [AllowedOn(typeof(BlueprintBuff), false)]
    [TypeId("e27e29b6-4186-4633-8736-e68f93fcc240")]
    public class GruesomeRiposte : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.IsHit && evt.Target.Buffs.Enumerable.Any(c=>c.Blueprint == PsionicFocus.BlueprintInstance))
            {
                var hand = evt.Target.GetThreatHand();
                var wep = hand.HasWeapon ? hand.Weapon : null;
                if (wep != null)
                {
                    PsionicFocus.Spend(evt.Target);
                    evt.Target.CombatState.AttackOfOpportunityCount++;
                    if(!evt.Target.CombatState.AttackOfOpportunity(evt.Initiator, false, false, false))
                        evt.Target.CombatState.AttackOfOpportunityCount--;
                }
            }
        }
    }

    public class GruesomeRiposteBuff
    {
        private static readonly string BuffName = "GruesomeRiposteBuff";
        private static readonly string BuffGUID = "0401ece6-4c1a-405f-8cf8-37217fa5620c";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "GruesomeRiposteFeat.Name";
        private static readonly string Description = "GruesomeRiposteFeat.Description";
        private static readonly string Icon = "assets/icons/gruesomeriposte.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(BuffName, BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<GruesomeRiposte>()
                .Configure();
        }
    }
}
