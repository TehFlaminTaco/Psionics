using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class AlterBladeFeat
    {
        private static readonly string FeatName = "AlterBladeFeat";
        private static readonly string FeatGUID = "9d0bf308-54a8-45c6-81d3-c2dcf308b9aa";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Alter Blade")]
        private static readonly string DisplayName = "AlterBladeFeat.Name";
        [Translate("The soulknife gains the ability to shape her mind blade into different weapon forms. She may change her blade’s form to the light weapon, one-handed weapon, or two-handed weapon forms any time she forms her mind blade. Additionally, the soulknife may choose to form her mind blade into a one-handed weapon and a light weapon instead of two light weapons.", true)]
        private static readonly string Description = "AlterBladeFeat.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddFeatureIfHasFact(ShapeMindBladeFreeAbility.BlueprintInstance, ShapeMindBladeFreeAbility.BlueprintInstance, true)
                .Configure();
        }

    }
}
