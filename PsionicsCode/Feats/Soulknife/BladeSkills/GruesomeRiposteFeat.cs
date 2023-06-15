using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
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
    public class GruesomeRiposteFeat
    {
        private static readonly string FeatName = "GruesomeRiposteFeat";
        private static readonly string FeatGUID = "13102c2d-24fa-4388-b6bc-8ba246b0be7f";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Gruesome Riposte")]
        private static readonly string DisplayName = "GruesomeRiposteFeat.Name";
        [Translate("The soulknife may expend her psionic focus as an immediate action to attack an enemy who has successfully struck her in melee. This attack is assumed to happen after the successful attack, so she cannot use this ability if the attack would put her below 0 hit points, nor does dropping her enemy below 0 hit points prevent the attack from hitting. A soulknife must be at least 10th level to choose this blade skill.", true)]
        private static readonly string Description = "GruesomeRiposteFeat.Description";
        private static readonly string Icon = "assets/icons/gruesomeriposte.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(GruesomeRiposteAbility.BlueprintInstance, GruesomeRiposteAbility.BlueprintInstance, true)
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
