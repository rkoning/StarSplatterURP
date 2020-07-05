using System.Collections.Generic;
using System;
using UnityEngine;

using AI.BehaviourTree;
using AI.Factions;

namespace AI.CommandGroups {

   public class FollowTargetCommand : Command {
      public Target target;

      public float range;

      protected bool inRange;

      public FollowTargetCommand() {} // neseccary for this class to be inherited from
      public FollowTargetCommand(Target target, float range) {
         this.target = target;
         this.range = range;
         commandDefinition = new Selector(new Node[] {
            new Sequence(new Node[] {
               new Inverter(new Node(TargetIsAlive)),
               new Node(FailCommand),
            }),
            new Node(InFollowRange),
            new Selector(new Node[] {
               new Node(CommandNodeSet),
               new Node(MoveIfOutsideRange)
            }),
         });
      }  

      public State TargetIsAlive() {
         if (target != null) {
            return State.Success;
         }
         return State.Failure;
      }

      public State InFollowRange() {
         bool allInRange = true;
         Vector3 targetPosition = target.GetPosition();
         foreach (var pilot in pilots) {
            if ((targetPosition - pilot.transform.position).sqrMagnitude > range * range) {
               allInRange = false; 
               break;
            }
         }
         if (allInRange) {
            if (inRange) {
               return State.Success;
            }
            inRange = true;
            commandGuid = Guid.NewGuid();
            for (int i = 0; i < pilots.Count; i++) {
               Node node = new Node(() => { return State.Failure; });
               pilots[i].SetCommandNode(node);
               pilotCommands.Add(new PilotCommand(pilots[i], node, commandGuid));
            }
            return State.Success;
         } else {
            if (inRange == true) {
               inRange = false;
               commandGuid = Guid.NewGuid(); // Reset the command Guid if the pilots were previously in range but aren't anymore
            }
            return State.Failure;
         }
      }

      public State MoveIfOutsideRange() {
         commandGuid = Guid.NewGuid();
         foreach (var pilot in pilots) {
            Node node = new Sequence(new Node[] {
               new Node(() => {
                  Vector3 targetPosition = target.GetPosition();
                  Vector3 point = new Ray(targetPosition, pilot.transform.position - targetPosition).GetPoint(range);
                  pilot.SetTargetPosition(point);
                  return State.Success;
               }),
               new Node(pilot.GetRouteTo),
               new Node(pilot.MoveToPosition)
            });
            pilot.SetCommandNode(node);
            pilotCommands.Add(new PilotCommand(pilot, node, commandGuid));
         }
         return State.Success;
      }
   }
}