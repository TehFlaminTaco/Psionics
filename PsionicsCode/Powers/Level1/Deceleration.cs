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
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.EntitySystem.Stats;
using JetBrains.Annotations;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker;
using UnityEngine;
using Kingmaker.UnitLogic.Buffs;

namespace Psionics.Powers.Level1
{
    [TypeId("c63e52dd-f7ae-413a-9ac7-fd9c6ae30572")]
    public class ContextActionSavingThrowBonus : ContextAction
    {
        public ContextValue Bonus;
        [Serializable]
        public struct ConditionalDCIncrease
        {
            public ConditionsChecker Condition;

            public ContextValue Value;
        }

        public SavingThrowType Type;

        [InfoBox("Search through Actions, find ContextActionApplyBuff and use it in saving throw rule (used in BuffDescriptorImmunity for now)")]
        public bool FromBuff;

        [SerializeField]
        public ConditionalDCIncrease[] m_ConditionalDCIncrease = new ConditionalDCIncrease[0];

        [InfoBox("If context already had saving throw use DC from it (for example if this action is in trigger on fireball cast)\nIf not check this fact DC or CustomDC")]
        public bool UseDCFromContextSavingThrow;

        public bool HasCustomDC;

        [ShowIf("HasCustomDC")]
        public ContextValue CustomDC;

        public ActionList Actions;

        public override string GetCaption()
        {
            return "Saving throw " + Type;
        }

        public override void RunAction()
        {
            if (base.Target.Unit == null)
            {
                PFLog.Default.Error(this, "Can't use ContextActionSavingThrow because target is not an unit");
            }
            else
            {
                if (base.Context == null)
                {
                    return;
                }

                int num = 0;
                if (m_ConditionalDCIncrease != null)
                {
                    ConditionalDCIncrease[] conditionalDCIncrease = m_ConditionalDCIncrease;
                    for (int i = 0; i < conditionalDCIncrease.Length; i++)
                    {
                        ConditionalDCIncrease conditionalDCIncrease2 = conditionalDCIncrease[i];
                        if (conditionalDCIncrease2.Condition.Check())
                        {
                            num += conditionalDCIncrease2.Value.Calculate(base.Context);
                        }
                    }
                }

                int dc = ((!UseDCFromContextSavingThrow || base.Context.SavingThrow == null) ? ((HasCustomDC ? CustomDC.Calculate(base.Context) : base.Context.Params.DC) + num) : base.Context.SavingThrow.DifficultyClass);
                if (Bonus != null)
                {
                    dc += Bonus.Calculate(Context);
                }
                RuleSavingThrow ruleSavingThrow = base.Context.TriggerRule(CreateSavingThrow(base.Target.Unit, dc, persistentSpell: false));
                if (ruleSavingThrow.IsPassed && base.Context.HasMetamagic(Metamagic.Persistent))
                {
                    ruleSavingThrow = base.Context.TriggerRule(CreateSavingThrow(base.Target.Unit, dc, persistentSpell: true));
                }

                using (ContextData<SavingThrowData>.Request().Setup(ruleSavingThrow))
                {
                    Actions.Run();
                }
            }
        }

        [CanBeNull]
        public static ContextActionApplyBuff FindApplyBuffAction(ActionList actions)
        {
            GameAction[] actions2 = actions.Actions;
            foreach (GameAction gameAction in actions2)
            {
                ContextActionConditionalSaved contextActionConditionalSaved = gameAction as ContextActionConditionalSaved;
                ContextActionApplyBuff contextActionApplyBuff = ((contextActionConditionalSaved != null) ? FindApplyBuffAction(contextActionConditionalSaved.Succeed) : (gameAction as ContextActionApplyBuff));
                if (contextActionApplyBuff != null)
                {
                    return contextActionApplyBuff;
                }
            }

            return null;
        }

        public RuleSavingThrow CreateSavingThrow(UnitEntityData unit, int dc, bool persistentSpell)
        {
            return new RuleSavingThrow(unit, Type, dc)
            {
                Buff = ((!FromBuff) ? null : FindApplyBuffAction(Actions)?.Buff),
                PersistentSpell = persistentSpell
            };
        }
    }

    [TypeId("13b23c3f-a88e-4082-a537-bc0ab0ada1f3")]
    public class ContextActionSavingThrowAugmentDC : ContextAction
    {
        public Augment ScaleWith;
        public int ScaleStep = 1;
        public int ScaleAmount = 1;

        [Serializable]
        public struct ConditionalDCIncrease
        {
            public ConditionsChecker Condition;

            public ContextValue Value;
        }

        public SavingThrowType Type;

        [InfoBox("Search through Actions, find ContextActionApplyBuff and use it in saving throw rule (used in BuffDescriptorImmunity for now)")]
        public bool FromBuff;

        [SerializeField]
        public ConditionalDCIncrease[] m_ConditionalDCIncrease = new ConditionalDCIncrease[0];

        [InfoBox("If context already had saving throw use DC from it (for example if this action is in trigger on fireball cast)\nIf not check this fact DC or CustomDC")]
        public bool UseDCFromContextSavingThrow;

        public bool HasCustomDC;

        [ShowIf("HasCustomDC")]
        public ContextValue CustomDC;

        public ActionList Actions;

        public override string GetCaption()
        {
            return "Saving throw " + Type;
        }

