using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Psion
{
    public class PsionProficiencies
    {
        private static readonly string FeatName = "PsionProficiencies";
        private static readonly string FeatGUID = "abc82533-adc8-4dbe-aec1-1bdd43cb646d";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Psion Proficiencies")]
        private static readonly string DisplayName = "PsionProficiencies.Name";
        [Translate("Psions are proficient with simple weapons. They are not proficient with any type of armor or shield. Armor does not, however, interfere with the manifestation of powers.", true)]
        private static readonly string Description = "PsionProficiencies.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddFacts(new List<BlueprintCore.Utils.Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>>() {
                    FeatureRefs.SimpleWeaponProficiency.Reference.Get()
                })
                .SetIsClassFeature(true)
                .Configure();
        }

    }
}
