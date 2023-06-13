using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Psionics.Abilities.Soulknife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Buffs
{

    [ComponentName("Lightning Blade Shock")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("7e755ac1-f7fe-494c-ba88-cae7dd63dfe7")]
    public class LightningBladeShock : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IInitiatorRulebookHandler<RuleCalculateWeaponStats>
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            bool overCharged = false;
            if (evt.Initiator?.Body?.Armor.HasArmor ?? false)
            {
                if (evt.Initiator.Body.Armor.Armor.Blueprint.Type.ShardItem == ItemRefs.MetalShardItem.Reference.Get())
                    overCharged = true;
            }
            evt.AddModifier(new Modifier(overCharged ? -3 : -2, this.Fact, ModifierDescriptor.UntypedStackable));
        }

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            bool overCharged = false;
            if (evt.Initiator?.Body?.Armor.HasArmor ?? false)
            {
                if (evt.Initiator.Body.Armor.Armor.Blueprint.Type.ShardItem == ItemRefs.MetalShardItem.Reference.Get())
                    overCharged = true;
            }
            evt.AddDamageModifier(overCharged ? -3 : -2, this.Fact, ModifierDescriptor.UntypedStackable);
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            
        }
    }

    public class LightningBladeDebuff
    {
        private static readonly string FeatGUID = "7fcc70dd-dd42-46bf-a181-3df31dd7e3b1";
        public static BlueprintBuff BlueprintInstance = null;
        private static readonly string Icon = "assets/icons/lightningblade.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New($"LightningBladeDebuff", FeatGUID)
                .SetDisplayName($"LightningBladeDebuff.Name".Translate($"Lightning Blade Shock"))
                .SetDescription($"LightningBladeFeat.Description")
                .SetIcon(Icon)
                .AddComponent<LightningBladeShock>()
                .Configure();
        }
    }
}
