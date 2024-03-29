﻿using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Psionics.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Resources
{
    public class PowerPoints
    {
        private static string ResourceName = "PowerPointResource";
        private static string ResourceGUID = "12c8a442-4018-4e94-92ff-546aeef48406";
        public static BlueprintAbilityResource BlueprintInstance;
        public static void Configure()
        {
            BlueprintInstance = AbilityResourceConfigurator.New(ResourceName, ResourceGUID)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                )
                .Configure();
        }
    }
}
