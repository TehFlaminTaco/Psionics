using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.MiscEx;
using BlueprintCore.Actions.Builder.UpgraderEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Psionics.Buffs;
using Psionics.Classes;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    public class ShapeMindBladeFreeAbility
    {
        private static readonly string AbilityName = "ShapeMindBladeFreeAbility";
        private static readonly string AbilityGUID = "828737f1-ea8d-46b0-beb1-c8f62049d406";

        public static BlueprintAbility[] FormShapes = null;
        public static BlueprintAbility BlueprintInstance = null;

        [Translate("Shape Mind Blade (Free)")]
        private static readonly string DisplayName = "ShapeMindBladeFreeAbility.Name";
        private static readonly string Icon = "assets/icons/shapemindblade.png";


        public static readonly string[] ShapeNames = new[] { "Light", "Sword", "Heavy" };
        private static readonly string[] VariantGUIDs = new[]
        {
            "6d4b61aa-5134-4d24-9d4d-d7b34837d876",
            "b303ea3c-92a9-465a-bebc-2219eadf694d",
            "4e9eede0-4425-41ef-9c14-5b9f209d461d",
        };

        public static void Configure()
        {
            FormShapes = new BlueprintAbility[3];
            int fsIndex = 0;
            foreach (var Shape in ShapeNames)
            {
                FormShapes[fsIndex] = AbilityConfigurator.New($"Reform{Shape}Free", VariantGUIDs[fsIndex])
                    .SetDisplayName($"Reform{Shape}Free.Name")
                    .SetDescription(ShapeMindBladeAbility.Description)
                    .SetIcon(Icon)
                    .SetNotOffensive(true)
                    .SetCanTargetSelf(true)
                    .SetRange(AbilityRange.Personal)
                    .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                    .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                    .AddAbilityEffectRunAction(
                        ActionsBuilder.New()
                            .RemoveBuff(MindBladeShapeBuff.BlueprintInstances[0], true, false, true)
                            .RemoveBuff(MindBladeShapeBuff.BlueprintInstances[1], true, false, true)
                            .RemoveBuff(MindBladeShapeBuff.BlueprintInstances[2], true, false, true)
                            .ApplyBuffPermanent(MindBladeShapeBuff.BlueprintInstances[fsIndex], true, true, false, true, true, false, true)
                    )
                    .Configure();
                fsIndex++;
            }

            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(ShapeMindBladeAbility.Description)
                .SetIcon(Icon)
                .SetAbilityVariants(
                    new Kingmaker.Utility.Cacheable<Kingmaker.UnitLogic.Abilities.Components.AbilityVariants>()
                )
                .AddAbilityVariants(
                   FormShapes.Select(c => (BlueprintCore.Utils.Blueprint<BlueprintAbilityReference>)c.ToReference<BlueprintAbilityReference>()).ToList()
                )
                .Configure();
        }
    }
}
