using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
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
    public class RendingBladesCrit : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeaponResolve>
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeaponResolve evt)
        {
            
        }

        public void OnEventDidTrigger(RuleAttackWithWeaponResolve evt)
        {
            if (!MindBladeItem.TypeInstances.Take(3).Contains(evt.AttackWithWeapon.Weapon?.Blueprint?.Type)) return;
            if (evt.AttackRoll.IsCriticalConfirmed)
                evt.Target.AddBuff(BuffRefs.Bleed1d6Buff.Reference.Get(), Context);
        }
    }

    public class RendingBlades
    {
        private static readonly string FeatName = "RendingBlades";
        private static readonly string FeatGUID = "6de68c64-f314-4ae8-aa7b-eed06e7af1a5";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Rending Blades")]
        private static readonly string DisplayName = "RendingBlades.Name";
        [Translate("Hooks extend from the soulknife’s mind blade, dealing an additional 1d6 bleed damage on a critical hit.", true)]
        private static readonly string Description = "RendingBlades.Description";
        private static readonly string Icon = "assets/icons/rendingblades.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<RendingBladesCrit>()
                .Configure();
        }

    }
}
