using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using static Kingmaker.RuleSystem.Rules.RuleCalculateAttacksCount;
using static Pathfinding.Util.RetainedGizmos;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class DuelingBladeFeat
    {
        private static readonly string FeatName = "DuelingBladeFeat";
        private static readonly string FeatGUID = "39711a9e-3d60-4fec-abaf-d1534cce0475";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Dueling Blade")]
        private static readonly string DisplayName = "DuelingBladeFeat.Name";
        [Translate("When a soulknife is fighting defensively, or using the Combat Expertise feat, with a mind blade, and an opponent misses her in melee, she may expend her psionic focus to make an attack of opportunity against that opponent with her mind blade.", true)]
        private static readonly string Description = "DuelingBladeFeat.Description";
        private static readonly string Icon = "assets/icons/duelingblade.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(DuelingBladeAbility.BlueprintInstance, DuelingBladeAbility.BlueprintInstance, true)
                .Configure();
        }

    }
}
