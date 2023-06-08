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

namespace Psionics.Abilities.Soulknife
{
    public class ShapeMindBladeAbility
    {
        private static readonly string AbilityName = "ShapeMindBladeAbility";
        private static readonly string AbilityGUID = "a18e8773-5510-43c4-98b2-ec70b1cd6dd0";

        public static BlueprintAbility[] FormShapes = null;
        public static BlueprintAbility BlueprintInstance = null;

        private static readonly string DisplayName = "ShapeMindBladeAbility.Name";
        public static readonly string Description = "ShapeMindBladeAbility.Description";
        private static readonly string Icon = "assets/icons/shapemindblade.png";


        public static readonly string[] ShapeNames = new[] { "Light", "Sword", "Heavy" };
        public static readonly Dictionary<string, string> ShapeTranslations = new Dictionary<string, string>()
        {
            ["Light"] = "Light",
            ["Sword"] = "One-Handed",
            ["Heavy"] = "Two-Handed",
        };
        private static readonly string[] VariantGUIDs = new[]
        {
            "698ada36-ab6f-4579-82a5-d110e5e9c6c1",
            "484ae06e-c4dc-4621-8bf3-b7f69c894619",
            "22c25154-9b29-4189-b81c-8ee564e5d371",
        };

        public static void Configure()
        {
            FormShapes = new BlueprintAbility[3];
            int fsIndex = 0;
            foreach (var Shape in ShapeNames)
            {
                FormShapes[fsIndex] = AbilityConfigurator.New($"Reform{Shape}", VariantGUIDs[fsIndex])
                    .SetDisplayName($"Reform{Shape}.Name".Translate($"Reform Mindblade ({ShapeTranslations[Shape]})"))
                    .SetDescription(Description)
                    .SetIcon(Icon)
                    .SetNotOffensive(true)
                    .SetCanTargetSelf(true)
                    .SetRange(AbilityRange.Personal)
                    .SetIsFullRoundAction(true)
                    .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
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
                .SetDescription(Description)
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
