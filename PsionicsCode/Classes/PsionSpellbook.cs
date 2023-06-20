using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Tutorial;
using Psionics.Abilities.Soulknife.Bladeskills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Classes
{
    public class PsionSpellbook
    {
        private static readonly string SpellbookName = "PsionSpellbook";
        private static readonly string SpellbookGUID = "8153950b-f84e-4660-b595-1d086ef45816";
        public static BlueprintSpellbook BlueprintInstance = null;

        private static readonly string SpellsPerDayName = "PsionSpellsPerDay";
        private static readonly string SpellsPerDayGUID = "23717c91-41af-482f-b283-ecddced2ebe8";
        public static BlueprintSpellsTable SpellsPerDay = null;

        public static void ConfigureSpellsPerDay()
        {
            int BIG = 1;
            SpellsPerDay = SpellsTableConfigurator.New(SpellsPerDayName, SpellsPerDayGUID)
                .SetLevels(
                    /*00*/new SpellsLevelEntry(){Count = new int[] {0,BIG}},
                    /*01*/new SpellsLevelEntry(){Count = new int[] {0,BIG}},
                    /*02*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG}},
                    /*03*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG}},
                    /*04*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG}},
                    /*05*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG}},
                    /*06*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG}},
                    /*07*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG}},
                    /*08*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG}},
                    /*09*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*10*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*11*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*12*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*13*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*14*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*15*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*16*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*17*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*18*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*19*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*20*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*21*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*22*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*23*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*24*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*25*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*26*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*27*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*28*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*29*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}},
                    /*30*/new SpellsLevelEntry(){Count = new int[] {0,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG,BIG}}
                )
                .Configure();
        }

        public static void Configure()
        {
            ConfigureSpellsPerDay();
            BlueprintInstance = SpellbookConfigurator.New(SpellbookName, SpellbookGUID)
                .OnConfigure(bp=>bp.m_CharacterClass = Psion.ClassBlueprint.ToReference<BlueprintCharacterClassReference>())
                .SetSpellList(PsionSpellList.BlueprintInstance)
                .SetSpellsPerLevel(2)
                .SetCantripsType(CantripsType.Cantrips)
                .SetSpellsPerDay(SpellsPerDay)
                .SetSpontaneous(true)
                .SetIsArcane(false)
                .SetSpontaneous(true)
                .SetCastingAttribute(Kingmaker.EntitySystem.Stats.StatType.Intelligence)
                .Configure(true);
            
        }

    }
}
