using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Abilities.Soulknife
{
    public class PsychicStrikeFreeAbility
    {
        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "PsychicStrikeFreeAbility";
        private static readonly string AbilityGUID = "8257450f-b057-4dd4-9c56-339b99af33c3";


        [Translate("Psychic Strike (Swift)")]
        private static readonly string DisplayName = "PsychicStrikeFreeAbility.Name";
        private static readonly string Description = "PsychicStrikeAbility.Description";
        private static readonly string Icon = "assets/icons/psychicstrike.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<HideDCFromTooltip>()
                .AddComponent<RequireMindBlade>()
                .AddComponent<PrerequisiteHasPsionicFocus>()
                .SetIcon(Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .SetHasFastAnimation(true)
                .SetRange(AbilityRange.Personal)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .ApplyBuffPermanent(PsychicStrikeBuff.BlueprintInstance, true, true, false, true, true, false, true)
                        .Add<SpendPsionicFocus>()
                )
                .Configure();
        }
    }
}
