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
using Kingmaker.RuleSystem;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Enums.Damage;
using Kingmaker.Blueprints.Classes.Spells;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;

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
        public bool Elemental = false;
        public Augment? ScaleDamageWith;
        public DiceType Dice = DiceType.D6;
        public int MinDice = 1;

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
            int power = MinDice;
            if(ScaleDamageWith.HasValue)
                power += Augmentable.GetRank(Context.MaybeCaster, ScaleDamageWith.Value);
            var focus = Context.MaybeCaster.GetFocusType();
            if (Elemental)
            {
                switch (focus)
                {
                    case "Fire":
                        damager.DamageType.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire;
                        damager.Value = ContextDice.Value(Dice, MinDice + power, power);
                        break;
                    case "Electricity":
                        damager.DamageType.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Electricity;
                        damager.Value = ContextDice.Value(Dice, MinDice + power, 0);
                        break;
                    case "Cold":
                        damager.DamageType.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Cold;
                        damager.Value = ContextDice.Value(Dice, MinDice + power, power);
                        break;
                    case "Sonic":
                        damager.DamageType.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Sonic;
                        damager.Value = ContextDice.Value(Dice, MinDice + power, -power);
                        break;
                }
            }
            else
            {
                damager.Value = ContextDice.Value(Dice, MinDice + power);
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
            Description = "EnergyRay_Power.Description".Translate("For every additional power point you spend, this power’s damage increases by one die"),
            Cost = 1,
            MaxRank = 29
        };

        [Translate("Energy Ray")]
        private static readonly string DisplayName = "EnergyRay.Name";
        [Translate("You create a ray of energy of your active energy type (cold, electricity, fire, or sonic) that shoots forth from your fingertip and strikes a target within range, dealing 1d6 points of damage, if you succeed on a ranged touch attack with the ray.\n<b>Cold</b> A ray of this energy type deals +1 point of damage per die.\n<b>Electricity</b> Manifesting a ray of this energy type provides a +3 bonus on your attack roll if the target is wearing metal armor and a +2 bonus on manifester level checks for the purpose of overcoming power resistance.\n<b>Fire</b> A ray of this energy type deals +1 point of damage per die.\n<b>Sonic</b> A ray of this energy type deals –1 point of damage per die and ignores an object’s hardness. This power’s subtype is the same as the type of energy you manifest.\nAugment You can augment this power in one of the following ways. 1. For every additional power point you spend, this power’s damage increases by one die (d6).\n 2. If you expend your psionic focus when manifesting this power, the cost of the power is reduced by 1 (to a minimum of 0), but the damage is reduced to 1d3 and it cannot be further augmented.", true)]
        private static readonly string Description = "EnergyRay.Description";
        private static readonly string Icon = "assets/icons/energyray.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Evocation)
                .AddComponent<ConstantCountText>(cct=>cct.Text = -1)
                .Augmentable(
                    a =>
                    {
                        a.Cantrip = AbilityConfigurator.New($"{AbilityName}_Talent".HashGUID(), HashGUID.Last)
                            .SetDisplayName($"{AbilityName}_Talent.Name".Translate("Energy Ray (Talent)"))
                            .SetDescription(Description)
                            .SetIcon(Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .SetType(TypeAbility)
                            .AddComponent<ConstantCountText>(cct => cct.Text = -1)
                            .SetAnimation(CastAnimationStyle.Directional)
                            .AddComponent<ElectricalBonusAttack>()
                            .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Evocation)
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
                                adp.NeedAttackRoll = true;
                            })
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ConfigureDamage>(cd =>
                                {
                                    cd.Elemental = true;
                                    cd.Dice = DiceType.D3;
                                })
                                .Add<ContextActionDealDamage>(cadd => {
                                    cadd.DamageType = new DamageTypeDescription()
                                    {
                                        Energy = DamageEnergyType.Fire,
                                        Type = DamageType.Energy
                                    };
                                    cadd.Duration = ContextDuration.Fixed(0);
                                    cadd.Value = ContextDice.Value(DiceType.Zero, 0, 0);
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
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.Directional)
                            .AddComponent<ElectricalBonusAttack>()
                            .AddComponent<SpellComponent>(sp =>sp.School = SpellSchool.Evocation)
                            .SetCanTargetFriends(false)
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
                                .Add<ConfigureDamage>(cd =>
                                {
                                    cd.Elemental = true;
                                    cd.ScaleDamageWith = PowerAugment;
                                })
                                .Add<ContextActionDealDamage>(cadd => {
                                    cadd.DamageType = new DamageTypeDescription()
                                    {
                                        Energy = DamageEnergyType.Fire,
                                        Type = DamageType.Energy
                                    };
                                    cadd.Duration = ContextDuration.Fixed(0);
                                    cadd.Value = ContextDice.Value(DiceType.Zero, 0, 0);
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
