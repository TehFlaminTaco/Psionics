using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    [ComponentName("Mind Blade Finesse Replacement")]
    [TypeId("1f15ee57-c10c-4e32-ad8a-dd922a9e1a95")]
    public class MindBladeFinesseReplacement : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>
    {
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            // Only apply to mind-blades
            if (!MindBladeItem.TypeInstances.Take(3).Contains(evt.Weapon?.Blueprint?.Type))
                return;

            evt.OverrideDamageBonusStat(Kingmaker.EntitySystem.Stats.StatType.Dexterity);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            
        }
    }

    public class MindBladeFinesse
    {
        private static readonly string FeatName = "MindBladeFinesse";
        private static readonly string FeatGUID = "b6782330-50f6-4deb-a097-22248719eba2";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Mind Blade Finesse")]
        private static readonly string DisplayName = "MindBladeFinesse.Name";
        [Translate("Mind Blade Finesse: The benefits of the Weapon Finesse feat apply to the mind blade even when it is in forms that cannot normally be the subject of Weapon Finesse (including two-handed forms).", true)]
        private static readonly string Description = "MindBladeFinesse.Description";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddPrerequisiteFeature(FeatureRefs.WeaponFinesse.Reference.Get())
                .AddComponent<MindBladeFinesseReplacement>()
                .Configure();
        }

    }
}
