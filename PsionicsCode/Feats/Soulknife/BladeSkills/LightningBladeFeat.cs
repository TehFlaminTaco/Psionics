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
    public class LightningBladeFeat
    {
        private static readonly string FeatName = "LightningBladeFeat";
        private static readonly string FeatGUID = "2281a291-02d8-4baf-a39f-178f76780a76";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Lightning Blade")]
        private static readonly string DisplayName = "LightningBladeFeat.Name";
        [Translate("When the soulknife makes an attack with her mind blade, she can choose to have it deal electricity damage instead of its normal damage. In addition, the soulknife can expend her psionic focus when she hits with an attack to give the target a -2 penalty to attack and damage rolls until the end of her next turn. If the target is wearing metal armor, the penalty increases to -3. The soulknife must be at least 8th level to choose this blade skill.", true)]
        private static readonly string Description = "LightningBladeFeat.Description";
        private static readonly string Icon = "assets/icons/lightningblade.png";

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
                .AddFeatureIfHasFact(LightningBladeAbility.BlueprintInstance, LightningBladeAbility.BlueprintInstance, true)
                .AddFeatureIfHasFact(LightningBladeSpendFocusAbility.BlueprintInstance, LightningBladeSpendFocusAbility.BlueprintInstance, true)
                .Configure(true);
        }

    }
}
