using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Psionics.Abilities.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Buffs
{
    public class MindBladeShapeBuff
    {
        private static readonly string[] FeatGUIDs = new[]
        {
            "cfe14ccd-3442-429e-896e-501e04fdc439",
            "46d869f5-ba36-4977-9008-d8e1218ed505",
            "cc5a92b4-0f3c-4695-b5ba-fd960a186be6"
        };
        public static BlueprintBuff[] BlueprintInstances = new BlueprintBuff[3];
        private static readonly string Icon = "assets/icons/shapemindblade.png";

        public static void Configure()
        {
            int fsIndex = 0;
            foreach (var Shape in ShapeMindBladeAbility.ShapeNames)
            {
                BlueprintInstances[fsIndex] = BuffConfigurator.New($"MindBladeShape{Shape}Buff", FeatGUIDs[fsIndex])
                    .SetDisplayName($"MindBladeShape{Shape}Buff.Name".Translate($"Shaped Mind Blade ({ShapeMindBladeAbility.ShapeTranslations[Shape]})"))
                    .SetDescription($"ShapeMindBladeAbility.Description")
                    .SetIcon(Icon)
                    .Configure();
                fsIndex++;
            }
        }
    }
}
