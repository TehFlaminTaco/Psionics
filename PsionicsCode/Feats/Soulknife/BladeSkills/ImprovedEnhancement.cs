using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class ImprovedEnhancement
    {
        private static readonly string FeatName = "ImprovedEnhancement";
        private static readonly string FeatGUID = "c27cb77b-03db-44a9-a4bf-9064449121e8";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Improved Enhancement")]
        private static readonly string DisplayName = "ImprovedEnhancement.Name";
        [Translate("The soulknife’s enhancement bonus on her mind blade increases by 1. This increase may be used to increase the actual enhancement bonus of the mind blade (to a maximum of +5) or be spent on weapon special abilities, as normal. A soulknife must be at least 12th level to choose this blade skill.", true)]
        private static readonly string Description = "ImprovedEnhancement.Description";
        private static readonly string Icon = "assets/icons/improvedenhancement.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddIncreaseResourceAmount(MindbladeEnhancement.BlueprintInstance, 1)
                .OnConfigure(bp =>
                {
                    PrerequisiteClassLevel prerequisiteClassLevel = new PrerequisiteClassLevel();
                    prerequisiteClassLevel.m_CharacterClass = Psionics.Classes.Soulknife.ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
                    prerequisiteClassLevel.Level = 12;
                    prerequisiteClassLevel.CheckInProgression = false;
                    bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                })
                .Configure(true);
        }

    }
}
