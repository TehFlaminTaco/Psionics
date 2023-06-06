using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace Psionics.Buffs
{
    [TypeId("0a621433-0ef2-463d-a426-aa565d6b05cb")]
    public class SpendPsionicFocus : ContextAction
    {
        public override string GetCaption()
        {
            return "Spend Psioinic Focus";
        }

        public override void RunAction()
        {
            if(Context.MaybeCaster is not null)
                PsionicFocus.Spend(Context.MaybeCaster);
        }
    }

    [TypeId("41684ba0-3d5e-4593-88e6-edb67885e9be")]
    public class PrerequisiteHasPsionicFocus : BlueprintComponent, IAbilityCasterRestriction
    {
        public bool Not;

        public string GetAbilityCasterRestrictionUIText() {
            return $"{(Not ? "Has" : "No")} Psionic Focus!";
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return caster.Buffs.Enumerable.Any(c => c.Blueprint == PsionicFocus.BlueprintInstance) ^ Not;
        }
    }

    public static class PsionicFocusExt {
        public static AbilityConfigurator RequirePsionicFocus(this AbilityConfigurator ab, bool not = false)
        {
            return ab.AddComponent(new PrerequisiteHasPsionicFocus() { Not = not });
        }
    }

    public class PsionicFocus
    {
        private static readonly string BuffName = "PsionicFocus";
        private static readonly string BuffGUID = "94146fc6-b459-434c-a440-de6db3dd66e4";
        public static BlueprintBuff BlueprintInstance = null;

        [Translate("Psionic Focus")]
        private static readonly string DisplayName = "PsionicFocus.Name";
        [Translate("You have become Psionically Focused", true)]
        private static readonly string Description = "PsionicFocus.Description";
        private static readonly string Icon = "assets/icons/psionicfocus.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(BuffName, BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetStacking(StackingType.Rank)
                .SetIcon(Icon)
                .Configure();
        }

        public static void Spend(UnitEntityData unit)
        {
            var buff = unit.Buffs.Enumerable.FirstOrDefault(c => c.Blueprint == PsionicFocus.BlueprintInstance);
            if(buff.GetRank() <= 1)
            {
                buff.Deactivate();
                buff.Remove();
            }
            if (buff is not null)
                buff.RemoveRank();
        }
    }
}
