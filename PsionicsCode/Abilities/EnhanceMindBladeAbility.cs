using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using Kingmaker.Utility;
using Microsoft.Build.Utilities;
using Psionics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using static Kingmaker.Kingdom.Settlements.BuildingComponents.BuildingTacticalUnitFactBonus;
using static Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbility;

namespace Psionics.Abilities
{
    [AllowedOn(typeof(BlueprintActivatableAbility), false)]
    [TypeId("5007f4aa-63f1-450b-98fd-ce1231f35cce")]
    public class TemporarilySpendResource : ActivatableAbilityRestriction, IActivatableAbilityStartHandler, IActivatableAbilityWillStopHandler
    {

        [FormerlySerializedAs("Cost")]
        public int m_Cost;
        [FormerlySerializedAs("MinimumOverhead")]
        public int m_MinimumOverhead;
        [FormerlySerializedAs("Resource")]
        public BlueprintAbilityResource m_Resource;
        private static uint Freebies = 0;
        public void HandleActivatableAbilityStart(ActivatableAbility ability)
        {
            if (ability != this.Fact) return;
            if (base.Owner.Resources.HasEnoughResource(m_Resource, m_Cost + m_MinimumOverhead))
            {
                Main.Logger.Info("Spending Points!");
                base.Owner.Resources.Spend(m_Resource, m_Cost);
                
            }
            else
            {
                // Horrifing hack to skip WillStop on this one
                Freebies++;
                Fact.TurnOffImmediately();
            }
        }

        public void HandleActivatableAbilityWillStop(ActivatableAbility ability){
            if (ability != this.Fact) return;
            if (Freebies > 0)
            {
                Freebies--;
                return;
            }
            Main.Logger.Info("Refunding Points!");
            base.Owner.Resources.Restore(m_Resource, m_Cost);
        }

        public override bool IsAvailable()
        {
            return this.Fact.IsOn || base.Owner.Resources.HasEnoughResource(m_Resource, m_Cost + m_MinimumOverhead);
        }
    }

    public class EnhanceMindBladeAbility
    {
        public class Enchantment
        {
            public string Target;
            public string GUID;
            public string Name;
            public string Icon;
            public int Cost;
            public BlueprintActivatableAbility Instance;
        }

        public static Dictionary<BlueprintActivatableAbility, Enchantment> enchantmentByBlueprint = new();

