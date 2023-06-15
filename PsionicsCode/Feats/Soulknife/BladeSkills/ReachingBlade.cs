using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class ReachingBlade
    {
        private static readonly string FeatName = "ReachingBlade";
        private static readonly string FeatGUID = "c561faca-21f9-449d-969b-2f72973051ad";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Reaching Blade")]
        private static readonly string DisplayName = "ReachingBladeFeat.Name";
        [Translate("The soulknife may expend her focus to increase her reach with her mind blade by 5 feet until the start of her next turn. Unlike normal reach weapons, a soulknife may also attack adjacent opponents with her mind blade. A soulknife must be at least 8th level to choose this blade skill.", true)]
        private static readonly string Description = "ReachingBladeFeat.Description";
        private static readonly string Icon = "assets/icons/reachingblade.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .OnConfigure(bp =>
                {
                    PrerequisiteClassLevel prerequisiteClassLevel = new PrerequisiteClassLevel();
                    prerequisiteClassLevel.m_CharacterClass = Psionics.Classes.Soulknife.ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
                    prerequisiteClassLevel.Level = 8;
                    prerequisiteClassLevel.CheckInProgression = false;
                    bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                })
                .AddFeatureIfHasFact(ReachingBladeAbility.BlueprintInstance, ReachingBladeAbility.BlueprintInstance, true)
                .Configure(true);
        }

    }
}
