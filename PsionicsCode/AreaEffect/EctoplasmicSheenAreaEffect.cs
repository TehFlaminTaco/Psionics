using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.UnitLogic;
using Kingmaker.Enums;
using Kingmaker.Utility;

namespace Psionics.AreaEffect
{
    [TypeId("ae3d9bd9-36a7-4bfa-af18-4c0e67803f19")]
    public class AbilityAreaEffectBuffRanks : AbilityAreaEffectLogic
    {
        public ConditionsChecker Condition;

        public bool CheckConditionEveryRound;

        [SerializeField]
        [FormerlySerializedAs("Buff")]
        public BlueprintBuffReference m_Buff;

        public BlueprintBuff Buff => m_Buff?.Get();

        public override void OnUnitEnter(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
        {
            TryApplyBuff(context, areaEffect, unit);
        }

        public override void OnUnitExit(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
        {
            TryRemoveBuff(context, areaEffect, unit);
        }

        public override void OnRound(MechanicsContext context, AreaEffectEntityData areaEffect)
        {
            if (!CheckConditionEveryRound)
            {
                return;
            }

            foreach (UnitEntityData item in areaEffect.InGameUnitsInside)
            {
                if (IsConditionPassed(context, item))
                {
                    TryApplyBuff(context, areaEffect, item);
                }
                else
                {
                    TryRemoveBuff(context, areaEffect, item);
                }
            }
        }

        public void TryApplyBuff(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
        {
            if (FindAppliedBuff(areaEffect, unit) == null && IsConditionPassed(context, unit))
            {
                Buff buff = unit.Descriptor.AddBuff(Buff, context);
                if (buff != null)
                {
                    buff.SourceAreaEffectId = areaEffect.UniqueId;
                }
                buff.Rank = areaEffect.Context.Ranks[(int)AbilityRankType.DamageDice];
            }
        }

        public void TryRemoveBuff(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
        {
            FindAppliedBuff(areaEffect, unit)?.Remove();
        }

        public bool IsConditionPassed(MechanicsContext context, UnitEntityData unit)
        {
            using (context.GetDataScope(unit))
            {
                return Condition.Check();
            }
        }

        public Buff FindAppliedBuff(AreaEffectEntityData areaEffect, UnitEntityData unit)
        {
            foreach (Buff rawFact in unit.Buffs.RawFacts)
            {
                if (rawFact.SourceAreaEffectId == areaEffect.UniqueId && rawFact.Blueprint == Buff)
                {
                    return rawFact;
                }
            }

            return null;
        }
    }

    public class EctoplasmicSheenAreaEffect
    {
        public static BlueprintAbilityAreaEffect BlueprintInstance = null;
        public static void Configure()
        {
            BlueprintInstance = AbilityAreaEffectConfigurator.New("EctoplasmicSheenAreaEffect".HashGUID(), HashGUID.Last)
                .AddAbilityAreaEffectBuff(BuffRefs.GreaseBuff.Reference.Get(), false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(10.Feet())
                .SetFx(AbilityAreaEffectRefs.GreaseArea.Reference.Get().Fx)
                .Configure();
        }
    }
}
