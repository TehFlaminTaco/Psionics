using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Psionics.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class BladestormFeat
    {
        private static readonly string FeatName = "BladestormFeat";
        private static readonly string FeatGUID = "1e453914-4aa5-43eb-8abf-3a5b19d2be28";
        public static BlueprintFeature BlueprintInstance = null;

        private static readonly string DisplayName = "BladestormAbility.Name";
        private static readonly string Description = "BladestormAbility.Description";
        private static readonly string Icon = "assets/icons/bladestorm.png";

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
                    prerequisiteClassLevel.Level = 16;
                    prerequisiteClassLevel.CheckInProgression = false;
                    bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                })
                .AddFeatureIfHasFact(BladestormAbility.BlueprintInstance, BladestormAbility.BlueprintInstance, true)
                .Configure(true);
        }
    }
}
