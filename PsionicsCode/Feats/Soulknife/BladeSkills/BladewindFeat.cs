using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class BladewindFeat
    {
        private static readonly string FeatName = "BladewindFeat";
        private static readonly string FeatGUID = "b7cd4f07-4452-4fb7-a9ed-0ae30b7f65f0";
        public static BlueprintFeature BlueprintInstance = null;

        private static readonly string DisplayName = "BladewindAbility.Name";
        private static readonly string Description = "BladewindAbility.Description";
        private static readonly string Icon = "assets/icons/bladewind.png";

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
                .SetIsClassFeature(true)
                .AddFeatureIfHasFact(BladewindAbility.BlueprintInstance, BladewindAbility.BlueprintInstance, true)
                .AddFeatureIfHasFact(BladewindSpendPsionicStrikeAbility.BlueprintInstance, BladewindSpendPsionicStrikeAbility.BlueprintInstance, true)
                .Configure();
        }
    }
}
