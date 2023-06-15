using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Feats
{
    [ComponentName("Add psionic body hit points")]
    [AllowedOn(typeof(BlueprintFeature), false)]
    [TypeId("d540a86a-d724-41e4-8c9f-756a71f60525")]
    public class PsionicBodyLogic : UnitFactComponentDelegate, IOwnerGainLevelHandler
    {
        public override void OnTurnOn()
        {
            this.Apply();
        }

        // Token: 0x0600E538 RID: 58680 RVA: 0x003AA0C2 File Offset: 0x003A82C2
        public override void OnTurnOff()
        {
            base.Owner.Stats.HitPoints.RemoveModifiersFrom(base.Runtime);
        }

        // Token: 0x0600E539 RID: 58681 RVA: 0x003AA0DF File Offset: 0x003A82DF
        public void HandleUnitGainLevel()
        {
            this.Apply();
        }

        // Token: 0x0600E53A RID: 58682 RVA: 0x003AA0E8 File Offset: 0x003A82E8
        private void Apply()
        {
            base.Owner.Stats.HitPoints.RemoveModifiersFrom(base.Runtime);
            int value = this.Owner.Facts.GetAll<Feature>().Where(c => Main.PsionicFeats.Contains(c.Blueprint) && c.IsTurnedOn).Select(c => c.Rank).Sum();
            base.Owner.Stats.HitPoints.AddModifier(2 * value, base.Runtime, ModifierDescriptor.UntypedStackable);
        }
    }

    public class PsionicBody
    {
        private static readonly string FeatName = "Psionic Body";
        private static readonly string FeatGUID = "4addf906-6f7f-459a-8f4e-15580504bfb0";
        public static BlueprintFeature BlueprintInstance = null;

        [Translate("Psionic Body")]
        private static readonly string DisplayName = "PsionicBody.Name";
        [Translate("When you take this feat, you gain 2 hit points for each psionic feat you have (including this one). Whenever you take a new psionic feat, you gain 2 more hit points.", true)]
        private static readonly string Description = "PsionicBody.Description";
        private static readonly string Icon = "assets/icons/psionicbody.png";

        public static void Configure()
        {
            BlueprintInstance = FeatureConfigurator.New(FeatName, FeatGUID, FeatureGroup.Feat)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(Icon)
                .AddComponent<PsionicBodyLogic>()
                .Configure();
        }

    }
}
