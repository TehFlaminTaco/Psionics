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
    public class ThunderBladeFeat
    {
        private static readonly string FeatName = "ThunderBladeFeat";
        private static readonly string FeatGUID = "5d8a8313-4272-4ac1-b284-241dca25abca";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Thunder Blade")]
        private static readonly string DisplayName = "ThunderBladeFeat.Name";
        [Translate("When the soulknife makes an attack with her mind blade, she can choose to have it deal sonic damage instead of its normal damage, although the damage of the attack is halved. In addition, the soulknife can expend her psionic focus when she hits with an attack to stagger the target until the end of her next turn unless the target makes a successful Fortitude save (DC 10 + the soulknife’s base attack bonus). The soulknife must be at least 8th level to choose this blade skill.", true)]
        private static readonly string Description = "ThunderBladeFeat.Description";
        private static readonly string Icon = "assets/icons/thunderblade.png";

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
                .AddFeatureIfHasFact(ThunderBladeAbility.BlueprintInstance, ThunderBladeAbility.BlueprintInstance, true)
                .AddFeatureIfHasFact(ThunderBladeSpendFocusAbility.BlueprintInstance, ThunderBladeSpendFocusAbility.BlueprintInstance, true)
                .Configure(true);
        }

    }
}
