using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Psionics;
using Psionics.Buffs;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.Items.Slots;
using BlueprintCore.Actions.Builder;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using BlueprintCore.Blueprints.References;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    [TypeId("2bd6207d-4aee-4436-957a-3433bfbe9f4a")]
    public class DisruptingStrikeMeleeAction : ContextAction
    {
        public override string GetCaption()
        {
            return "Caster melee attack";
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

            UnitEntityData unitEntityData = SelectTarget(Context.MaybeCaster, 5.Feet().Meters, false, Target?.Unit);
            if (!(unitEntityData != null))
                return;

            var attack = RunAttackRule(maybeCaster, unitEntityData, threatHandRanged);
            if (attack.IsHit)
                unitEntityData.AddBuff(DisruptedBuff.BlueprintInstance, Context, TimeSpan.FromSeconds(12));
        }

        public RuleAttackRoll RunAttackRule(UnitEntityData caster, UnitEntityData target, WeaponSlot hand, int attackBonusPenalty = 0, int attackNumber = 0, int attacksCount = 1)
        {
            var tossWep = hand.Weapon;
            if (caster.Body.SecondaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(caster.Body.SecondaryHand.Weapon.Blueprint.Type))
                tossWep = caster.Body.SecondaryHand.Weapon;
            RuleAttackRoll ruleAttackWithWeapon = new RuleAttackRoll(caster, target, tossWep, attackBonusPenalty)
            {
                Reason = Context,
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
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


    public class DisruptingStrikeAbility {
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Physical;

        private static readonly string AbilityName = "DisruptingStrikeAbility";
        private static readonly string AbilityGUID = "c3e698af-bd0d-494a-a2b3-b15cbef090be";

        public static BlueprintAbility BlueprintInstance = null;

        private static readonly string DisplayName = "DisruptingStrikeFeat.Name";
        private static readonly string Description = "DisruptingStrikeFeat.Description";
        private static readonly string Icon = "assets/icons/disruptingstrike.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<AbilityTargetsAround>(bp => {
                    bp.m_Radius = 5.Feet();
                    bp.m_TargetType = TargetType.Enemy;
                    bp.m_IncludeDead = false;
                    bp.m_SpreadSpeed = 0.Feet();
                    bp.m_Flags = 0;
                    bp.m_Condition = new ConditionsChecker();
                })
                .AddComponent<HideDCFromTooltip>()
                .AddComponent<RequireMindBlade>()
                .SetIcon(Icon)
                .SetActionType(ActionType)
                .SetIsFullRoundAction(true)
                .SetType(TypeAbility)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<DisruptingStrikeMeleeAction>()
                )
                .Configure();
        }
    }
}
