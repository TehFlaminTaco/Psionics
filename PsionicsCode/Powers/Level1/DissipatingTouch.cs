using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Psionics;
using System;
using System.Linq;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using HarmonyLib;
using BlueprintCore.Blueprints.References;
using Psionics.Buffs;
using Kingmaker.RuleSystem.Rules.Damage;
using static Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription;
using Kingmaker.Enums.Damage;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace Psionics.Powers.Level1
{
    public class DissipatingTouch
    {
        private static readonly AbilityRange Range = AbilityRange.Touch;
        private static readonly CommandType ActionType = CommandType.Standard;
        private static readonly AbilityType TypeAbility = AbilityType.Spell;

        private static readonly string AbilityName = "DissipatingTouch";
        private static readonly string AbilityGUID = "4a03f571-d141-4291-b388-091add44d6d9";

        public static BlueprintAbility BlueprintInstance = null;
        public static BlueprintAbility CastBlueprint = null;
        public static Augment PowerAugment = new Augment()
        {
            BaseName = AbilityName,
            Name = "Power",
            Description = $"{AbilityName}_Power.Description".Translate("For every additional power point you spend, this power’s damage increases by 1d6 points."),
            Cost = 1,
            MaxRank = 29
        };

        [Translate("Dissipating Touch")]
        private static readonly string DisplayName = $"{AbilityName}.Name";
        [Translate("Your mere touch can disperse the surface material of a foe or object, sending a tiny portion of it far away. This effect is disruptive; thus, your successful melee touch attack deals 1d6 points of damage.\nAugment For every additional power point you spend, this power’s damage increases by 1d6 points.")]
        private static readonly string Description = $"{AbilityName}.Description";
        private static readonly string Icon = "assets/icons/dissipatingtouch.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddSpellComponent(SpellSchool.Conjuration)
                .AddComponent<ConstantCountText>(cct=>cct.Text = -1)
                .AddComponent<HideDCFromTooltip>()
                .Augmentable(
                    a =>
                    {
                        a.Castable = CastBlueprint = AbilityConfigurator.New($"{AbilityName}_Cast".HashGUID(), HashGUID.Last)
                            .SetDisplayName(DisplayName)
                            .SetDescription(Description)
                            .SetIcon(Icon)
                            .SetRange(Range)
                            .SetActionType(ActionType)
                            .AddSpellComponent(SpellSchool.Conjuration)
                            .AddComponent<HideDCFromTooltip>()
                            .SetType(TypeAbility)
                            .SetAnimation(CastAnimationStyle.Directional)
                            .SetCanTargetFriends(false)
                            .SetCanTargetSelf(false)
                            .SetCanTargetEnemies(true)
                            .SetSpellResistance(true)
                            .AddAbilityEffectRunAction(ActionsBuilder.New()
                                .Add<ConfigureDamage>()
                                .Add<ContextActionDealDamage>(cadd => {
                                    cadd.DamageType = new DamageTypeDescription() {
                                        Type = DamageType.Untyped
                                    };
                                    cadd.Duration = ContextDuration.Fixed(0);
                                    cadd.Value = ContextDice.Value(DiceType.D6, 1, 0);
                                })
                            )
                            .AddComponent<AbilityCostPowerpoints>(c => {
                                c.m_Cost = 1;
                                c.AugmentableHolder = a;
                            })
                            .Configure();
                        a.Augments = new[] {
                            PowerAugment
                        };
                        a.BaseName = AbilityName;
                        a.BaseCost = 1;
                    }
                )
                .SetIcon(Icon)
                .Configure();
        }
    }
}
