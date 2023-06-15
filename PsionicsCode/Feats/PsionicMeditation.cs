using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats
{
    public class PsionicMeditation
    {
        private static readonly string FeatName = "PsionicMeditation";
        private static readonly string FeatGUID = "534a6c7f-dea6-4fe5-bcb9-98298eb92500";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Psionic Meditation")]
        private static readonly string DisplayName = "PsionicMeditation.Name";
        [Translate("You can take a move action to become psionically focused.", true)]
        private static readonly string Description = "PsionicMeditation.Description";
        private static readonly string Icon = "assets/icons/psionicfocus.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID, FeatureGroup.Feat)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddPrerequisiteFeature(PowerPointPool.BlueprintInstance)
                .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Wisdom, 13)
                .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.SkillPerception, 4)
                .AddFeatureIfHasFact(GainPsionicFocusMoveAbility.BlueprintInstance)
                .Configure();
        }

    }
}
