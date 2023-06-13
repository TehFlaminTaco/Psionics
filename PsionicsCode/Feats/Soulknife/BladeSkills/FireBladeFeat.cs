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
    public class FireBladeFeat
    {
        private static readonly string FeatName = "FireBladeFeat";
        private static readonly string FeatGUID = "b8281266-ffa5-433e-9d36-cf9aaed270cc";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Fire Blade")]
        private static readonly string DisplayName = "FireBladeFeat.Name";
        [Translate("When the soulknife makes an attack with her mind blade, she can choose to have it deal fire damage instead of its normal damage. In addition, the soulknife can expend her psionic focus when she hits with an attack to deal an additional +1d10 fire damage. The soulknife must be at least 8th level to choose this blade skill.", true)]
        private static readonly string Description = "FireBladeFeat.Description";
        private static readonly string Icon = "assets/icons/fireblade.png";

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
                .AddFeatureIfHasFact(FireBladeAbility.BlueprintInstance, FireBladeAbility.BlueprintInstance, true)
                .AddFeatureIfHasFact(FireBladeSpendFocusAbility.BlueprintInstance, FireBladeSpendFocusAbility.BlueprintInstance, true)
                .Configure(true);
        }

    }
}
