using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class MindbladeEnchantmentFeats
    {
        private static readonly string[] FeatNames = new[]
        {
            "MindbladeEnchantment1Feat",
            "MindbladeEnchantment2Feat",
            "MindbladeEnchantment4Feat",
        };
        private static readonly string[] FeatGUIDs = new[]
        {
            "d4b28e36-06f7-4304-91fb-8aaa8fa30e8d",
            "6255cd6d-fd73-49fe-b7b8-cbf06e665900",
            "80b68c7a-184d-4cea-9b92-a6d3e84c7464",
        };
        private static readonly int[] EnchantCosts = new[] { 1, 2, 4 };

        public static BlueprintFeature[] BlueprintInstances = new BlueprintFeature[3];
        private static readonly string[] DisplayNames = new[] {
            "MindbladeEnchantment1Feat.Name".Translate("Mind Blade Enchantments (+1)"),
            "MindbladeEnchantment2Feat.Name".Translate("Mind Blade Enchantments (+2)"),
            "MindbladeEnchantment4Feat.Name".Translate("Mind Blade Enchantments (+4)"),
        };
        private static readonly string[] Descriptions = new[] {
            "MindbladeEnchantment1Feat.Description".Translate("The Soulknife can apply +1 Enchantments to their Mind Blades"),
            "MindbladeEnchantment2Feat.Description".Translate("The Soulknife can apply +2 Enchantments to their Mind Blades"),
            "MindbladeEnchantment4Feat.Description".Translate("The Soulknife can apply +4 Enchantments to their Mind Blades"),
        };
        private static readonly string Icon = "assets/icons/enhancedmindblade.png";

        public static void Configure()
        {
            for(int i=0; i < 3; i++)
            {
                var builder = FeatureConfigurator.New(FeatNames[i], FeatGUIDs[i])
                    .SetDisplayName(DisplayNames[i])
                    .SetDescription(Descriptions[i])
                    .SetIcon(Icon)
                    .SetIsClassFeature(true);

                foreach(var ench in EnhanceMindBladeAbility.enchantmentTable.Where(c=>c.Cost == EnchantCosts[i]))
                {
                    builder.AddFeatureIfHasFact(ench.Instance, ench.Instance, true);
                }

                BlueprintInstances[i] = builder.Configure();
            }
        }

    }
}
