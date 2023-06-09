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
    public class DazzlingBladeFeat
    {
        private static readonly string FeatName = "DazzlingBladeFeat";
        private static readonly string FeatGUID = "d1b2506e-6034-4962-82cc-0b84bb238781";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Dazzling Blade")]
        private static readonly string DisplayName = "DazzlingBladeFeat.Name";
        [Translate("The soulknife may, as a standard action, channel psionic energy into her mind blade, dazzling all creatures within 30 feet. A successful Fortitude save negates this effect. The save DC is 10 + the soulknife’s base attack bonus.", true)]
        private static readonly string Description = "DazzlingBladeFeat.Description";
        private static readonly string Icon = "assets/icons/dazzlingblade.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(DazzlingBladeAbility.BlueprintInstance, DazzlingBladeAbility.BlueprintInstance, true)
                .Configure();
        }

    }
}
