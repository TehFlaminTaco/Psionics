using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Psionics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;

namespace Psionics.Feats
{
    [AllowMultipleComponents]
    [TypeId("cb0f2ca0-779e-43ff-bd72-c44eb9738cb1")]
    public class IncreaseResourceAmountVariable : UnitFactComponentDelegate, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber
    {
        [SerializeField]
        [FormerlySerializedAs("Resource")]
        public BlueprintAbilityResourceReference m_Resource;

        public int[] ValueTable;

        public BlueprintAbilityResource Resource => m_Resource?.Get();

        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus)
        {
            if (base.Fact.Active && resource == Resource)
            {
                for(int i=0; i < base.Fact.GetRank(); i++)
                    bonus += ValueTable[i < ValueTable.Length ? i : ValueTable.Length-1];
            }
        }
    }
    public class WildTalentFeat
    {

        private static readonly string FeatName = "WildTalent";
        private static readonly string FeatGUID = "9d67bde5-8a37-45a7-804e-8af6e418239b";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Wild Talent")]
        private static readonly string DisplayName = "WildTalent.Name";
        [Translate("Your latent power of psionics flares to life, conferring upon you the designation of a psionic character. As a psionic character, you gain a reserve of 2 power points and can take psionic feats, metapsionic feats, and psionic item creation feats. You do not, however, gain the ability to manifest powers simply by virtue of having this feat. Additionally, you can take this feat multiple times. The first, you gain 2 additional power points, every other time, you gain only one.")]
        private static readonly string Description = "WildTalent.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID, FeatureGroup.Feat)
                .AddComponent(new IncreaseResourceAmountVariable() { m_Resource = PowerPoints.BlueprintInstance.ToReference<BlueprintAbilityResourceReference>(), ValueTable = new[] {2,2,1} })
                .AddFeatureIfHasFact(PowerPointPool.BlueprintInstance, PowerPointPool.BlueprintInstance, true)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetRanks(10)
                .Configure();
        }
    }
}
