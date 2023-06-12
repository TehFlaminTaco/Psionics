using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.MiscEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using Kingmaker.Items.Slots;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.TurnBasedMode.Controllers;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.View.Equipment;
using Microsoft.Build.Utilities;
using Psionics.Buffs;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using Kingmaker.UnitLogic;
using Psionics.Feats.Soulknife.BladeSkills;
using Psionics.Feats.Soulknife;
using Kingmaker.Blueprints.Items.Weapons;

namespace Psionics.Abilities.Soulknife
{
    public class FormMindBoltAbility
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityGUID = "4864fa8e-687d-4475-9e9c-208a78f54d25";

        [Translate("At 1st level, as a move action, a soulbolt can form a semi-solid ranged weapon composed of psychic energy distilled from her own mind. This mind bolt appears inside of or enveloping the soulbolt’s hand.\r\nA soulbolt must choose the form of her mind bolt at 1st level. She can either form it into a short range, medium range, or long range bolt which determines the weapon’s range increment and base damage. Once chosen, her mind bolt stays in this form every time the soulknife forms her mind bolt. The long range bolt form is the smallest of forms and deals 1d6 points of damage and has a 100 ft. range increment. The medium range bolt form deals 1d8 points of damage, with a 60 ft. range increment, and the short range bolt form is the largest and heaviest form and deals 1d10 points of damage with a 20 ft. range increment. A soulbolt adds her Strength modifier to damage rolls when using the mind bolt in the short range form. All damages are based on a Medium-sized creature wielding Medium-sized weapons; adjust the weapon damage as appropriate for different sized weapons. A soulbolt with powerful build or any similar ability forms an appropriately-sized mind bolt dealing the size-appropriate amount of damage. Regardless of form, the mind bolt has a 19-20 critical threat range, has a maximum range of 10 range increments, and is treated as a projectile.\nThe form of the soulbolt’s mind bolt also determines how many hands must be used to form and manipulate the mind bolt. If the mind bolt is in long range form, both of the soulbolt’s hands remain free to hold other items such as a shield or a weapon. If the mind bolt is in medium range form, the soulbolt must have at least one hand free to form and manipulate the mind bolt. If the mind bolt is in short range form, the soulbolt must have both hands free to wield and launch the mind bolt.\nA soulbolt can wield a buckler without penalty regardless of the form of her mind bolt.\nRegardless of the weapon form a soulbolt has chosen, her mind bolt does not have a set damage type. When shaping her weapon and assigning abilities to it, the soulbolt chooses whether it will deal bludgeoning, piercing, or slashing damage. The soulbolt may change the damage type of an existing mind bolt, or may summon a new mind bolt with a different damage type, as a full-round action; otherwise, the mind bolt retains the last damage type chosen every time it is summoned.\nThe bolt can be broken (it has hardness 10 and 10 hit points); however, a soulbolt can simply create another on her next move action. The moment she relinquishes her grip on her blade, it dissipates (unless she intends to throw it; see below). A mind bolt is considered a magic weapon for the purpose of overcoming damage reduction and is considered a masterwork weapon.\nA soulbolt can use feats such as Rapid Shot or Precise Shot in conjunction with the mind bolt just as if it were a normal ranged weapon. She can also choose her mind bolt for feats requiring a specific weapon choice, such as Weapon Focus and Improved Critical. Powers or spells that upgrade weapons can be used on a mind bolt. The soulbolt can use feats such as Weapon Finesse that work on light weapons with her mind bolt, but such feats only work on mind bolts in a light weapon form, such as using the Mind Daggers blade skill.\nEven in places where psionic effects do not normally function (such as within a null psionics field), a soulbolt can attempt to sustain her mind bolt by making a DC 20 Will save. On a successful save, the soulbolt maintains her mind bolt for a number of rounds equal to her class level before she needs to check again, although the mind bolt is treated for all purposes as a non-magical, masterwork ranged weapon while in a place where psionic effects do not normally function. On an unsuccessful attempt, the mind bolt vanishes. As a move action on her turn, the soulbolt can attempt a new Will save to rematerialize her mind bolt while she remains within the psionics-negating effect. She gains a bonus on Will saves made to maintain or form her mind bolt equal to the total enhancement bonus of her mind bolt (see below).\nThe soulbolt chooses the appearance of her mind bolt, although its shape must reflect the selections the soulbolt has chosen: a bludgeoning mind bolt would be blunt, slashing would have an edge, etc.\nThis ability replaces the Form Mind Blade class feature normally gained by soulknives.")]
        private static readonly string Description = "FormMindBoltAbility.Description";
        private static readonly string Icon = "assets/icons/mindbolt.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New($"FormMindBoltAbility", AbilityGUID)
                .SetDisplayName($"FormMindBoltAbility.Name".Translate("Form Mind Bolt"))
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(MindBoltBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
