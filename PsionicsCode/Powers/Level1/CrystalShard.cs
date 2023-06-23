using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Psionics;
using System;
using System.Linq;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using HarmonyLib;
using BlueprintCore.Blueprints.References;
using Psionics.Buffs;
using Kingmaker.RuleSystem.Rules.Damage;
using static Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription;
using Kingmaker.Enums.Damage;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace Psionics.Powers.Level1
{
    public class CrystalShard
    {
        private static readonly AbilityRange Range = AbilityRange.Close;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "CrystalShard";
        private static readonly string AbilityGUID = "bb69b500-d749-43c7-8b69-374bbbb6754f";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment PowerAugment = new Augment()
        {
            BaseName = "CrystalShard",
            Name = "Power",
            Description = "CrystalShard_Power.Description".Translate("For every additional power point you spend, this power’s damage increases by 1d6 points."),
            Cost = 1,
            MaxRank = 29
        };

        [Translate("Crystal Shard")]
        private static readonly string DisplayName = "CrystalShard.Name";
        [Translate("Upon manifesting this power, you propel a razorsharp crystal shard at your target. You must succeed on a ranged touch attack with the ray to deal damage to a target. The ray deals 1d6 points of piercing damage.\nAugment You can augment this power in one of the following ways.\n1. For every additional power point you spend, this power’s damage increases by 1d6 points.\n2. If you expend your psionic focus when manifesting this power, the cost of the power is reduced by 1 (to a minimum of 0), but the damage is reduced to 1d3 points of piercing damage and cannot be further augmented.")]
        private static readonly string Description = "CrystalShard.Description";
        private static readonly string Icon = "assets/icons/crystalshard.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Conjuration)
                .AddComponent<HideDCFromTooltip>()
                .AddComponent<ConstantCountText>(cct=>cct.Text = -1)
                .Augmentable(
                    a =>
                    {
                        a.Cantrip = AbilityConfigurator.New($"{AbilityName}_Talent".HashGUID(), HashGUID.Last)
                            .SetDisplayName($"{AbilityName}_Talent.Name".Translate("Crystal Shard (Talent)"))
                            .SetDescription(Description)
                            .SetIcon(Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .SetType(TypeAbility)
                            .AddComponent<ConstantCountText>(cct => cct.Text = -1)
                            .SetAnimation(CastAnimationStyle.Directional)
                            .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Conjuration)
                            .RequirePsionicFocus()
                            .SetCanTargetFriends(false)
                            .SetCanTargetSelf(false)
                            .SetCanTargetEnemies(true)
                            .SetSpellResistance(true)
                            .AddComponent<PrerequisiteHasElementalFocus>()
                            .AddComponent<AbilityDeliverProjectile>(adp =>
                            {
                                adp.m_LineWidth = 5.Feet();
                                adp.m_Weapon = ItemWeaponRefs.RayItem.Reference.Get().ToReference<BlueprintItemWeaponReference>();
                                adp.m_Projectiles = new[] {
                                    ProjectileRefs.Kinetic_Ice00_Projectile.Reference.Get().ToReference<BlueprintProjectileReference>()
                                };
                                adp.NeedAttackRoll = true;
                            })
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ConfigureDamage>(cd =>
                                {
                                    cd.Dice = DiceType.D3;
                                })
                                .Add<ContextActionDealDamage>(cadd => {
                                    cadd.DamageType = new DamageTypeDescription()
                                    {
                                        Physical = new PhysicalData()
                                        {
                                            Enhancement = 0,
                                            Form = PhysicalDamageForm.Piercing
                                        },
                                        Type = DamageType.Physical
                                    };
                                    cadd.Duration = ContextDuration.Fixed(0);
                                    cadd.Value = ContextDice.Value(DiceType.D6, 1, 0);
                                })
                                .Add<SpendPsionicFocus>()
                            )
                            .Configure();
                        a.Castable = CastBlueprint = AbilityConfigurator.New($"{AbilityName}_Cast".HashGUID(), HashGUID.Last)
                            .SetDisplayName(DisplayName)
                            .SetDescription(Description)
                            .SetIcon(Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Conjuration)
                            .AddComponent<HideDCFromTooltip>()
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.Directional)
                            .SetCanTargetFriends(false)
                            .SetCanTargetSelf(false)
                            .SetCanTargetEnemies(true)
                            .SetSpellResistance(false)
                            .AddComponent<AbilityDeliverProjectile>(adp =>
                            {
                                adp.m_LineWidth = 5.Feet();
                                adp.m_Weapon = ItemWeaponRefs.RayItem.Reference.Get().ToReference<BlueprintItemWeaponReference>();
                                adp.m_Projectiles = new[] {
                                    ProjectileRefs.Kinetic_Ice00_Projectile.Reference.Get().ToReference<BlueprintProjectileReference>()
                                };
                                adp.NeedAttackRoll = true;
                            })
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ConfigureDamage>()
                                .Add<ContextActionDealDamage>(cadd => {
                                    cadd.DamageType = new DamageTypeDescription()
                                    {
                                        Physical = new PhysicalData() {
                                            Enhancement = 0,
                                            Form = PhysicalDamageForm.Piercing
                                        },
                                        Type = DamageType.Physical
                                    };
                                    cadd.Duration = ContextDuration.Fixed(0);
                                    cadd.Value = ContextDice.Value(DiceType.D6, 1, 0);
                                })
                            )
                            .AddComponent<AbilityCostPowerpoints>(c => {
                                c.m_Cost = 1;
                                c.AugmentableHolder = a;
                            })
                            .Configure();
                        a.Augments = new[] {
                            PowerAugment
                        };
                        a.BaseName = "CrystalShard";
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(Icon)
                .Configure();
        }
    }
}
