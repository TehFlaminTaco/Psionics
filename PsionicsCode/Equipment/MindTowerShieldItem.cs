using BlueprintCore.Blueprints.Configurators.Items;
using BlueprintCore.Blueprints.Configurators.Items.Armors;
using BlueprintCore.Blueprints.Configurators.Items.Shields;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Shields;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Equipment
{
    public class MindTowerShieldItem
    {
        private static readonly string TypeName = "MindTowerShieldType";
        private static readonly string TypeGUID = "c1d40cb0-d97b-45e9-8752-a476645b6716";

        private static readonly string ItemName = "MindTowerShieldItem";
        private static readonly string ItemGUID = "0349d242-8af8-4e6b-a24f-156318998521";
        public static BlueprintItemShield BlueprintInstance = null;
        public static BlueprintShieldType TypeInstance = null;

        [Translate("Mind Tower Shield")]
        private static readonly string DisplayName = "MindTowerShieldItem.Name";
        [Translate("A Tower-shield made of Psychic Energy", true)]
        private static readonly string Description = "MindTowerShieldItem.Description";
        private static readonly string Icon = "assets/icons/mindtowershield.png";

        public static void Configure()
        {
            TypeInstance = ShieldTypeConfigurator.New(TypeName, TypeGUID)
                .CopyFrom(ShieldTypeRefs.TowerShieldType)
                .SetIcon(Icon)
                .Configure();

            BlueprintInstance = ItemShieldConfigurator.New(ItemName, ItemGUID)
                .CopyFrom(ItemShieldRefs.TowerShield)
                .SetDisplayNameText(DisplayName)
                .SetDescriptionText(Description)
                .SetIcon(Icon)
                .OnConfigure(bp=>bp.ArmorComponent.m_Type = TypeInstance.ToReference<BlueprintArmorTypeReference>())
                .Configure();
        }

    }
}
