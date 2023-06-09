using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using Kingmaker;
using Kingmaker.Blueprints.CharGen;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Items.Slots;
using Kingmaker.Utility;
using Owlcat.Runtime.Core.Utils;
using Psionics.Equipment;
using Kingmaker.ElementsSystem;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using UnityEngine.Serialization;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    [TypeId("0a621433-0ef2-463d-a426-aa565d6b05cb")]
    public class RequireMindBlade : BlueprintComponent, IAbilityCasterRestriction
    {
        [FormerlySerializedAs("AllowLight")]
        public bool m_AllowLight = true;
        [FormerlySerializedAs("AllowSword")]
        public bool m_AllowSword = true;
        [FormerlySerializedAs("AllowHeavy")]
        public bool m_AllowHeavy = true;
        public string GetAbilityCasterRestrictionUIText()
        {
            return "Caster not holding valid Mind Blade";
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            foreach (var blade in (new[] { caster.Body.PrimaryHand.HasWeapon ? caster.Body.PrimaryHand.Weapon : null, caster.Body.SecondaryHand.HasWeapon ? caster.Body.SecondaryHand.Weapon : null }).Where(c => c != null).Distinct())
            {
                var bladeType = blade.Blueprint.Type;
                if (bladeType == MindBladeItem.BlueprintInstances[0])
                    return m_AllowLight;
                if (bladeType == MindBladeItem.BlueprintInstances[1])
                    return m_AllowSword;
                if (bladeType == MindBladeItem.BlueprintInstances[2])
                    return m_AllowHeavy;

            }
            return false;
        }
    }

    public class BladestormAbility
    {

        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "BladestormAbility";
        private static readonly string AbilityGUID = "783f1c02-4ffc-495b-837a-59737d95773b";


        [Translate("Bladestorm")]
        private static readonly string DisplayName = "BladestormAbility.Name";
        [Translate("As a full attack, when wielding her mind blade, the soulknife can give up her regular attacks and instead throw one mind blade at her full attack bonus at all opponents within 30 feet, ignoring the normal range increments for throwing a mind blade. Regardless of the number of attacks she makes, she only provokes attacks of opportunity as though she made a single ranged attack. The soulknife must possess the Bladewind blade skill to take this ability, this ability may not be used if the mind blade is in a two-handed weapon form, and the soulknife must be at least 16th level to choose this blade skill.")]
        private static readonly string Description = "BladestormAbility.Description";
        private static readonly string Icon = "assets/icons/bladestorm.png";


        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddComponent<AbilityTargetsAround>(c=>
                {
                    c.m_Radius = 30.Feet();
                    c.m_TargetType = TargetType.Enemy;
                    c.m_IncludeDead = false;
                    c.m_SpreadSpeed = 0.Feet();
                    c.m_Flags = 0;
                    c.m_Condition = new ConditionsChecker();
                })
                .AddComponent<RequireMindBlade>(c=>
                {
                    c.m_AllowHeavy = false;
                })
                .AddComponent<HideDCFromTooltip>()
                .RequirePsionicFocus()
                .SetIcon(Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetType(AbilityType.Extraordinary)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .Add(new ForcedRangedAction()
                        {
                            SelectNewTarget = false,
                            AutoHit = false,
                            IgnoreStatBonus = false,
                            AutoCritThreat = false,
                            AutoCritConfirmation = false,
                            FullAttack = false,
                            ExtraAttack = true,
                            name = "$ForcedRangedAction$" + Guid.NewGuid().ToString()
                        })
                        .Add(new RemoveMindBlade()
                        {
                            name = "$RemoveMindBlade$" + Guid.NewGuid().ToString()
                        })
                        .Add(new SpendPsionicFocus()
                        {
                            name = "$SpendPsionicFocus$" + Guid.NewGuid().ToString()
                        })
                )
                .Configure();
        }

    }
}