        public override void RunAction()
        {
            if (base.Target.Unit == null)
            {
                PFLog.Default.Error(this, "Can't use ContextActionSavingThrow because target is not an unit");
            }
            else
            {
                if (base.Context == null)
                {
                    return;
                }

                int num = 0;
                if (m_ConditionalDCIncrease != null)
                {
                    ConditionalDCIncrease[] conditionalDCIncrease = m_ConditionalDCIncrease;
                    for (int i = 0; i < conditionalDCIncrease.Length; i++)
                    {
                        ConditionalDCIncrease conditionalDCIncrease2 = conditionalDCIncrease[i];
                        if (conditionalDCIncrease2.Condition.Check())
                        {
                            num += conditionalDCIncrease2.Value.Calculate(base.Context);
                        }
                    }
                }

                int dc = ((!UseDCFromContextSavingThrow || base.Context.SavingThrow == null) ? ((HasCustomDC ? CustomDC.Calculate(base.Context) : base.Context.Params.DC) + num) : base.Context.SavingThrow.DifficultyClass);
                if (ScaleWith.BaseName != null)
                {
                    int ranks = Augmentable.GetRank(Context.MaybeCaster, ScaleWith);
                    dc += (ranks / ScaleStep) * ScaleAmount;
                }
                RuleSavingThrow ruleSavingThrow = base.Context.TriggerRule(CreateSavingThrow(base.Target.Unit, dc, persistentSpell: false));
                if (ruleSavingThrow.IsPassed && base.Context.HasMetamagic(Metamagic.Persistent))
                {
                    ruleSavingThrow = base.Context.TriggerRule(CreateSavingThrow(base.Target.Unit, dc, persistentSpell: true));
                }

                using (ContextData<SavingThrowData>.Request().Setup(ruleSavingThrow))
                {
                    Actions.Run();
                }
            }
        }

        [CanBeNull]
        public static ContextActionApplyBuff FindApplyBuffAction(ActionList actions)
        {
            GameAction[] actions2 = actions.Actions;
            foreach (GameAction gameAction in actions2)
            {
                ContextActionConditionalSaved contextActionConditionalSaved = gameAction as ContextActionConditionalSaved;
                ContextActionApplyBuff contextActionApplyBuff = ((contextActionConditionalSaved != null) ? FindApplyBuffAction(contextActionConditionalSaved.Succeed) : (gameAction as ContextActionApplyBuff));
                if (contextActionApplyBuff != null)
                {
                    return contextActionApplyBuff;
                }
            }

            return null;
        }

        public RuleSavingThrow CreateSavingThrow(UnitEntityData unit, int dc, bool persistentSpell)
        {
            return new RuleSavingThrow(unit, Type, dc)
            {
                Buff = ((!FromBuff) ? null : FindApplyBuffAction(Actions)?.Buff),
                PersistentSpell = persistentSpell
            };
        }
    }

    public class Deceleration
    {
        private static readonly AbilityRange Range = AbilityRange.Close;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "DecelerationPower";
        private static readonly string AbilityGUID = "0f89e945-1d01-4d06-bc33-386454f50c16";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment EnhancementAugment = new Augment()
        {
            BaseName = "DecelerationPower",
            Name = "DC",
            Description = "DecelerationPower_DC.Description".Translate("For every 2 additional power points you spend, this power’s save DC increases by 1"),
            Cost = 2,
            MaxRank = 15
        };

        [Translate("Deceleration")]
        private static readonly string DisplayName = "DecelerationPower.Name";
        [Translate("You warp space around an individual, hindering the subject’s ability to move. The subject’s speed (in any movement mode it possesses) is halved. A subsequent manifestation of deceleration on the subject does not further decrease its speed.\nAugment For every 2 additional power points you spend, this power’s save DC increases by 1")]
        private static readonly string Description = "DecelerationPower.Description";
        private static readonly string Icon = "assets/icons/deceleration.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Conjuration)
                .AddComponent<ConstantCountText>(cct=>cct.Text = -1)
                .AddSpellDescriptorComponent(SpellDescriptor.MovementImpairing)
                .Augmentable(
                    a =>
                    {
                        a.Castable = CastBlueprint = AbilityConfigurator.New($"{AbilityName}_Cast".HashGUID(), HashGUID.Last)
                            .SetDisplayName(DisplayName)
                            .SetDescription(Description)
                            .SetIcon(Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .AddComponent<SpellComponent>(sp => sp.School = SpellSchool.Conjuration)
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.Touch)
                            .AddSpellDescriptorComponent(SpellDescriptor.MovementImpairing)
                            .SetCanTargetSelf(false)
                            .SetCanTargetEnemies(true)
                            .SetSpellResistance(true)
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ContextActionSavingThrowAugmentDC>(castad=> {
                                    castad.Type = SavingThrowType.Reflex;
                                    castad.FromBuff = false;
                                    castad.CustomDC = null;
                                    castad.HasCustomDC = false;
                                    castad.m_ConditionalDCIncrease = new ContextActionSavingThrowAugmentDC.ConditionalDCIncrease[0];
                                    castad.UseDCFromContextSavingThrow = false;
                                    castad.ScaleWith = EnhancementAugment;
                                    castad.Actions = ActionsBuilder.New()
                                                        .Add<ApplyBuffAugmentRanks>(abar =>
                                                        {
                                                            abar.buff = DecelerationBuff.BlueprintInstance;
                                                            abar.augment = EnhancementAugment;
                                                            abar.toCaster = false;
                                                            abar.duration = ContextDuration.Variable(ContextValues.Rank(AbilityRankType.Default), DurationRate.Minutes);
                                                        })
                                                        .Build();
                                })
                            )
                            .AddComponent<AbilityCostPowerpoints>(c => {
                                c.m_Cost = 1;
                                c.AugmentableHolder = a;
                            })
                            .AddContextRankConfig(ContextRankConfigs.CasterLevel(false, AbilityRankType.Default))
                            .Configure();
                        a.Augments = new[] {
                            EnhancementAugment
                        };
                        a.BaseName = "DecelerationPower";
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(Icon)
                .Configure();
        }
    }
}
