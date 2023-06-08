using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Abilities.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class FormMindBladeFeat
    {
        private static readonly string FeatName = "FormMindBladeFeat";
        private static readonly string FeatGUID = "3c636e23-cdfc-444d-867d-22359f5371fe";
        public static BlueprintFeature FormMindBladeBlueprint = null;

        private static readonly string DisplayName = "FormMindBladeFeat.Name";
        private static readonly string Description = "FormMindBladeFeat.Description";
        private static readonly string Icon = "assets/icons/formmindblade.png";



        public static void Configure()
        {
            FormMindBladeBlueprint = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetIsClassFeature(true)
                .AddFeatureIfHasFact(FormMindBladeAbility.BlueprintInstance, FormMindBladeAbility.BlueprintInstance, true)
                .AddFeatureIfHasFact(TwinMindBlade.BlueprintInstance, TwinMindBlade.BlueprintInstance, true)
                .Configure();
        }
    }
}
