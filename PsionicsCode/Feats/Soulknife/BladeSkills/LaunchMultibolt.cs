using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class LaunchMultibolt
    {
        private static readonly string FeatName = "LaunchMultibolt";
        private static readonly string FeatGUID = "1795ebe4-d386-48af-a102-93ebf5d73109";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Launch Multibolt")]
        private static readonly string DisplayName = "LaunchMultibolt.Name";
        [Translate("The soulknife gains the benefits of the Manyshot feat. The soulknife must possess the Launch Mindbolt class feature to select this blade skill.", true)]
        private static readonly string Description = "LaunchMultibolt.Description";
        private static readonly string Icon = "assets/icons/launchmultibolt.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddPrerequisiteFeature(FormMindBoltFeat.BlueprintInstance)
                .AddPrerequisiteNoFeature(FeatureRefs.Manyshot.Reference.Get())
                .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 6)
                .SetIcon(Icon)
                .AddFeatureIfHasFact(FeatureRefs.Manyshot.Reference.Get(), FeatureRefs.Manyshot.Reference.Get(),true)
                .Configure(true);
        }

    }
}
