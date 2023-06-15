using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine.Serialization;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    [TypeId("2051e0d2-5adf-4d5a-af22-19df1c8312d4")]
    public class IncreaseFeatureRank : UnitFactComponentDelegate
    {
        [FormerlySerializedAs("Feature")]
        public BlueprintFeature m_Feature;
        public override void OnActivate()
        {
            foreach (Feature feature in base.Owner.Progression.Features)
            {
                if (feature.Blueprint == m_Feature)
                {
                    feature.AddRank();
                }
            }
        }

        public override void OnDeactivate()
        {
            foreach (Feature feature in base.Owner.Progression.Features)
            {
                if (feature.Blueprint == m_Feature)
                {
                    if (feature.Rank > 1)
                    {
                        feature.RemoveRank();
                    }
                    else
                    {
                        PFLog.Default.Error("Can't decrease rank to 0");
                    }
                }
            }
        }
    }

    public class PowerfulStrikes
    {
        private static readonly string FeatName = "PowerfulStrikes";
        private static readonly string FeatGUID = "b2fc198b-af65-4dfa-9e95-e46aebf128b7";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Powerful Strikes")]
        private static readonly string DisplayName = "PowerfulStrikes.Name";
        [Translate("The soulknife’s psychic strike deals an additional 1d8 damage.", true)]
        private static readonly string Description = "PowerfulStrikes.Description";
        private static readonly string Icon = "assets/icons/powerfulstrikes.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddPrerequisiteFeature(PsychicStrikeFeat.BlueprintInstance)
                .SetIcon(Icon)
                .AddComponent<IncreaseFeatureRank>(f=>f.m_Feature = PsychicStrikeFeat.BlueprintInstance)
                .Configure();
        }

    }
}
