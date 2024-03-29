﻿using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Psionics.Buffs;
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
                        m_Spells = new []{
                            Bolt.BlueprintInstance,
                            Broker.BlueprintInstance,
                            CircumstanceShield.BlueprintInstance,
                            CrystalShard.BlueprintInstance,
                            Deceleration.BlueprintInstance,
                            Demoralize.BlueprintInstance,
                            DissipatingTouch.BlueprintInstance,
                            EnergyRay.BlueprintInstance,
                            EctoplasmicSheen.BlueprintInstance
                        }.Select(c=>c.ToReference<BlueprintAbilityReference>()).ToList()
                    }
                })
                .Configure();
        }
    }
}
