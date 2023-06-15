using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.RuleSystem;
using UnityEngine.Serialization;
using Kingmaker.Kingdom.Rules;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Psionics.Equipment;
using Psionics.Feats.Soulknife;
using Kingmaker.UnitLogic.Buffs;
using Psionics.Abilities.Soulknife.Bladeskills;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.AI.Blueprints;

namespace Psionics.Buffs
{
    [TypeId("d23fe5f1-0fcf-4f39-a655-ae336622c3f2")]
    public class PrerequisiteHasPsychicStrike : BlueprintComponent, IAbilityCasterRestriction
    {
        public bool Not;

        public string GetAbilityCasterRestrictionUIText()
        {
            return $"{(Not ? "Has" : "No")} Psionic Strike Charge!";
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return caster.Buffs.Enumerable.Any(c => c.Blueprint == PsychicStrikeBuff.BlueprintInstance) ^ Not;
        }
    }

    [ComponentName("Psychic strike damage bonus")]
    [TypeId("c307f9e9-8590-4e4a-b486-0fe384d225da")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    public class PsychicStrikeDamageBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, IAttackHandler
    {
        [FormerlySerializedAs("ScaleWith")]
        public BlueprintFeature m_ScaleWith;
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (!MindBladeItem.TypeInstances.Contains(evt.Weapon.Blueprint.Type))
                return;
            var alternativeSpend = evt.Initiator.ActivatableAbilities.Enumerable.FirstOrDefault(b => b.Blueprint == BladewindSpendPsionicStrikeAbility.BlueprintInstance);
            if (alternativeSpend != null && alternativeSpend.IsOn)
                return;
            int scale = 1;
            if (m_ScaleWith is not null && evt.Initiator.HasFact(m_ScaleWith))
                scale = evt.Initiator.GetFeature(m_ScaleWith).GetRank();
            else
                Main.Logger.Info("Couldn't find Psychic Strike Scale!");
            DiceFormula num = new DiceFormula()
            {
                m_Dice = DiceType.D8,
                m_Rolls = scale
            };

            int res = new DiceFormulaEvaluator() { DiceFormula = num }.GetValue();
            evt.DamageModifiers.Add(new Modifier(res, $"{scale}d8", base.Fact, ModifierDescriptor.UntypedStackable));
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            
        }

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }

        public void HandleAttackHitRoll(RuleAttackRoll rollAttackHit)
        {
            if (!MindBladeItem.TypeInstances.Contains(rollAttackHit.Weapon.Blueprint.Type))
                return;
            var alternativeSpend = rollAttackHit.Initiator.ActivatableAbilities.Enumerable.FirstOrDefault(b => b.Blueprint == BladewindSpendPsionicStrikeAbility.BlueprintInstance);
            if (alternativeSpend is not null && alternativeSpend.IsOn && BladewindForcedMeleeAction.IsBladeWinding)
                return;
            if (rollAttackHit.IsHit)
            {
                ((Buff)base.Fact).Deactivate();
                ((Buff)base.Fact).Remove();
            }
        }
    }

    public class PsychicStrikeBuff
    {
        public static readonly string BuffName = "PsychicStrikeBuff";
        public static readonly string BuffGUID = "477343d4-ec85-4852-84d5-b95b7e704072";

        public static BlueprintBuff BlueprintInstance = null;
        public static readonly string DisplayName = "PsychicStrikeAbility.Name";
        public static readonly string Description = "PsychicStrikeAbility.Description";
        public static readonly string Icon = "assets/icons/psychicstrike.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(BuffName, BuffGUID)
                .OnConfigure(bp=>bp.ComponentsArray = bp.ComponentsArray.Append(new PsychicStrikeDamageBonus() { m_ScaleWith = PsychicStrikeFeat.BlueprintInstance }).ToArray())
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .Configure(true);
        }

        public static void Spend(UnitEntityData unit)
        {
            var buff = unit.Buffs.Enumerable.FirstOrDefault(c => c.Blueprint == PsychicStrikeBuff.BlueprintInstance);
            if (buff.GetRank() <= 1)
            {
                buff.Deactivate();
                buff.Remove();
            }
            if (buff is not null)
                buff.RemoveRank();
        }

    }
}
