using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.RuleSystem;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Equipment
{
    public class MindBoltItem
    {
        public static BlueprintWeaponType[] TypeInstances = new BlueprintWeaponType[3];
        public static BlueprintItemWeapon[] BlueprintInstances = new BlueprintItemWeapon[3];

        private static string[] TypeGUIDs = new[]
        {
            "168b0be1-825d-4f42-9feb-eb59ddd48a4c",
            "718b06ca-e2d5-4306-b646-331a565f4548",
            "97d4ab98-6911-4050-8612-cbbca4fd9616"
        };
        private static string[] ItemGUIDs = new[]
        {
            "ebd18e1e-3d43-4d1c-b423-5e57887346bc",
            "ec052527-21db-4d98-9812-6410f61a3269",
            "84929ddd-d3bb-46bf-90f5-236db8f05098"
        };
        private static string[] DisplayNames = new[] {
            "MindBoltShort.Name".Translate("Mind Bolt (Short Range)"),
            "MindBoltMedium.Name".Translate("Mind Bolt (Medium Range)"),
            "MindBoltLong.Name".Translate("Mind Bolt (Long Range)")
        };
        private static string Description = "MindBolt.Description".Translate("A bolt of psionic energy");
        private static DiceType[] DamageSizes = new[]
        {
            DiceType.D10,
            DiceType.D8,
            DiceType.D6
        };
        private static Feet[] Ranges = new[] {
            20.Feet(),
            60.Feet(),
            100.Feet()
        };


        public static string[] MindBoltShapes = new[] { "Short", "Medium", "Long" };

        public static void Configure()
        {
            for(int fsIndex = 0; fsIndex < 3; fsIndex++)
            {
                var config = WeaponTypeConfigurator.New($"MindBolt{MindBoltShapes[fsIndex]}", TypeGUIDs[fsIndex])
                    .CopyFrom(WeaponTypeRefs.LightCrossbow)
                    .ModifyVisualParameters(vp => vp.m_WeaponModel = null)
                    .SetIsTwoHanded(fsIndex != 1)
                    .SetCategory(Kingmaker.Enums.WeaponCategory.KineticBlast)
                    .SetCriticalRollEdge(19)
                    .SetCriticalModifier(Kingmaker.Enums.Damage.DamageCriticalModifierType.X2)
                    .SetBaseDamage(new()
                    {
                        m_Dice = DamageSizes[fsIndex],
                        m_Rolls = 1
                    })
                    .SetVisualParameters(WeaponTypeRefs.LightCrossbow.Reference.Get().VisualParameters.Clone())
                    .ModifyVisualParameters(vp =>
                    {
                        vp.m_WeaponModel = WeaponTypeRefs.Dart.Reference.Get().VisualParameters.m_WeaponModel;
                        vp.m_Projectiles = new[]
                        {
                            BlueprintTool.GetRef<BlueprintProjectileReference>("2e3992d1695960347a7f9bdf8122966f")
                        };
                        vp.m_WeaponAnimationStyle = Kingmaker.View.Animation.WeaponAnimationStyle.Fist;

                    })
                    .AddToFighterGroupFlags(WeaponFighterGroupFlags.Bows)
                    .SetIcon("assets/icons/mindbolt.png")
                    .SetAttackRange(Ranges[fsIndex]);
                if (fsIndex == 0) config.AddToEnchantments(WeaponEnchantmentRefs.StrengthThrown.Reference.Get());
                MindBladeItem.TypeInstances[fsIndex + 3] = TypeInstances[fsIndex] = config.Configure();

                MindBladeItem.BlueprintInstances[fsIndex + 3] = BlueprintInstances[fsIndex] = ItemWeaponConfigurator.New($"MindBolt{MindBoltShapes[fsIndex]}Item", ItemGUIDs[fsIndex])
                    .CopyFrom(ItemWeaponRefs.StandardLightCrossbow)
                    .SetType(TypeInstances[fsIndex])
                    .SetDisplayNameText(DisplayNames[fsIndex])
                    .SetDescriptionText(Description)
                    .SetIcon("assets/icons/mindbolt.png")
                    .Configure();
            }
        }
    }
}
