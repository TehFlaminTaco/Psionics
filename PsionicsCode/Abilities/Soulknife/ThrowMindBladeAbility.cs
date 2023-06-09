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
using static Pathfinding.Util.RetainedGizmos;
using System.Windows.Markup;
using Kingmaker.Blueprints;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace Psionics.Abilities.Soulknife
{
    [TypeId("44a76118-ec2a-4725-a9fc-8fd8300c5743")]
    public class ForcedRangedAction : ContextAction
    {
        public bool SelectNewTarget;

        public bool AutoHit;

        public bool IgnoreStatBonus;

        public bool AutoCritThreat;

        public bool AutoCritConfirmation;

        public bool ExtraAttack = true;

        public bool FullAttack;

        public override string GetCaption()
        {
            return "Caster range attack " + (SelectNewTarget ? "(change target)" : "");
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
                PFLog.Default.Error("Caster can't make range attack");
                return;
            }

            UnitEntityData unitEntityData = SelectTarget(Context.MaybeCaster, new Feet(20).Meters, SelectNewTarget, Target?.Unit);
            if (!(unitEntityData != null))
            {
                return;
            }

            if (FullAttack)
            {
                RuleCalculateAttacksCount attacksCount = Rulebook.Trigger(new RuleCalculateAttacksCount(maybeCaster));
                int num = 0;
                List<UnitAttack.AttackInfo> list = UnitAttack.EnumerateAttacks(attacksCount).ToTempList();
                foreach (UnitAttack.AttackInfo item in list)
                {
                    RunAttackRule(maybeCaster, unitEntityData, item.Hand, item.AttackBonusPenalty, num, list.Count);
                    num++;
                }
            }
            else
            {
                RunAttackRule(maybeCaster, unitEntityData, threatHandRanged);
            }
        }

        public void RunAttackRule(UnitEntityData caster, UnitEntityData target, WeaponSlot hand, int attackBonusPenalty = 0, int attackNumber = 0, int attacksCount = 1)
        {
            var tossWep = hand.Weapon;
            if (caster.Body.SecondaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(caster.Body.SecondaryHand.Weapon.Blueprint.Type))
                tossWep = caster.Body.SecondaryHand.Weapon;
            var oldProjectiles = tossWep.WeaponVisualParameters.Projectiles;
            tossWep.WeaponVisualParameters.m_Projectiles = new BlueprintProjectileReference[] { BlueprintTool.GetRef<BlueprintProjectileReference>("dbcc51cfd11fc1441a495daf9df9b340") };
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(caster, target, tossWep, attackBonusPenalty)
            {
                Reason = Context,
                AutoHit = AutoHit,
                AutoCriticalThreat = AutoCritThreat,
                AutoCriticalConfirmation = AutoCritConfirmation,
                ExtraAttack = ExtraAttack,
                IsFullAttack = FullAttack,
                AttackNumber = attackNumber,
                AttacksCount = attacksCount
            };
            if (IgnoreStatBonus)
            {
                ruleAttackWithWeapon.WeaponStats.OverrideDamageBonusStatMultiplier(0f);
            }

            Context.TriggerRule(ruleAttackWithWeapon);
            tossWep.WeaponVisualParameters.m_Projectiles = oldProjectiles;
        }

        public static UnitEntityData SelectTarget(UnitEntityData caster, float range, bool selectNewTarget, UnitEntityData target)
        {
            if (!selectNewTarget)
            {
                if (target == null)
                {
                    PFLog.Default.Error("Target is invalid");
                    return null;
                }

                return target;
            }

            range += caster.View.Corpulence;
            UnitEntityData unitEntityData = null;
            foreach (UnitGroupMemory.UnitInfo enemy in caster.Memory.Enemies)
            {
                UnitEntityData unit = enemy.Unit;
                if (!(unit == null) && !(unit.View == null) && !(unit == target) && !(caster.DistanceTo(unit) > range + unit.View.Corpulence) && unit.Descriptor.State.IsConscious && (unitEntityData == null || unit.DistanceTo(target.Position) < unitEntityData.DistanceTo(target.Position)))
                {
                    unitEntityData = unit;
                }
            }

            return unitEntityData;
        }
    }

    [TypeId("be27c8ab-a2d7-4aa2-8445-1c42893e1d60")]
    public class RemoveMindBlade : ContextAction
    {
        public override string GetCaption()
        {
            return "Remove Mind Blade";
        }

        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            if (caster == null)
                return;
            var formAbility = caster.ActivatableAbilities.Enumerable.Where(c => c.Blueprint == FormMindBladeAbility.BlueprintInstance).FirstOrDefault();
            // Check if we're dual-wielding these, and only delete the secondary if we are.
            if (caster.Body.SecondaryHand.HasWeapon && MindBladeItem.TypeInstances.Contains(caster.Body.SecondaryHand.Weapon.Blueprint.Type))
            {
                var wep = caster.Body.SecondaryHand.Weapon;
                wep.HoldingSlot?.RemoveItem();
                using (ContextData<ItemsCollection.SuppressEvents>.Request())
                    wep.Collection?.Remove(wep);
            }
            else
            {
                if (formAbility is not null && formAbility.IsOn)
                    formAbility.IsTurnedOn = false;
                caster.SetBuffDuration(MindBladeBuff.BlueprintInstance, 0f);
            }
        }
    }

    public class ThrowMindBladeAbility
    {
        public static BlueprintAbility[] BlueprintInstances = new BlueprintAbility[2];
        private static readonly string[] AbilityGUIDs = new[] {
            "22540ef1-bdf7-484d-bb1e-7eb1d88e2d8b",
            "1fe5869f-5eda-4c57-b26a-3c1900c464f8"
        };


        [Translate("Throw Mind Blade")]
        private static readonly string DisplayName = "ThrowMindBladeAbility.Name";
        [Translate("All soulknives have some knowledge of how to throw their mind blades, though the range increment varies by form and the largest of blade forms cannot be thrown. Light weapon mind blades have a range increment of 20 ft. One-handed weapon mind blades have a range increment of 15 ft. Two-handed weapon mind blades cannot be thrown without the Two-Handed Throw blade skill. Whether or not the attack hits, a thrown mind blade then dissipates.")]
        private static readonly string Description = "ThrowMindBladeAbility.Description";
        private static readonly string Icon = "assets/icons/formmindblade.png";


        public static void Configure()
        {
            for (int i = 0; i < 2; i++)
            {
                BlueprintInstances[i] = AbilityConfigurator.New($"ThrowMindBlade{ShapeMindBladeAbility.ShapeNames[i]}Ability", AbilityGUIDs[i])
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetCanTargetEnemies(true)
                    .SetCanTargetFriends(false)
                    .SetCanTargetPoint(false)
                    .SetCanTargetSelf(false)
                    .AddComponent<HideDCFromTooltip>()
                    .SetIcon(Icon)
                    .SetCustomRange(20 - i * 5)
                    .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                    .AddAbilityEffectRunAction(
                        ActionsBuilder.New()
                            .Add(new ForcedRangedAction()
                            {
                                SelectNewTarget = false,
                                AutoHit = false,
                                IgnoreStatBonus = false,
                                AutoCritThreat = false,
                                AutoCritConfirmation = false,
                                ExtraAttack = true,
                                name = "$ForcedRangedAction$" + Guid.NewGuid().ToString()
                            })
                            .Add(new RemoveMindBlade()
                            {
                                name = "$RemoveMindBlade$" + Guid.NewGuid().ToString()
                            })
                    )
                    .Configure();
            }
        }

    }
}
