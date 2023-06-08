using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using Kingmaker;
using Kingmaker.Blueprints.CharGen;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Items.Slots;
using Kingmaker.Utility;
using Owlcat.Runtime.Core.Utils;
using Psionics.Equipment;
using Kingmaker.ElementsSystem;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using UnityEngine.Serialization;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Microsoft.Build.Utilities;
using System.Runtime.Remoting.Contexts;

namespace Psionics.Abilities
{
    [TypeId("9dc00f83-581d-4760-85e2-374cddb452ea")]
    public class BladewindForcedMeleeAction : ContextAction
    {
        // Ugly Hack :(
        public static bool IsBladeWinding = false;
        public override string GetCaption()
        {
            return "Caster melee attack";
        }

        public override void RunAction()
        {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing");
                return;
            }

            WeaponSlot threatHandRanged = maybeCaster.GetThreatHand();
            if (threatHandRanged == null)
            {
                PFLog.Default.Error("Caster can't make melee attack");
                return;
            }

            UnitEntityData unitEntityData = SelectTarget(base.Context.MaybeCaster, new Feet(5).Meters, false, base.Target?.Unit);
            if (!(unitEntityData != null))
            {
                return;
            }

            IsBladeWinding = true;
            try
            {
                var attack = RunAttackRule(maybeCaster, unitEntityData, threatHandRanged);
                if (!attack.AttackRoll.Result.IsHit())
                {
                    var buff = Context.MaybeCaster.Buffs.Enumerable.FirstOrDefault(c => c.Blueprint == PsychicStrikeBuff.BlueprintInstance);
                    var toggle = Context.MaybeCaster.ActivatableAbilities.Enumerable.FirstOrDefault(c => c.Blueprint == BladewindSpendPsionicStrikeAbility.BlueprintInstance);
                    if (buff != null && toggle != null && toggle.IsOn)
                    {
                        buff.Deactivate();
                        buff.Remove();
                        RunAttackRule(maybeCaster, unitEntityData, threatHandRanged);
                    }
                }
            }
            finally { 
                IsBladeWinding = false;
            }
        }

        public RuleAttackWithWeapon RunAttackRule(UnitEntityData caster, UnitEntityData target, WeaponSlot hand, int attackBonusPenalty = 0, int attackNumber = 0, int attacksCount = 1)
        {
            var tossWep = hand.Weapon;
            if (caster.Body.SecondaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(caster.Body.SecondaryHand.Weapon.Blueprint.Type))
                tossWep = caster.Body.SecondaryHand.Weapon;
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(caster, target, tossWep, attackBonusPenalty)
            {
                Reason = base.Context,
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
                AttackNumber = attackNumber,
                AttacksCount = attacksCount
            };

            return base.Context.TriggerRule(ruleAttackWithWeapon);
        }

        public static UnitEntityData SelectTarget(UnitEntityData caster, float range, bool selectNewTarget, UnitEntityData target)
        {
            if (target == null)
            {
                PFLog.Default.Error("Target is invalid");
                return null;
            }

            return target;
        }
    }

    public class BladewindAbility {
    
        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "BladewindName";
        private static readonly string AbilityGUID = "9a0fbd43-0dd0-4337-b252-a75fe22f84f2";


        [Translate("Bladewind")]
        private static readonly string DisplayName = "BladewindAbility.Name";
        [Translate("The soulknife gains the ability to momentarily fragment her mind blade into numerous identical blades, each of which strikes at a nearby opponent. As a full attack, when wielding her mind blade, a soulknife can give up her regular attacks and instead fragment her mind blade to make one melee attack at her fullbase attack bonus against each opponent within reach. Each fragment functions identically to the soulknife’s regular mind blade. When using bladewind, the soulknife can choose to expend her psychic strike if an attack misses to reroll that attack, but none of the bladewind attacks deal any extra damage from psychic strike. When using bladewind, a soulknife forfeits any bonus or extra attacks granted by other feats or abilities (such as Cleave or haste). The mind blade immediately reverts to its previous form after the bladewind attack. A soulknife must be at least 8th level to choose this blade skill.")]
        private static readonly string Description = "BladewindAbility.Description";
        private static readonly string Icon = "assets/icons/bladewind.png";


        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent(new AbilityTargetsAround()
                {
                    m_Radius = 5.Feet(),
                    m_TargetType = TargetType.Enemy,
                    m_IncludeDead = false,
                    m_SpreadSpeed = 0.Feet(),
                    m_Flags = 0,
                    m_Condition = new ConditionsChecker()
                })
                .AddComponent(new RequireMindBlade())
                .SetIcon(Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetType(AbilityType.Extraordinary)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .Add(new BladewindForcedMeleeAction()
                        {
                            name = "$BladewindForcedMeleeAction$" + Guid.NewGuid().ToString()
                        })
                )
                .Configure();
        }

    }
}
