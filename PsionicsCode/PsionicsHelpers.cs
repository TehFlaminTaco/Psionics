using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics
{
    public static class PsionicsHelpers
    {
        public static bool TryGet<A,B>(this UnitEntityData unit, A bp, out B fact) where A : BlueprintUnitFact where B : UnitFact
        {
            if (unit.HasFact(bp))
            {
                fact = unit.GetFact<B>(bp);
            }
            fact = null;
            return false;
        }
        public static bool TryGetFeature(this UnitEntityData unit, BlueprintFeature bp, out Feature fact)
        {
            fact = unit.GetFeature(bp);
            return fact != null;
        }
    }
}
