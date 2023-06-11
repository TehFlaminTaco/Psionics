using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.MiscEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using Kingmaker.Items.Slots;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.TurnBasedMode.Controllers;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.View.Equipment;
using Microsoft.Build.Utilities;
using Psionics.Buffs;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using Kingmaker.UnitLogic;
using Psionics.Feats.Soulknife.BladeSkills;
using Psionics.Feats.Soulknife;
using Kingmaker.Blueprints.Items.Weapons;

namespace Psionics.Abilities.Soulknife
{
    public class FormTowerMindShield
    {
        public static BlueprintActivatableAbility BlueprintInstance = null;
        private static readonly string AbilityGUID = "4bbafe49-95d6-4b5b-8a20-a415bbf0bb56";

        private static readonly string DisplayName = "TowerMindShieldFeat.Name";
        private static readonly string Description = "TowerMindShieldFeat.Description";
        private static readonly string Icon = "assets/icons/mindtowershield.png";

        public static void Configure()
        {
            BlueprintInstance = ActivatableAbilityConfigurator.New($"FormTowerMindShield", AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .SetBuff(MindTowerShieldBuff.BlueprintInstance)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetDeactivateImmediately(true)
                .Configure();
        }
    }
}
