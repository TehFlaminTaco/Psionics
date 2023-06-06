using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Psionics.Abilities;
using Psionics.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class BladeRushFeat
    {
        private static readonly string FeatName = "BladeRushFeat";
        private static readonly string FeatGUID = "4d5c7f6b-f38b-46a6-8fe8-8780c2621160";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Blade Rush")]
        private static readonly string DisplayName = "BladeRushFeat.Name";
        [Translate("The soulknife rushes forward with a dash of incredible speed. As a Swift action, the soulknife may expend her psionic focus and move up to her speed without provoking attacks of opportunity. The soulknife must be at least 6th level in order to select this blade skill.", true)]
        private static readonly string Description = "BladeRushFeat.Description";
        private static readonly string Icon = "assets/icons/bladerush.png";

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
                    prerequisiteClassLevel.Level = 6;
                    prerequisiteClassLevel.CheckInProgression = false;
                    bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                })
                .AddFeatureIfHasFact(BladeRushAbility.BlueprintInstance, BladeRushAbility.BlueprintInstance, true)
                .Configure(true);
        }
    }
}
