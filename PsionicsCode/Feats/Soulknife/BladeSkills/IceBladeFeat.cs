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
    public class IceBladeFeat
    {
        private static readonly string FeatName = "IceBladeFeat";
        private static readonly string FeatGUID = "8e6f168b-3aaf-482e-8d0a-24a9ca08306c";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Ice Blade")]
        private static readonly string DisplayName = "IceBladeFeat.Name";
        [Translate("When the soulknife makes an attack with her mind blade, she can choose to have it deal cold damage instead of its normal damage. In addition, the soulknife can expend her psionic focus when she hits with an attack to reduce the target’s speed by half until the end of the soulknife’s next turn. The soulknife must be at least 8th level to choose this blade skill.", true)]
        private static readonly string Description = "IceBladeFeat.Description";
        private static readonly string Icon = "assets/icons/iceblade.png";

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
                .AddFeatureIfHasFact(IceBladeAbility.BlueprintInstance, IceBladeAbility.BlueprintInstance, true)
                .AddFeatureIfHasFact(IceBladeSpendFocusAbility.BlueprintInstance, IceBladeSpendFocusAbility.BlueprintInstance, true)
                .Configure(true);
        }

    }
}
