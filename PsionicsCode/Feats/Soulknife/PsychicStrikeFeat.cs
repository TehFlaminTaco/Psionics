using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class PsychicStrikeFeat
    {
        private static readonly string FeatName = "PsychicStrikeFeat";
        private static readonly string FeatGUID = "52964536-8b60-4436-acd3-98a6e652ffb7";
        public static BlueprintFeature BlueprintInstance = null;

        private static readonly string DisplayName = "PsychicStrikeAbility.Name";
        private static readonly string Description = "PsychicStrikeAbility.Description";
        private static readonly string Icon = "assets/icons/psychicstrike.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetIsClassFeature(true)
                .AddFeatureIfHasFact(PsychicStrikeAbility.BlueprintInstance, PsychicStrikeAbility.BlueprintInstance, true)
                .AddFeatureIfHasFact(PsychicStrikeFreeAbility.BlueprintInstance, PsychicStrikeFreeAbility.BlueprintInstance, true)
                .SetRanks(10)
                .Configure();
        }
    }
}
