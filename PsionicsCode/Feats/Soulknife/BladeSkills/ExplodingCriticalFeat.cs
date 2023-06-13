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
    public class ExplodingCriticalFeat
    {
        private static readonly string FeatName = "ExplodingCriticalFeat";
        private static readonly string FeatGUID = "c09ae24e-60a5-41bb-860b-387ae3c1d157";
        public static BlueprintFeature BlueprintInstance = null;

        private static readonly string DisplayName = "ExplodingCriticalFeat.Name".Translate("Exploding Critical");
        private static readonly string Description = "ExplodingCriticalFeat.Description".Translate("When a soulknife confirms a critical hit, she can expend her psionic focus to deal her psychic strike damage, even if her mind blade was not charged with psychic strike, and even if she already dealt psychic strike on the attack. a soulknife must be at least 12th level to choose this blade skill.");
        private static readonly string Icon = "assets/icons/explodingcritical.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddPrerequisiteFeature(PsychicStrikeFeat.BlueprintInstance)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(ExplodingCriticalAbility.BlueprintInstance, ExplodingCriticalAbility.BlueprintInstance, true)
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
