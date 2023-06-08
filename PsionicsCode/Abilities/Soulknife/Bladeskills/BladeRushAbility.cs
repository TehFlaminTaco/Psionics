using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.KingdomEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using JetBrains.Annotations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using Psionics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.Blueprints.Root;
using Kingmaker.Pathfinding;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic;
using System.Collections;
using System.Net;
using TurnBased.Controllers;
using Pathfinding;
using Kingmaker.View;
using HarmonyLib;
using Kingmaker.View.Equipment;
using Psionics.Equipment;
using Psionics.Feats.Soulknife;
using BlueprintCore.Utils.Types;
using static Kingmaker.UI.CanvasScalerWorkaround;
using DungeonArchitect;
using Kingmaker.Visual.Particles;
using Kingmaker.ResourceLinks;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;

namespace Psionics.Abilities.Soulknife.Bladeskills
{
    public class AbilityCustomBladeRush : AbilityCustomLogic
    {
        public override void Cleanup(AbilityExecutionContext context)
        {
            context.Caster.View.AgentASP.MaxSpeedOverride = null;
        }

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper targetWrapper)
        {
            Vector3 target = targetWrapper.Point;
            if (target == null)
            {
                PFLog.Default.Error("Target point is missing");
                yield break;
            }

            UnitEntityData caster = context.Caster;
            PsionicFocus.Spend(caster);

            Vector3 position = caster.Position;
            Vector3 endPoint = target;

            var prefMoved = caster.CombatState.TBM.TimeMoved;

            caster.View.StopMoving();
            if (!CombatController.IsInTurnBasedCombat())
            {
                caster.View.AgentASP.FindPath(endPoint, (path) =>
                {
                    path.BlockUntilCalculated();
                    var targetPosition = path.vectorPath.Last();
                    caster.CombatState.PreventAttacksOfOpporunityNextFrame = true;
                    caster.Position = targetPosition;
                }, 1f);
                yield break;
            }
            //caster.View.AgentASP.ForcePath((Path)(object)new ForcedPath(new List<Vector3> { position, endPoint }), disableApproachRadius: true);
            caster.AddBuff(BladeRushBuff.BlueprintInstance, caster, TimeSpan.FromSeconds(3), context.Params);
            caster.View.AgentASP.PathTo(null, endPoint, 1f, 4f);
            IEnumerator turnBasedRoutine = null;
            while (true)
            {
                IEnumerator obj = turnBasedRoutine ?? TurnBasesRoutine(caster, endPoint);
                IEnumerator enumerator = obj;
                turnBasedRoutine = obj;
                var enumerator2 = enumerator;

                if (!enumerator2.MoveNext())
                {
                    break;
                }

                yield return null;
            }
            caster.SetBuffDuration(BladeRushBuff.BlueprintInstance, 0f);
            if (caster.IsCurrentUnit())
                Game.Instance.TurnBasedCombatController.CurrentTurn.m_RiderMovementStats.TimeMoved = prefMoved;
        }


        public static IEnumerator TurnBasesRoutine(UnitEntityData caster, Vector3 endPoint)
        {
            UnitEntityData mount = caster.GetSaddledUnit();
            if (mount == null)
            {
                UnitMovementAgent agentASP = caster.View.AgentASP;
                float timeSinceStart = 0f;
                while (agentASP != null && agentASP.EstimatedTimeLeft > 0)
                {
                    if ((bool)Game.Instance.TurnBasedCombatController.WaitingForUI)
                    {
                        yield return null;
                        continue;
                    }

                    timeSinceStart += Game.Instance.TimeController.GameDeltaTime;
                    if (timeSinceStart > 6f)
                    {
                        Main.Logger.Info("Charge: timeSinceStart > 6f");
                        break;
                    }

                    if (!caster.Descriptor.State.CanMove)
                    {
                        Main.Logger.Info("Charge: !caster.Descriptor.State.CanMove");
                        break;
                    }

                    if (!agentASP)
                    {
                        Main.Logger.Info("Charge: !(bool)caster.View.AgentASP");
                        break;
                    }

                    if (!agentASP.IsReallyMoving)
                    {
                        agentASP.ForcePath((Path)(object)new ForcedPath(new List<Vector3> { caster.Position, endPoint }), disableApproachRadius: true);
                        if (!agentASP.IsReallyMoving)
                        {
                            Main.Logger.Info("Charge: !caster.View.AgentASP.IsReallyMoving");
                            break;
                        }
                    }

                    agentASP.MaxSpeedOverride = Math.Max(agentASP.MaxSpeedOverride.GetValueOrDefault(), caster.CombatSpeedMps * 3f);
                    yield return null;
                }
            }
            else
            {
                yield break;
            }

            caster.View.StopMoving();
        }

