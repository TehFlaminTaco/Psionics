using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Psionics.Buffs;
using Psionics.Feats;
using Psionics.Feats.Psion;
using Psionics.Feats.Soulknife;
using Psionics.Feats.Soulknife.BladeSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Classes
{
    public class PsionProgression
    {
        private static readonly string ProgressionName = "PsionProgression";
        private static readonly string ProgressionGUID = "f25a2131-1211-4cc4-9dd5-3bb1a3975b2a";
        public static BlueprintProgression ProgressionBlueprint = null;

        public static void Configure()
        {
            ProgressionBlueprint = ProgressionConfigurator.New(ProgressionName, ProgressionGUID)
               .SetLevelEntry(01, PsionProficiencies.BlueprintInstance, PowerPointPool.BlueprintInstance, PsionPowerPoints.BlueprintInstance)
               .Configure();
        }
    }
}
