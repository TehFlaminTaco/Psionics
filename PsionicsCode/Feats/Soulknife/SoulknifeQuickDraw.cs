using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife
{
    public class SoulknifeQuickDraw
    {
        public static BlueprintFeature BlueprintInstance = null;
        private static readonly string FeatName = "SoulknifeQuickDraw";
        private static readonly string FeatGUID = "22c6274e-d98f-46cb-8b5d-38014abceddf";

        [Translate("Quick Draw")]
        private static readonly string DisplayName = "SoulknifeQuickDraw.Name";
        [Translate("A 5th level soulknife may manifest her mind blade as a free action, though she may still only attempt to do so once per round (unless throwing the weapon multiple times using the Multiple Throw blade skill).")]
        private static readonly string Description = "SoulknifeQuickDraw.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIsClassFeature(true)
                .Configure();
        }
    }
}
