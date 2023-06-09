using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items.Slots;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Psionics;
using Psionics.Buffs;
using Psionics.Classes;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    [TypeId("c30c73ec-8603-47e2-bd81-a79db075fcc3")]
    public class DispellingStrikeAction : ContextAction
    {
        public override string GetCaption()
        {
            return "Dispelling Strike";
        }

        public override void RunAction()
        {
            UnitEntityData maybeCaster = Context.MaybeCaster;
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

            UnitEntityData unitEntityData = SelectTarget(Context.MaybeCaster, threatHandRanged.Weapon.AttackRange.Meters, false, Target?.Unit);
            if (!(unitEntityData != null))
                return;

            PsychicStrikeBuff.Spend(maybeCaster);
            PsionicFocus.Spend(maybeCaster);

            var attack = RunAttackRule(maybeCaster, unitEntityData, threatHandRanged);
            if (attack.AttackRoll.Result.IsHit())
            {
                DispelMagic(unitEntityData);
            }
        }

        public void DispelMagic(UnitEntityData target)
        {
            int dispellRoll = 0;
            List<Buff> list = base.Target.Unit.Buffs.Enumerable.ToList<Buff>();
            list.Sort((Buff b1, Buff b2) => -b1.Context.Params.CasterLevel.CompareTo(b2.Context.Params.CasterLevel));
            for (int i = 0; i < list.Count; i++)
            {
                if (this.TryDispelBuff(list[i], ref dispellRoll))
                {
                    break;
                }
            }
        }

        private bool TryDispelBuff(Buff buff, ref int previousRollResult)
        {
            UnitEntityData unit = base.Target.Unit;
            if (unit == null || base.Context.MaybeCaster == null || buff.IsNotDispelable || buff.HiddenInInspector || !buff.IsFromSpell)
            {
                return false;
            }
            RuleDispelMagic ruleDispelMagic = new RuleDispelMagic(base.Context.MaybeCaster, unit, buff, RuleDispelMagic.CheckType.None, Kingmaker.EntitySystem.Stats.StatType.Unknown);
            if (previousRollResult > 0)
            {
                ruleDispelMagic.SetRollOverride(previousRollResult);
            }
            ruleDispelMagic.Bonus += Context.MaybeCaster.Progression.Classes.FirstOrDefault(c=>c.CharacterClass == Classes.Soulknife.ClassBlueprint)?.Level ?? 0;
            bool success = base.Context.TriggerRule<RuleDispelMagic>(ruleDispelMagic).Success;
            previousRollResult = ruleDispelMagic.CheckRoll;
            return success;
        }

        public RuleAttackWithWeapon RunAttackRule(UnitEntityData caster, UnitEntityData target, WeaponSlot hand, int attackBonusPenalty = 0, int attackNumber = 0, int attacksCount = 1)
        {
            var tossWep = hand.Weapon;
            if (caster.Body.SecondaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(caster.Body.SecondaryHand.Weapon.Blueprint.Type))
                tossWep = caster.Body.SecondaryHand.Weapon;
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(caster, target, tossWep, attackBonusPenalty)
            {
                Reason = Context,
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
                AttackNumber = attackNumber,
                AttacksCount = attacksCount
            };

            return Context.TriggerRule(ruleAttackWithWeapon);
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

    public class DispellingStrikeAbility
    {
        private static readonly AbilityRange Range = AbilityRange.Weapon;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Physical;

        private static readonly string AbilityName = "DispellingStrikeAbility";
        private static readonly string AbilityGUID = "01ee3f80-a37f-4fb6-be1c-2e681f0a9907";

        public static BlueprintAbility BlueprintInstance = null;

        private static readonly string DisplayName = "DispellingStrikeFeat.Name";
        private static readonly string Description = "DispellingStrikeFeat.Description";
        private static readonly string Icon = "assets/icons/dispellingstrike.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<PrerequisiteHasPsychicStrike>()
                .AddComponent<PrerequisiteHasPsionicFocus>()
                .SetIcon(Icon)
                .SetRange(Range)
                .SetActionType(ActionType)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetType(TypeAbility)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .Add<DispellingStrikeAction>()
                )
                .Configure();
        }
    }
}
