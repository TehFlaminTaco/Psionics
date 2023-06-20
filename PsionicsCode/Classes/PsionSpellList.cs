using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Psionics.Powers.Level1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Classes
{
    public class PsionSpellList
    {
        private static readonly string SpellListName = "PsionSpellList";
        private static readonly string SpellListGUID = "5ff08de2-5929-49b9-b64d-26d5f3651a79";
        public static BlueprintSpellList BlueprintInstance = null;

        public static void Configure()
        {
            BlueprintInstance = SpellListConfigurator.New(SpellListName, SpellListGUID)
                .SetSpellsByLevel(new SpellLevelList[]
                {
                    new SpellLevelList(0)
                    {
                        m_Spells = new List<BlueprintAbilityReference>(){
                            AbilityRefs.Jolt.Reference.Get().ToReference<BlueprintAbilityReference>()
                        }
                    },
                    new SpellLevelList(1)
                    {
                        m_Spells = new List<BlueprintAbilityReference>(){
                            EnergyRay.BlueprintInstance.ToReference<BlueprintAbilityReference>(),

                            AbilityRefs.Snowball.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.Vanish.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.ColorSpray.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.ShockingGraspCast.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.MagicWeapon.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.EarPiercingScream.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.StunningBarrier.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.MageShield.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.CorrosiveTouch.Reference.Get().ToReference<BlueprintAbilityReference>(),
                            AbilityRefs.ExpeditiousRetreat.Reference.Get().ToReference<BlueprintAbilityReference>(),
                        }
                    }
                })
                .Configure();
        }
    }
}
