using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    [AllowedOn(typeof(BlueprintFeature), false)]
    [TypeId("af87ce37-4a01-4949-a8de-f0c876d41f27")]
    public class DeadlyBlowComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon != null && MindBladeItem.TypeInstances.Contains(evt.Weapon.Blueprint.Type))
            {
                evt.AdditionalCriticalMultiplier.Add(new Modifier(1, base.Fact, ModifierDescriptor.UntypedStackable));
            }
        }
        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }

    public class DeadlyBlow
    {
        private static readonly string FeatName = "DeadlyBlow";
        private static readonly string FeatGUID = "2041d65e-943c-414b-b23d-1ae2f8a60d2a";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Deadly Blow")]
        private static readonly string DisplayName = "DeadlyBlow.Name";
        [Translate("The soulknife’s mind blade critical multiplier increases by 1. A soulknife must be at least 10th level to choose this blade skill.", true)]
        private static readonly string Description = "DeadlyBlow.Description";
        private static readonly string Icon = "assets/icons/deadlyblow.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<DeadlyBlowComponent>()
                .OnConfigure(bp =>
                {
                    PrerequisiteClassLevel prerequisiteClassLevel = new PrerequisiteClassLevel();
                    prerequisiteClassLevel.m_CharacterClass = Psionics.Classes.Soulknife.ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
                    prerequisiteClassLevel.Level = 10;
                    prerequisiteClassLevel.CheckInProgression = false;
                    bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                })
                .Configure(true);
        }

    }
}
