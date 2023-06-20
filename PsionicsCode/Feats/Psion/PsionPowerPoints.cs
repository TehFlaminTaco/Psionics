using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Psion
{
    [TypeId("c6dde873-9f18-4d45-b2ae-e43be0593b1d")]
    public class PsionPowerPointScaling : UnitFactComponentDelegate, IResourceAmountBonusHandler
    {
        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus)
        {
            if (resource == PowerPoints.BlueprintInstance)
            {
                int PsionLevel = Owner?.Progression?.Classes.FirstOrDefault(c => c.CharacterClass == Classes.Psion.ClassBlueprint)?.Level ?? 0;
                int BasePoints = PsionLevel switch
                {
                    0 => 0,
                    1 => 2,
                    2 => 6,
                    3 => 11,
                    4 => 17,
                    5 => 25,
                    6 => 35,
                    7 => 46,
                    8 => 58,
                    9 => 72,
                    10 => 88,
                    11 => 106,
                    12 => 126,
                    13 => 147,
                    14 => 170,
                    15 => 195,
                    16 => 221,
                    17 => 250,
                    18 => 280,
                    19 => 311,
                    20 => 343,
                    _ => (PsionLevel - 9) * 32
                };
                BasePoints += ((Owner?.Stats?.Intelligence ?? 10) - 10) / 4 * PsionLevel;
                bonus += BasePoints;
            }
        }
    }

    public class PsionPowerPoints
    {
        private static readonly string FeatName = "PsionPowerPoints";
        private static readonly string FeatGUID = "ffee694e-3871-419a-b64a-8460ae2bcfd1";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Psion Power Points")]
        private static readonly string DisplayName = "PsionPowerPoints.Name";
        [Translate("Scaling Power Points", true)]
        private static readonly string Description = "PsionPowerPoints.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .AddFeatureIfHasFact(PowerPointPool.BlueprintInstance, PowerPointPool.BlueprintInstance, true)
                .AddComponent<PsionPowerPointScaling>()
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetHideInUI(true)
                .Configure();
        }

    }
}
