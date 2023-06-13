using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    public class ExplodingCriticalAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityName = "ExplodingCriticalAbility";
        private static readonly string AbilityGUID = "92805bd7-b1de-42b1-af9b-2d7c141c2f36";

        private static readonly string DisplayName = "ExplodingCriticalFeat.Name".Translate("Exploding Critical");
        private static readonly string Description = "ExplodingCriticalFeat.Description".Translate("When a soulknife confirms a critical hit, she can expend her psionic focus to deal her psychic strike damage, even if her mind blade was not charged with psychic strike, and even if she already dealt psychic strike on the attack. a soulknife must be at least 12th level to choose this blade skill.");
        private static readonly string Icon = "assets/icons/explodingcritical.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(ExplodingCriticalBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
