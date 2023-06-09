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
    [TypeId("a4a2980b-18d2-4f04-8b54-fabdf04596b8")]
    public class DuelingBladeRiposte : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (!evt.Target.CombatState.CanAttackOfOpportunity) return;
            if (!evt.IsHit && evt.Target.Buffs.Enumerable.Any(c=>c.Blueprint == PsionicFocus.BlueprintInstance))
            {
                // Riposte~
                if ((evt.Target.TryGet(ActivatableAbilityRefs.FightDefensivelyToggleAbility.Reference.Get(), out ActivatableAbility fd) && fd.IsOn)
                 || (evt.Target.TryGet(ActivatableAbilityRefs.CombatExpertiseToggleAbility.Reference.Get(), out ActivatableAbility ce) && ce.IsOn))
                {
                    var hand = evt.Target.GetThreatHand();
                    var wep = hand.HasWeapon ? hand.Weapon : null;
                    if (wep != null)
                    {
                        PsionicFocus.Spend(evt.Target);
                        evt.Target.CombatState.AttackOfOpportunity(evt.Initiator, false, false, false);
                    }
                }
            }
        }
    }

    public class DuelingBladeBuff
    {
        private static readonly string BuffName = "DuelingBladeBuff";
        private static readonly string BuffGUID = "7dab9fbc-f02d-4713-884c-98285a64b2e4";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "DuelingBladeFeat.Name";
        private static readonly string Description = "DuelingBladeFeat.Description";
        private static readonly string Icon = "assets/icons/duelingblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(BuffName, BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<DuelingBladeRiposte>()
                .Configure();
        }
    }
}
