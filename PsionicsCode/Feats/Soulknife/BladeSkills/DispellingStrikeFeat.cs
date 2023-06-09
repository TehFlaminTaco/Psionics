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
    public class DispellingStrikeFeat
    {
        private static readonly string FeatName = "DispellingStrikeFeat";
        private static readonly string FeatGUID = "87903596-7d87-41d6-979a-56fc83656160";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Dispelling Strike")]
        private static readonly string DisplayName = "DispellingStrikeFeat.Name";
        [Translate("With this blade skill, the soulknife is capable of channeling her psychic strike damage into caustic, anti-psionic energy. As a standard action, the soulknife can expend her psionic focus and her psionic strike to make an attack with her mind blade (or equivalent weapon). If it hits, it deals weapon damage as normal, and the soulknife affects the target with a targeted dispel psionics power as a psilike ability, with a manifester level equal to her class level. The soulknife does not apply the expended psychic strike’s damage to her attack. The soulknife must be 8th level to select this blade skill.", true)]
        private static readonly string Description = "DispellingStrikeFeat.Description";
        private static readonly string Icon = "assets/icons/dispellingstrike.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(DispellingStrikeAbility.BlueprintInstance, DispellingStrikeAbility.BlueprintInstance, true)
                .OnConfigure(bp =>
                {
                    PrerequisiteClassLevel prerequisiteClassLevel = new PrerequisiteClassLevel();
                    prerequisiteClassLevel.m_CharacterClass = Psionics.Classes.Soulknife.ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
                    prerequisiteClassLevel.Level = 8;
                    prerequisiteClassLevel.CheckInProgression = false;
                    bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                })
                .Configure(true);
        }

    }
}
