using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities.Soulknife;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class TowerMindShieldFeat
    {
        private static readonly string FeatName = "TowerMindShieldFeat";
        private static readonly string FeatGUID = "8bc72b62-0902-4a68-90e9-76f65457f60b";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Tower Mind Shield")]
        private static readonly string DisplayName = "TowerMindShieldFeat.Name";
        [Translate("The soulknife’s mind shield can be shaped into a tower shield. Altering the mind shield in this fashion is a move action that does not provoke attacks of opportunity. The mind shield remains in this form until shaped back into its standard form. The mind shield is treated in all ways (except visually) as a masterwork tower shield, granting a +4 shield bonus to AC, imposing a +2 Maximum Dexterity Bonus, a -9 armor check penalty, and a 50% arcane spell failure chance. The soulknife must have the Mind Shield blade skill to select this blade skill.", true)]
        private static readonly string Description = "TowerMindShieldFeat.Description";
        private static readonly string Icon = "assets/icons/mindtowershield.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddPrerequisiteFeature(MindShield.BlueprintInstance, false)
                .AddFeatureIfHasFact(FormTowerMindShield.BlueprintInstance, FormTowerMindShield.BlueprintInstance, true)
                .Configure();
        }

    }
}
