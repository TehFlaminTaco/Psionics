using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using BlueprintCore.Conditions.Builder;

namespace Psionics.Buffs
{
    public class DisruptedBuff
    {
        private static readonly string BuffName = "DisruptedBuff";
        private static readonly string BuffGUID = "1b78a29b-dfa1-43eb-86a4-344d026b6768";

        public static BlueprintBuff BlueprintInstance = null;

        [Translate("Disrupted")]
        private static readonly string DisplayName = "DisruptedBuff.Name";
        [Translate("Target takes a -5 penalty to all melee and ranged damage rolls.")]
        private static readonly string Description = "DisruptedBuff.Description";
        private static readonly string Icon = "assets/icons/disruptingstrike.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(BuffName, BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddDamageBonusConditional(new Kingmaker.UnitLogic.Mechanics.ContextValue()
                {
                    ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                    Value = -5
                }, false, ConditionsBuilder.New().AddTrue(), Kingmaker.Enums.ModifierDescriptor.UntypedStackable, false)
                .Configure();
        }
    }
}
