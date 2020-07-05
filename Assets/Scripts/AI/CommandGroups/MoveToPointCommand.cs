using System;
using System.Collections.Generic;
using UnityEngine;

using AI.BehaviourTree;

namespace AI.CommandGroups {
   [System.Serializable]
   public class MoveToPointCommand : Command {

      public Vector3 point;

      public float threshold;

      public MoveToPointCommand(Vector3 point, float threshold) {
         this.point = point;
         this.threshold = threshold;
      }

      public override void Init(CommandGroup commandGroup) {
         base.Init(commandGroup);
         commandDefinition = new Selector(new Node[] {
            new Sequence(new Node[] {
               new Node(GroupAtLocation),                // check if the group is already at the position
               new Node(CompleteCommand),                // if so the command has been completed
            }),
            new Selector(new Node[] {
               new Node(CommandNodeSet),                 // check if the command has already been set
               new Node(GroupMoveToPosition),            // if not, set the command to move to a position      
            }),
         });
      }

      public State GroupAtLocation() {
         for (int i = 0; i < pilots.Count; i++) {
            if (pilots[i].GetSquaredDistanceTo(point) > threshold * threshold) {
               return State.Failure;
            }
         }
         return State.Success;
      }

      public State GroupMoveToPosition() {
         commandGuid = Guid.NewGuid();
         foreach (var pilot in pilots) {
            pilot.SetTargetPosition(point);
            Node node = new Sequence(new Node[] {
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