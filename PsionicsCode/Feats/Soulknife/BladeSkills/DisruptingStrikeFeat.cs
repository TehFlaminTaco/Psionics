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
    public class DisruptingStrikeFeat
    {
        private static readonly string FeatName = "DisruptingStrikeFeat";
        private static readonly string FeatGUID = "8b8b97d3-c473-4201-8a43-ce1dc4b2d190";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Disrupting Strike")]
        private static readonly string DisplayName = "DisruptingStrikeFeat.Name";
        [Translate("As a full-round action, a soulknife can make one melee attack against each enemy adjacent to her. If she hits, the attack deals no damage, but each enemy hit takes a -5 penalty to all melee and ranged damage rolls until the start of the soulknife’s next turn. This blade skill may not be used with the mind bolt.", true)]
        private static readonly string Description = "DisruptingStrikeFeat.Description";
        private static readonly string Icon = "assets/icons/disruptingstrike.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(DisruptingStrikeAbility.BlueprintInstance, DisruptingStrikeAbility.BlueprintInstance, true)
                .Configure();
        }

    }
}
