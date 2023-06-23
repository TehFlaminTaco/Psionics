using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Psionics.Powers.Level1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psionics.Buffs
{
    public class EctoplasmicSheenBuff
    {
        private static readonly string BuffName = "EctoplasmicSheenBuff";
        private static readonly string BuffGUID = "86786497-bf1b-427f-a3c6-0336c7b0df61";
        public static BlueprintBuff BlueprintInstance = null;

        private static readonly string DisplayName = "EctoplasmicSheenBuff.Name".Translate("Ectoplasmic Sheen");
        private static readonly string Description = "EctoplasmicSheenBuff.Description".Translate("You draw forth ectoplasm in an area, causing the surface to become slick. Any creature in the area when the power is manifested must make a successful Reflex save or fall. A creature can walk within or through the area of ectoplasm at half normal speed with a DC 10 Acrobatics check. Failure means it can’t move that round (and must then make a Reflex save or fall), while failure by 5 or more means it falls (see the Acrobatics skill for details). Creatures that do not move on their turn do not need to make this check and are not considered flat-footed.");
        private static readonly string Icon = "assets/icons/ectoplasmicsheen.png";

        public static void Configure()
        {
            BlueprintInstance = BuffConfigurator.New(BuffName, BuffGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddSpellDescriptorComponent(SpellDescriptor.MovementImpairing | SpellDescriptor.Ground)
                .AddFactContextActions(ActionsBuilder.New()
                    .Conditional(
                        ConditionsBuilder.New()
                            .Add<ContextConditionHasBuff>(cchb => {
                                cchb.m_Buff = BuffRefs.MountedBuff.Reference.Get().ToReference<BlueprintBuffReference>();
                                cchb.Not = false;
                            }),
                        ActionsBuilder.New(),
                        ActionsBuilder.New()
                            .Add<ContextActionSavingThrowBonus>(castb =>
                            {
                                castb.Type = SavingThrowType.Reflex;
                                castb.FromBuff = false;
                                castb.CustomDC = null;
                                castb.HasCustomDC = false;
                                castb.m_ConditionalDCIncrease = new ContextActionSavingThrowBonus.ConditionalDCIncrease[0];
                                castb.UseDCFromContextSavingThrow = false;
                                castb.Bonus = ContextValues.Rank(AbilityRankType.Default);
                                castb.Actions = ActionsBuilder.New()
                                                    .ApplyBuff(BuffRefs.Prone.Reference.Get(), ContextDuration.Fixed(1))
                                                    .Build();
                            })
                    ), null, ActionsBuilder.New()
                    .Conditional(
                        ConditionsBuilder.New()
                            .Add<ContextConditionHasBuff>(cchb => {
                                cchb.m_Buff = BuffRefs.MountedBuff.Reference.Get().ToReference<BlueprintBuffReference>();
                                cchb.Not = false;
                            }),
                        ActionsBuilder.New(),
                        ActionsBuilder.New()
                            .Add<ContextActionSavingThrowBonus>(castb =>
                            {
                                castb.Type = SavingThrowType.Reflex;
                                castb.FromBuff = false;
                                castb.CustomDC = null;
                                castb.HasCustomDC = false;
                                castb.m_ConditionalDCIncrease = new ContextActionSavingThrowBonus.ConditionalDCIncrease[0];
                                castb.UseDCFromContextSavingThrow = false;
                                castb.Bonus = ContextValues.Rank(AbilityRankType.Default);
                                castb.Actions = ActionsBuilder.New()
                                                    .ApplyBuff(BuffRefs.Prone.Reference.Get(), ContextDuration.Fixed(1))
                                                    .Build();
                            })
                    ))
                .AddContextRankConfig(ContextRankConfigs.BuffRank(BuffGUID, false, AbilityRankType.Default))
                .SetIcon(Icon)
                .Configure();
        }
    }
}
