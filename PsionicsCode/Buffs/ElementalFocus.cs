using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Buffs
{

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("1714adc9-c972-4ffb-baf9-fd49a73b56c3")]
    public class ElectricalBonusAttack : BlueprintComponent {}
    [AllowedOn(typeof(BlueprintBuff))]
    [TypeId("256bbb03-b706-48a8-93ee-2fd7a52e435d")]
    public class ElectricalFocusBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.Reason?.Ability?.Blueprint?.Components.OfType<ElectricalBonusAttack>().Any() ?? false)
            {
                if((evt.Target?.Body?.Armor?.HasArmor ?? false) && (evt.Target.Body.Armor.Armor?.Blueprint?.ShardItem))
                {
                    evt.AddModifier(new Modifier(3, this.Fact, Kingmaker.Enums.ModifierDescriptor.UntypedStackable));
                }
            }
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }
    }

    public class ElementalFocus
    {
        private static readonly string BuffName = "ElementalFocus";
        private static readonly string Description = "ElementalFocus.Description".Translate("When gaining Psionic Focus, you may choose to focus Fire, Cold, Electricity, or Sonic, which may affect powers manifested.", true);
        public static string[] BuffGUIDs = new[]
        {
            "3a5d8ac5-7ddd-49f6-8738-66ad8e8b3e15",
            "c0811570-4af9-4e06-bca9-7f92c78117af",
            "a27d4d6f-ef8a-4f92-8068-6a340aec7b3b",
            "88a5cfe7-0f97-4027-b797-d9d97b41d4cc"
        };
        public static string[] ElementNames = new[]
{
            "Fire",
            "Cold",
            "Electricity",
            "Sonic"
        };
        private static string[] ElementIcons = new[]
        {
            "assets/icons/firefocus.png",
            "assets/icons/coldfocus.png",
            "assets/icons/electricfocus.png",
            "assets/icons/sonicfocus.png"
        };

        public static BlueprintBuff[] BlueprintInstances = new BlueprintBuff[4];

        public static void Configure()
        {
            for(int i=0; i < 4; i++)
            {
                var builder = BuffConfigurator.New($"{BuffName}{ElementNames[i]}", BuffGUIDs[i])
                    .SetDisplayName($"{BuffName}{ElementNames[i]}.Name".Translate($"{ElementNames[i]} Focus"))
                    .SetDescription(Description)
                    .SetIcon(ElementIcons[i]);
                if (ElementNames[i] == "Electricity")
                    builder.AddComponent<ElectricalFocusBonus>();
                BlueprintInstances[i] = builder
                    .Configure();
            }
        }

    }
}
