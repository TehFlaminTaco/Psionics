using BlueprintCore.Blueprints.Configurators.Items;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Utility;
using Psionics.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Equipment
{
    public class MindBladeItem
    {
        public static readonly string[] ItemGUIDs = new[]
        {
            "8d97b664-e250-46dc-afde-0fb7a95fd50f",
            "bf1d4fe0-c676-483e-bce4-e3516f300b38",
            "a16b1c6f-1a1a-4a1b-8072-4a6a535ba5a4"
        };
        public static readonly string[] TypeGUIDs = new[]
        {
            "746b0367-3152-4a3d-938c-583005bbbe80",
            "d1fc190a-8a4e-4a00-bb6c-69d79a0be31d",
            "3b13ead4-8ea4-49de-a4e9-06cf1de66e05"
        };
        public static BlueprintItemWeapon[] BlueprintInstances = new BlueprintItemWeapon[9];
        public static BlueprintWeaponType[] TypeInstances = new BlueprintWeaponType[9];
        public static Dictionary<string, string> ShapeToGenericItem = new()
        {
            ["Light"] = "f717b39c351b8b44388c471d4d272f4e",
            ["Sword"] = "6fd0a849531617844b195f452661b2cd",
            ["Heavy"] = "2fff2921851568a4d80ed52f76cccdb6"
        };
        public static Dictionary<string, string> ShapeToType = new()
        {
            ["Light"] = "a7da36e0e7bb60e42b9f23462ce2f4fc",
            ["Sword"] = "d56c44bc9eb10204c8b386a02c7eed21",
            ["Heavy"] = "5f824fbb0766a3543bbd6ae50248688f"
        };
        private static Dictionary<string, DiceFormula> ShapeToDice = new()
        {
            ["Light"] = new DiceFormula(1, DiceType.D6),
            ["Sword"] = new DiceFormula(1, DiceType.D8),
            ["Heavy"] = new DiceFormula(2, DiceType.D6)
        };
        private static readonly string DisplayName = "MindBlade.Name".Translate("Mind Blade");
        private static readonly string Description = "MindBlade.Description".Translate("A blade formed by a Soulknife");
        private static readonly string Icon = "assets/icons/mindblade.png";

        public static void Configure()
        {
            int fsIndex = 0;
            foreach (var Shape in ShapeMindBladeAbility.ShapeNames)
            {
                TypeInstances[fsIndex] = WeaponTypeConfigurator.New($"MindBlade{Shape}Type", TypeGUIDs[fsIndex])
                    .CopyFrom(ShapeToType[Shape])
                    .SetIsTwoHanded(Shape == "Heavy")
                    .SetIsLight(Shape == "Light")
                    .SetCategory(Kingmaker.Enums.WeaponCategory.KineticBlast)
                    .SetCriticalRollEdge(19)
                    .SetCriticalModifier(Kingmaker.Enums.Damage.DamageCriticalModifierType.X2)
                    .SetDamageType(new DamageTypeDescription()
                    {
                        Type = DamageType.Physical
                    })
                    .SetBaseDamage(ShapeToDice[Shape])
                    .SetIcon(Icon)
                    .SetWeight(0)
                    .Configure();
                BlueprintInstances[fsIndex] = ItemWeaponConfigurator.New($"MindBlade{Shape}Item", ItemGUIDs[fsIndex])
                    .CopyFrom(ShapeToGenericItem[Shape])
                    .ModifyVisualParameters(c=>c.m_Projectiles = c.m_Projectiles.Append(BlueprintTool.GetRef<BlueprintProjectileReference>("dbcc51cfd11fc1441a495daf9df9b340")).ToArray())
                    .SetType(TypeInstances[fsIndex])
                    .SetDisplayNameText(DisplayName)
                    .SetDescriptionText(Description)
                    .SetIcon(Icon)
                    .SetCost(0)
                    .Configure();
                fsIndex++;
            }
        }
    }
}
