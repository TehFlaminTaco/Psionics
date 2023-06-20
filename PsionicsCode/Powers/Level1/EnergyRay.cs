using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.Controllers;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Particles;
using Owlcat.QA.Validation;
using Owlcat.Runtime.Visual.RenderPipeline.RendererFeatures.FogOfWar;
using Psionics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Owlcat.Runtime.Core.Utils;
using Kingmaker.UnitLogic;
using Kingmaker.Items.Slots;
using Owlcat.Runtime.Core.Math;
using Kingmaker.EntitySystem.Stats;
using HarmonyLib;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Actions.Builder.BasicEx;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using System.Runtime.Remoting.Contexts;
using Kingmaker.PubSubSystem;
using Psionics.Buffs;
using QuickGraph;

namespace Psionics.Powers.Level1
{
    [HarmonyPatch(typeof(AbilityDeliverProjectile), nameof(AbilityDeliverProjectile.Deliver), new Type[] { typeof(AbilityExecutionContext), typeof(TargetWrapper) })]
    public class AbilityDeliverProjectile_get_Projectiles_MultiprojectilePatch1
    {
        static bool Prefix(AbilityDeliverProjectile __instance, AbilityExecutionContext context, TargetWrapper target)
        {
            if(context.AbilityBlueprint == EnergyRay.CastBlueprint) ForceUpdateProjectiles(__instance, context);
            return true;
        }

        public static void ForceUpdateProjectiles(AbilityDeliverProjectile instance, AbilityExecutionContext context)
        {
            var focus = context.MaybeCaster.GetFocusType();
            instance.m_Projectiles = new BlueprintProjectileReference[]
            {
                (focus switch
                {
                    "Fire" => ProjectileRefs.Scorching_Ray00,
                    "Cold" => ProjectileRefs.RayOfFrost00,
                    "Electricity" => ProjectileRefs.ElectroCommonProjectile00,
                    "Sonic" => ProjectileRefs.SonicCommonRay00_Projectile,
                    _ => ProjectileRefs.TricksterFishMissile_Projectile
                }).Reference.Get().ToReference<BlueprintProjectileReference>()
            };
            
        }
    }

    [TypeId("efa12e79-7490-457e-a164-1e39ef0578c3")]
    public class ConfigureDamage : ContextAction
    {
        public override string GetCaption()
        {
            return "Configure Elemental Damage";
        }

