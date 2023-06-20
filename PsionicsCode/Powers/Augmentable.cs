using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ActionBar;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Pathfinding.Voxels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace Psionics.Powers
{
    public struct Augment
    {
        public string BaseName;
        public string Name;
        public int MaxRank;
        public int Cost;
        public string Description;
    }

    [TypeId("9a1eb6db-6476-450d-a03d-77cf100f0af4")]
    public class SetAugment : ContextAction
    {
        public BlueprintBuff buff;
        public Augment augment;
        public int AugmentIndex;
        public int Value;
        public override string GetCaption()
        {
            return "Set Augment";
        }

        public override void RunAction()
        {
            foreach (var augHolder in Context.MaybeCaster.Buffs.GetFactsContainingComponent<AugmentHolder>()
                        .Select(c => (c, c.GetComponent<AugmentHolder>()))
                        .Where(c => c.Item2.augment.BaseName == augment.BaseName && c.Item2.augment.Name == augment.Name).ToList())
            {
                augHolder.Item1.Deactivate();
                augHolder.Item1.Remove();
            }
            if (buff != null) Context.MaybeCaster.AddBuff(buff, Context.MaybeCaster);
        }
    }

    [TypeId("ac5a14f6-8c72-4105-9d71-5ecb2063c178")]
    public class HideIfNotManifestable : UnitFactComponentDelegate, IAbilityVisibilityProvider
    {
        public int SimpleCost;
        public bool IsAbilityVisible(AbilityData ability)
        {
            int casterLevel = ability.Spellbook?.CasterLevel ?? 1;
            return SimpleCost <= casterLevel;
        }
    }

    [TypeId("161d1223-a6d6-4d83-a727-b146f9f9cd23")]
    public class DisableIfNotManifestable : BlueprintComponent, IAbilityRestriction
    {
        public Augmentable Source;
        public int AugmentIndex;
        public int Value;
        bool simpleReason = false;
        public string GetAbilityRestrictionUIText()
        {
            if (simpleReason) return "Augment level already selected";
            return "Too high augmentation for Manifester Level";
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            int cost = Source.GetCostWith(ability.Caster, AugmentIndex, Value);
            int casterLevel = ability.m_ConvertedFrom?.Spellbook?.CasterLevel ?? 1;
            if (simpleReason = (Augmentable.GetRank(ability.Caster, Source.Augments[AugmentIndex]) == Value))
                return false;
            return cost <= casterLevel;
        }
    }

    [TypeId("35a3fb9e-c5dd-4177-9667-7c17a7d5fdb6")]
    public class AugmentHolder : BlueprintComponent
    {
        public Augment augment;
        public int AugmentIndex;
        public int Value;
    }

    [TypeId("729666ff-4aed-44ef-b401-35ae980b92cb")]
    public class Augmentable : UnitFactComponentDelegate
    {
        public BlueprintAbility Castable;
        public string BaseName;
        public BlueprintAbility[] Abilities;
        public Augment[] Augments;
        public BlueprintBuff[] Buffs;
        public int BaseCost;

        public static int GetRank(UnitEntityData caster, Augment augment)
        {
            return caster.Buffs.GetFactsContainingComponent<AugmentHolder>()
                        .Select(c => c.GetComponent<AugmentHolder>())
                        .Where(c => c.augment.BaseName == augment.BaseName && c.augment.Name == augment.Name)
                        .FirstOrDefault()?.Value ?? 0;
        }

        public int[] GetAugmentData(UnitEntityData caster)
        {
            int[] augmentData = new int[Augments.Length];
            foreach (var augHolder in caster.Buffs.GetFactsContainingComponent<AugmentHolder>()
                        .Select(c => c.GetComponent<AugmentHolder>())
                        .Where(c => c.augment.BaseName == BaseName)){
                augmentData[augHolder.AugmentIndex] = augHolder.Value;
            }
            return augmentData;
        }

        public int GetCost(UnitEntityData caster)
        {
            int total = this.BaseCost;
            int[] augmentData = GetAugmentData(caster);
            for (int i = 0; i < Augments.Length; i++)
            {
                total += augmentData[i] * Augments[i].Cost;
            }
            return total;
        }

        public int GetCostWith(UnitEntityData caster, int index, int value)
        {
            int total = this.BaseCost;
            int[] augmentData = GetAugmentData(caster);
            for (int i = 0; i < Augments.Length; i++)
            {
                total += i == index ? value : augmentData[i] * Augments[i].Cost;
            }
            return total;
        }

        public BlueprintAbility[] Configure()
        {
            Abilities = new BlueprintAbility[Augments.Select(c=>c.MaxRank+1).Sum()];
            Buffs = new BlueprintBuff[Augments.Select(c => c.MaxRank).Sum()];
            int buffIndex = 0;
            int index = 0;
            int augmentIndex = 0;
            for(int i=0; i < Augments.Length; i++)
            {
                this.Augments[i].BaseName = this.BaseName;
            }
            foreach(var aug in Augments)
            {
                for(int i=0; i <= aug.MaxRank; i++)
                {
                    BlueprintBuff buff = null;
                    if(i > 0)
                    {
                        buff = Buffs[buffIndex++] = BuffConfigurator.New($"{BaseName}_{aug.Name}{i}_Buff".HashGUID(), HashGUID.Last)
                            .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                            .AddComponent<AugmentHolder>(ah =>
                            {
                                ah.augment = aug;
                                ah.Value = i;
                                ah.AugmentIndex = augmentIndex;
                            })
                            .Configure();
                    }
                    Abilities[index] = AbilityConfigurator.New($"{BaseName}_{aug.Name}{i}".HashGUID(), HashGUID.Last)
                        .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                        .SetDisplayName($"{BaseName}_{aug.Name}{i}.Name".Translate($"{aug.Name} {(i == 0 ? "Off" : i)}"))
                        .AddComponent<ConstantCountText>(cct => cct.Text = i == 0 ? -1 : i * aug.Cost)
                        .AddComponent<HideIfNotManifestable>(hinm =>
                        {
                            hinm.SimpleCost = i == 0 ? BaseCost + aug.Cost : BaseCost + (aug.Cost * i);
                        })
                        .AddComponent<DisableIfNotManifestable>(dinm =>
                        {
                            dinm.Source = this;
                            dinm.AugmentIndex = augmentIndex;
                            dinm.Value = aug.Cost * i;
                        })
                        .SetDescription(aug.Description)
                        .SetType(AbilityType.Special)
                        .SetIcon("assets/icons/augment.png")
                        .SetDisableLog(true)
                        .AddAbilityEffectRunAction(ActionsBuilder.New()
                            .Add<SetAugment>(sa =>
                            {
                                sa.buff = buff;
                                sa.augment = aug;
                                sa.AugmentIndex = augmentIndex;
                                sa.Value = i;
                            })
                        )
                        .Configure();
                    index++;
                }
                augmentIndex++;
            }
            return Abilities;
        }
    }
}
