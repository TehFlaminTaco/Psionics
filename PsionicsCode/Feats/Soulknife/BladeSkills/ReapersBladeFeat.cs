using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Microsoft.Build.Utilities;
using Psionics.Abilities.Soulknife.Bladeskills;
using Psionics.Buffs;
using Psionics.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats.Soulknife.BladeSkills
{
    public class ReapersBlade : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeaponResolve>
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeaponResolve evt)
        {
            
        }

        public void OnEventDidTrigger(RuleAttackWithWeaponResolve evt)
        {
            // Check for a melee mind blade
            if (!MindBladeItem.TypeInstances.Take(3).Contains(evt.AttackWithWeapon.Weapon?.Blueprint?.Type)) return;
            if ((evt.Target.HPLeft + evt.Target.TemporaryHP) > evt.Damage.Result) return;
            if (!evt.Initiator.Buffs.Enumerable.Any(c => c.Blueprint == PsychicStrikeBuff.BlueprintInstance))
                evt.Initiator.AddBuff(PsychicStrikeBuff.BlueprintInstance, Context);
            else
                evt.Initiator.Buffs.Enumerable.FirstOrDefault(c => c.Blueprint == PsychicStrikeBuff.BlueprintInstance)?.AddRank();

        }
    }

    public class ReapersBladeFeat
    {
        private static readonly string FeatName = "ReapersBladeFeat";
        private static readonly string FeatGUID = "ea36af2f-5480-4cfb-b953-37d83844b12d";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Reaper's Blade")]
        private static readonly string DisplayName = "ReapersBladeFeat.Name";
        [Translate("A soulknife with this ability automatically recharges her psychic strike ability if she reduces an enemy’s hit points to below 0 with a melee attack using her mind blade. A soulknife must be at least 10th level to choose this blade skill.", true)]
        private static readonly string Description = "ReapersBladeFeat.Description";
        private static readonly string Icon = "assets/icons/reapersblade.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .OnConfigure(bp =>
                {
                    PrerequisiteClassLevel prerequisiteClassLevel = new PrerequisiteClassLevel();
                    prerequisiteClassLevel.m_CharacterClass = Psionics.Classes.Soulknife.ClassBlueprint.ToReference<BlueprintCharacterClassReference>();
                    prerequisiteClassLevel.Level = 10;
                    prerequisiteClassLevel.CheckInProgression = false;
                    bp.ComponentsArray = bp.ComponentsArray.Append(prerequisiteClassLevel).ToArray();
                })
                .AddPrerequisiteFeature(PsychicStrikeFeat.BlueprintInstance)
                .SetIcon(Icon)
                .AddComponent<ReapersBlade>()
                .Configure(true);
        }

    }
}
