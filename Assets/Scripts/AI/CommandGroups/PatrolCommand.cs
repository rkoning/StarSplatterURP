using System;
using AI.BehaviourTree;
using UnityEngine;
using System.Collections.Generic;

namespace AI.CommandGroups {
   [System.Serializable]
   public class PatrolCommand : Command {

      public Vector3[] points;

      private int current = 0;

      public float threshold;

      public PatrolCommand(Vector3[] points, float threshold) {
         this.points = points;
         this.threshold = threshold;
      }

      public override void Init(CommandGroup commandGroup) {
         base.Init(commandGroup);
         commandDefinition = new Selector(new Node[] {
            new Sequence(new Node[] {
               new Node(GroupAtLocation),
               new Node(IncrementPoint),
            }),
            new Selector(new Node[] {
               new Node(CommandNodeSet),                 // check if the command has already been set
               new Node(GroupMoveToPosition),            // Send group to new position
            }),
         });
      }

      public State IncrementPoint() {
         this.current++;
         if (current > points.Length - 1) {
            current = 0;
         }
         commandGuid = Guid.NewGuid(); // reset the command guid so the next CommandNodeSet will fail
         return State.Success;
      }

      public State GroupAtLocation() {
         for (int i = 0; i < pilots.Count; i++) {
            if (pilots[i].GetSquaredDistanceTo(points[current]) > threshold * threshold) {
               return State.Failure;
            }
         }
         return State.Success;
      }

      public State GroupMoveToPosition() {
         commandGuid = Guid.NewGuid();
         foreach (var pilot in pilots) {
            pilot.SetTargetPosition(points[current]);
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