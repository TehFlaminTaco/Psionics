using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Feats.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats
{
    public class ExtraBladeSkill
    {
        private static readonly string FeatName = "ExtraBladeSkill";
        private static readonly string FeatGUID = "ac53e168-f191-4972-91ea-0d8eadfb23e8";
        public static BlueprintFeatureSelection BlueprintInstance = null;

        [Translate("Extra Blade Skill")]
        private static readonly string DisplayName = "ExtraBladeSkill.Name";
        [Translate("You gain an additional blade skill.", true)]
        private static readonly string Description = "ExtraBladeSkill.Description";
        private static string Icon = "assets/icons/formmindblade.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureSelectionConfigurator.New(FeatName, FeatGUID, FeatureGroup.Feat)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 2)
                .SetRanks(10)
                .OnConfigure(c=> {
                    c.m_AllFeatures = BladeSkillsFeat.BlueprintInstance.m_AllFeatures;
                    PrerequisiteFeature feat = new PrerequisiteFeature()
                    {
                        m_Feature = BladeSkillsFeat.BlueprintInstance.ToReference<BlueprintFeatureReference>()
                    };
                    c.ComponentsArray = c.ComponentsArray.Append(feat).ToArray();
                })
                .Configure(true);
        }

    }
}
