using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Enums;

namespace Psionics.Feats.Soulknife
{
    public class SoulknifeProficiencies
    {
        private static readonly string FeatName = "SoulknifeProficiencies";
        private static readonly string FeatGUID = "ad7ff2c3-b905-4ef2-bb3a-1d3dee707a0f";
        public static BlueprintFeature SoulknifeProficienciesBlueprint = null;

        private static readonly string DisplayName = "SoulknifeProficiencies.Name";
        private static readonly string Description = "SoulknifeProficiencies.Description";

        public static void Configure()
        {
            SoulknifeProficienciesBlueprint = FeatureConfigurator.New(FeatName, FeatGUID)
                .AddFacts(new List<BlueprintCore.Utils.Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>>() {
                        "e70ecf1ed95ca2f40b754f1adb22bbdd", // Simple Weapon Proficiency,
                        "6d3728d4e9c9898458fe5e9532951132", // Light Armor Proficiency,
                        "46f4fb320f35704488ba3d513397789d",  // Medium Armor Proficiency
                        "cb8686e7357a68c42bdd9d4e65334633" // Shields proficiency
                })
                .AddProficiencies(new ArmorProficiencyGroup[] { }, null, new WeaponCategory[] { WeaponCategory.KineticBlast })
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIsClassFeature(true)
                .Configure();
        }

    }
}