        public override void RunAction()
        {
            var damager = Context.SourceAbility.GetComponent<AbilityEffectRunAction>()?.Actions?.Actions?.OfType<ContextActionDealDamage>().FirstOrDefault();
            if(damager == null)
            {
                Main.Logger.Error("Damager not found?");
                return;
            }
            int power = 1;
            power += Augmentable.GetRank(Context.MaybeCaster, EnergyRay.PowerAugment);
            var focus = Context.MaybeCaster.GetFocusType();
            switch (focus)
            {
                case "Fire":
                    damager.DamageType.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire;
                    damager.Value = new Kingmaker.UnitLogic.Mechanics.ContextDiceValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                        DiceCountValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                        {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = power
                        },
                        BonusValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                        {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = 1
                        }
                    };
                    break;
                case "Electricity":
                    damager.DamageType.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Electricity;
                    damager.Value = new Kingmaker.UnitLogic.Mechanics.ContextDiceValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                        DiceCountValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                        {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = power
                        },
                        BonusValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                        {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = 0
                        }
                    };
                    break;
                case "Cold":
                    damager.DamageType.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Cold;
                    damager.Value = new Kingmaker.UnitLogic.Mechanics.ContextDiceValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                        DiceCountValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                        {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = power
                        },
                        BonusValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                        {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = 1
                        }
                    };
                    break;
                case "Sonic":
                    damager.DamageType.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Sonic;
                    damager.Value = new Kingmaker.UnitLogic.Mechanics.ContextDiceValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                        DiceCountValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                        {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = power
                        },
                        BonusValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                        {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = -1
                        }
                    };
                    break;
            }
        }
    }


    public class EnergyRay
    {
        private static readonly AbilityRange Range = AbilityRange.Close;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "EnergyRay";
        private static readonly string AbilityGUID = "c205cadb-20ba-463d-8573-7ab340eaf84f";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment PowerAugment = new Augment()
        {
            BaseName = "EnergyRay",
            Name = "Power",
            Description = "EnergyRay_Power.Description".Translate("You can augment this power in one of the following ways. 1. For every additional power point you spend, this power’s damage increases by one die"),
            Cost = 1,
            MaxRank = 29
        };

        [Translate("Energy Ray")]
        private static readonly string DisplayName = "EnergyRay.Name";
        [Translate("You create a ray of energy of your active energy type (cold, electricity, fire, or sonic) that shoots forth from your fingertip and strikes a target within range, dealing 1d6 points of damage, if you succeed on a ranged touch attack with the ray.\r\n\r\n{b}Cold{/b} A ray of this energy type deals +1 point of damage per die.\r\n\r\n{b}Electricity{/b} Manifesting a ray of this energy type provides a +3 bonus on your attack roll if the target is wearing metal armor and a +2 bonus on manifester level checks for the purpose of overcoming power resistance.\r\n\r\n{b}Fire{/b} A ray of this energy type deals +1 point of damage per die.\r\n\r\n{b}Sonic{/b} A ray of this energy type deals –1 point of damage per die and ignores an object’s hardness. This power’s subtype is the same as the type of energy you manifest.\r\n\r\nAugment You can augment this power in one of the following ways. 1. For every additional power point you spend, this power’s damage increases by one die (d6). 2. If you expend your psionic focus when manifesting this power, the cost of the power is reduced by 1 (to a minimum of 0), but the damage is reduced to 1d3 and it cannot be further augmented.")]
        private static readonly string Description = "EnergyRay.Description";
        private static readonly string Icon = "assets/icons/energyray.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<ConstantCountText>(cct=>cct.Text = -1)
                .Augmentable(
                    a =>
                    {
                        a.Castable = CastBlueprint = AbilityConfigurator.New($"{AbilityName}_Cast".HashGUID(), HashGUID.Last)
                            .SetDisplayName(DisplayName)
                            .SetDescription(Description)
                            .SetIcon(Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .SetType(TypeAbility)
                            .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
                            .AddComponent<ElectricalBonusAttack>()
                            .SetCanTargetFriends(false)
                            .SetCanTargetSelf(false)
                            .SetCanTargetSelf(false)
                            .SetCanTargetEnemies(true)
                            .SetSpellResistance(true)
                            .AddComponent<PrerequisiteHasElementalFocus>()
                            .AddComponent<AbilityDeliverProjectile>(adp =>
                            {
                                adp.m_LineWidth = 5.Feet();
                                adp.m_Weapon = ItemWeaponRefs.RayItem.Reference.Get().ToReference<BlueprintItemWeaponReference>();
                                adp.NeedAttackRoll = true;
                            })
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ConfigureDamage>()
                                .Add<ContextActionDealDamage>(cadd => {
                                    cadd.DamageType = new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                                    {
                                        Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire,
                                        Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy
                                    };
                                    cadd.Duration = new Kingmaker.UnitLogic.Mechanics.ContextDurationValue()
                                    {
                                        Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds,
                                        DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                                        DiceCountValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                                        {
                                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                                            Value = 0
                                        },
                                        BonusValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                                        {
                                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                                            Value = 0
                                        }
                                    };
                                    cadd.Value = new Kingmaker.UnitLogic.Mechanics.ContextDiceValue()
                                    {
                                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                        DiceCountValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                                        {
                                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                                            Value = 0
                                        },
                                        BonusValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                                        {
                                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                                            Value = 1
                                        }
                                    };
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
                        a.BaseName = "EnergyRay";
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(Icon)
                .Configure();
        }
    }
}