        public static Enchantment[] enchantmentTable = new Enchantment[]
        {
            new(){Target = "a36ad92c51789b44fa8a1c5c116a1328", GUID = "61d08e3f-cd91-4626-b695-9d432a4b75a3", Cost = 1, Icon = "assets/icons/agilemindblade.png", Name = "Agile"},
            new(){Target = "633b38ff1d11de64a91d490c683ab1c8", GUID = "f2872269-384b-428c-b3a1-8f2d7538a08c", Cost = 1, Icon = "assets/icons/corrosivemindblade.png", Name = "Corrosive"},
            new(){Target = "30f90becaaac51f41bf56641966c4121", GUID = "b38f9bb5-b3b4-4a05-8fde-32f3b91fe138", Cost = 1, Icon = "assets/icons/firemindblade.png", Name = "Flaming"},
            new(){Target = "421e54078b7719d40915ce0672511d0b", GUID = "b39f52fa-8695-4aea-be83-32245da2f5ce", Cost = 1, Icon = "assets/icons/frostmindblade.png", Name = "Frost"},
            new(){Target = "b606a3f5daa76cc40add055613970d2a", GUID = "4bd02d8b-d5f4-422f-a4da-67865774926f", Cost = 1, Icon = "assets/icons/furiousmindblade.png", Name = "Furious"},
            new(){Target = "47857e1a5a3ec1a46adf6491b1423b4f", GUID = "92df386e-f6b9-4268-a1db-930d1bd34aff", Cost = 1, Icon = "assets/icons/ghosttouchmindblade.png", Name = "Ghost Touch"},
            new(){Target = "102a9c8c9b7a75e4fb5844e79deaf4c0", GUID = "07a34756-c61a-4edf-adae-a7531da094b5", Cost = 1, Icon = "assets/icons/enhancedmindblade.png", Name = "Keen"},
            new(){Target = "7bda5277d36ad114f9f9fd21d0dab658", GUID = "22e4571e-5f17-4f61-9fb5-a0b95264f464", Cost = 1, Icon = "assets/icons/shockmindblade.png", Name = "Shock"},
            new(){Target = "a1455a289da208144981e4b1ef92cc56", GUID = "320f2baf-db18-4f5f-8eee-a8deb1d09896", Cost = 1, Icon = "assets/icons/enhancedmindblade.png", Name = "Vicious"},
            new(){Target = "57315bc1e1f62a741be0efde688087e9", GUID = "d59be0f4-44f2-429b-ab33-0a0607878147", Cost = 2, Icon = "assets/icons/anarchicmindblade.png", Name = "Anarchic"},
            new(){Target = "0ca43051edefcad4b9b2240aa36dc8d4", GUID = "72aed536-39ec-414b-8545-ab87fd76e051", Cost = 2, Icon = "assets/icons/axiomaticmindblade.png", Name = "Axiomatic"},
            new(){Target = "3f032a3cd54e57649a0cdad0434bf221", GUID = "e14c4bae-454d-4bfd-919c-fa3c64cfe2e9", Cost = 2, Icon = "assets/icons/flamingburstmindblade.png", Name = "Flaming Burst"},
            new(){Target = "28a9964d81fedae44bae3ca45710c140", GUID = "95b93124-82dd-43c4-aecf-65ccfe0ffbeb", Cost = 2, Icon = "assets/icons/holymindblade.png", Name = "Holy"},
            new(){Target = "564a6924b246d254c920a7c44bf2a58b", GUID = "6763936b-5380-4cd6-9e79-5e33a266f0ba", Cost = 2, Icon = "assets/icons/icyburstmindblade.png", Name = "Icy Burst"},
            new(){Target = "914d7ee77fb09d846924ca08bccee0ff", GUID = "399e37ba-8461-4682-9c17-5b6387d349a9", Cost = 2, Icon = "assets/icons/shockingburstmindblade.png", Name = "Shocking Burst"},
            new(){Target = "d05753b8df780fc4bb55b318f06af453", GUID = "e14f7980-054a-47d8-bd93-5e007f758f70", Cost = 2, Icon = "assets/icons/unholymindblade.png", Name = "Unholy"},
            new(){Target = "66e9e299c9002ea4bb65b6f300e43770", GUID = "8ecd69ce-b0e8-4bb7-86ca-bc3ce170727a", Cost = 4, Icon = "assets/icons/enhancedmindblade.png", Name = "Brilliant Energy"},
        };

        public static string[] Enhancement = new[] {
            "d42fc23b92c640846ac137dc26e000d4",
            "eb2faccc4c9487d43b3575d7e77ff3f5",
            "80bb8a737579e35498177e1e3c75899b",
            "783d7d496da6ac44f9511011fc5f1979",
            "bdba267e951851449af552aa9f9e3992"
        };

        public static void Configure()
        {
            List<Blueprint<BlueprintActivatableAbilityReference>> enchantments = new();
            foreach (var ench in enchantmentTable)
            {
                var shortName = ench.Name.Replace(" ", "");
                ench.Instance = ActivatableAbilityConfigurator.New($"EnhanceMindBlade{shortName}", ench.GUID)
                    .SetDisplayName($"EnhanceMindBlade{shortName}.Name".Translate($"Enhance Mind Blade ({ench.Name} +{ench.Cost})"))
                    .SetDescription(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(ench.Target).Get().m_Description.Key)
                    .SetDeactivateImmediately(true)
                    .SetIcon(ench.Icon)
                    .AddComponent(new TemporarilySpendResource()
                    {
                        m_Resource = MindbladeEnhancement.BlueprintInstance,
                        m_MinimumOverhead = 1,
                        m_Cost = ench.Cost
                    })
                    .Configure();
                enchantments.Add(ench.Instance);
                enchantmentByBlueprint[ench.Instance] = ench;
            }
        }
    }
}
