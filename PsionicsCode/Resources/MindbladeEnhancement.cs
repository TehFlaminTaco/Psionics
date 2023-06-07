using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints;
using Psionics.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Resources
{
    public class MindbladeEnhancement
    {
        private static string ResourceName = "MindbladeEnhancementResource";
        private static string ResourceGUID = "a55d8a81-645b-4847-8297-6ca18131dbf8";
        public static BlueprintAbilityResource BlueprintInstance;

        public static void Configure()
        {
            BlueprintInstance = AbilityResourceConfigurator.New(ResourceName, ResourceGUID)
                .SetMaxAmount(ResourceAmountBuilder.New(0)
                    .IncreaseByLevelStartPlusDivStep(new string[] { Soulknife.ClassGUID }, 0, 3, 1, 3, 1, 0)
                )
                .Configure(true);
        }
    }
}
