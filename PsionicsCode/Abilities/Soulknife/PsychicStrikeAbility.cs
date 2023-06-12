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
    public class PsychicStrikeAbility
    {
        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "PsychicStrikeAbility";
        private static readonly string AbilityGUID = "8257450f-b057-4dd4-9c56-339b99af33c3";


        [Translate("Psychic Strike")]
        private static readonly string DisplayName = "PsychicStrikeAbility.Name";
        [Translate("As a move action, a soulknife of 3rd level or higher can imbue her mind blade with destructive psychic energy. This effect deals an extra 1d8 points of damage on any attack she wishes to activate it on (as long as the attack is made with her mind blade). A soulknife may hold the charge as long as she likes without discharging. It does not go off on any attack unless she chooses to use it, and the charge is not wasted if an attack misses. Mindless creatures are immune to this damage, although non-mindless creatures immune to mind-affecting effects are affected by this damage as normal. (Unlike the rogue’s sneak attack, the psychic strike is not precision damage and can affect creatures otherwise immune to extra damage from critical hits or more than 30 feet away.) A mind blade deals this extra damage only once when this ability is called upon, but a soulknife can imbue her mind blade with psychic energy again by taking another move action. Additionally, she may recharge it as a swift action by expending her psionic focus.\nOnce a soulknife has prepared her blade for a psychic strike, it holds the extra energy until it is used (whether the attack is successful or not). Even if the soulknife drops the mind blade (or it otherwise dissipates, such as when it is thrown), it is still imbued with psychic energy when the soulknife next materializes it.\nIf the soulknife forms her mind blade into two weapons, she may imbue each mind blade with psychic strike as normal. If she reshapes her mind blade into a single weapon form, the additional psychic strike imbued into the additional weapon is lost. At every four levels thereafter (7th, 11th, etc), the extra damage from a soulknife’s psychic strike increases by 1d8.")]
        private static readonly string Description = "PsychicStrikeAbility.Description";
        private static readonly string Icon = "assets/icons/psychicstrike.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<HideDCFromTooltip>()
                .AddComponent<RequireMindBlade>( rmb => rmb.m_AllowBolt = true)
                .SetIcon(Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .SetHasFastAnimation(true)
                .SetRange(AbilityRange.Personal)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .ApplyBuffPermanent(PsychicStrikeBuff.BlueprintInstance, true, true, false, true, true, false, true)
                )
                .Configure();
        }
    }
}