        public static IEnumerator RuntimeRoutine(UnitEntityData caster, Vector3 endPoint)
        {
            float maxDistance = caster.CombatSpeedMps * 3f;
            UnitEntityData mount = caster.GetSaddledUnit();
            if (mount == null)
            {
                if (!caster.View.MovementAgent.IsReallyMoving)
                {
                    Main.Logger.Info("Charge: Failed initial movement check!");
                }
                float passedDistance = 0f;
                int loops = 0;
                while (caster.View.MovementAgent.IsReallyMoving)
                {
                    loops++;
                    float valueOrDefault = caster.View.MovementAgent.MaxSpeedOverride.GetValueOrDefault();
                    caster.View.MovementAgent.MaxSpeedOverride = Math.Max(valueOrDefault, caster.CombatSpeedMps);
                    passedDistance += (caster.Position - caster.PreviousPosition).magnitude;
                    UnitMovementAgent agentASP = caster.View.AgentASP;
                    if (passedDistance > maxDistance || (endPoint - caster.Position).magnitude < 0.5f)
                    {
                        Main.Logger.Info("Charge: passedDistance > maxDistance || !attack.ShouldUnitApproach");
                        break;
                    }

                    Vector3 position = endPoint;
                    if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, position) != position)
                    {
                        Main.Logger.Info("Charge: obstacle != newEndPoint");
                        break;
                    }

                    yield return null;
                }

                if (!caster.View.MovementAgent.IsReallyMoving)
                {
                    Main.Logger.Info("Charge: !caster.View.MovementAgent.IsReallyMoving");
                }
                Main.Logger.Info($"Charge: Finished uneventfully! Looped {loops} times and moved {passedDistance} meters!");
            }
            else
            {
                yield break;
            }
        }
    }

    public class BladeRushAbility
    {
        public static BlueprintAbility BlueprintInstance = null;
        private static readonly string AbilityName = "BladeRushAbility";
        private static readonly string AbilityGUID = "80121bed-2f0f-4b69-a594-4341e32e7108";


        [Translate("Blade Rush")]
        private static readonly string DisplayName = "BladeRushAbility.Name";
        [Translate("The soulknife rushes forward with a dash of incredible speed. As a Swift action, the soulknife may expend her psionic focus and move up to her speed without provoking attacks of opportunity. The soulknife must be at least 6th level in order to select this blade skill.", true)]
        private static readonly string Description = "BladeRushAbility.Description";
        private static readonly string Icon = "assets/icons/bladerush.png";

        public static void Configure()
        {
            BlueprintInstance = AbilityConfigurator.New(AbilityName, AbilityGUID)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .RequirePsionicFocus()
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetCanTargetSelf(false)
                .SetCanTargetPoint(true)
                .SetRange(AbilityRange.Custom)
                .AddComponent(new AbilityCustomBladeRush())
                .SetIcon(Icon)
                .Configure();
        }
    }

    [HarmonyPatch(typeof(BlueprintAbility), nameof(BlueprintAbility.GetRange))]
    static class BlueprintAbility_GetRange_BladeRush_Patch
    {
        static bool Prefix(UnitViewHandsEquipment __instance, ref Feet __result, ref bool reach, ref AbilityData abilityData)
        {
            if (abilityData.Blueprint == BladeRushAbility.BlueprintInstance)
            {
                __result = BlueprintAbility.GetDoubleMoveRange(abilityData) / 2;
                return false;
            }

            return true;
        }
    }
}
