using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
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
            fact = unit.Facts.GetAll<B>().FirstOrDefault(c=>c.Blueprint == bp);
            return fact != null;
        }
    }
}
