using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class SoulknifeKineticBlastFeature
    {
        private static readonly string FeatName = "SoulknifeKinetBlastFeat";
        private static readonly string FeatGUID = "651c5378-5c4d-46e4-992e-253c448b6588";
        public static BlueprintFeature BlueprintInstance = null;

        private static readonly string DisplayName = "SoulknifeKinetBlastFeat.Name";
        private static readonly string Description = "SoulknifeKinetBlastFeat.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddParametrizedFeatures(
                    new Kingmaker.UnitLogic.FactLogic.AddParametrizedFeatures.FeatureData[]
                    {
                        new Kingmaker.UnitLogic.FactLogic.AddParametrizedFeatures.FeatureData()
                        {
                            m_Feature = BlueprintTool.GetRef<BlueprintParametrizedFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e"), // Weapon Focus
                            ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.KineticBlast
                        }
                    }
                )
                .Configure();

        }
    }
}
