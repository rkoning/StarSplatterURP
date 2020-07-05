using System;
using System.Collections.Generic;
using UnityEngine;

using AI.BehaviourTree;
using AI;

namespace AI.CommandGroups {
   public class DeliverCargoCommand : Command {
      public Vector3 pickUpPoint;
      public Vector3 dropOffPoint;
      public float threshold;

      public DeliverCargoCommand(Vector3 pickUpPoint, Vector3 dropOffPoint, float threshold) {
         this.pickUpPoint = pickUpPoint;
         this.dropOffPoint = dropOffPoint;
         this.threshold = threshold;
      }

      public override void Init(CommandGroup commandGroup) {
         base.Init(commandGroup);
         commandDefinition = new Selector(new Node[] {
            new Sequence(new Node[] {
               new Inverter(new Node(PilotsHaveCargo)),
               new Selector(new Node[] {
                  new Node(CommandNodeSet),
                  new Node(PickUpCargo)
               })
            }),  // move to pick up location and pick up cargo
            new Sequence(new Node[] {
               new Node(PilotsHaveCargo),
               new Selector(new Node[] {
                  new Node(CommandNodeSet),
                  new Node(DropOffCargo)
               })
            }),  // move to destination and drop cargo
            new Node(CompleteCommand)     // complete command
         });
      }

      public State PilotsHaveCargo() {
         foreach (var pilot in pilots) {
            var cargo = pilot.GetComponent<CargoComponent>();
            if (cargo && cargo.HasCargo() == State.Failure) {
               return State.Failure;
            }
         }
         return State.Success;
      }
      
      public State PickUpCargo() {
         commandGuid = Guid.NewGuid();
         foreach (var pilot in pilots) {
            var cargo = pilot.GetComponent<CargoComponent>();

            pilot.SetTargetPosition(pickUpPoint);
            Node node = new Sequence(new Node[] {
               new Node(pilot.GetRouteTo),
               new Node(pilot.MoveToPosition),
               new Node(cargo.PickUpCargo),
               new Node(() => { commandGuid = Guid.NewGuid(); return State.Success; })
            });

            pilot.SetCommandNode(node);
            pilotCommands.Add(new PilotCommand(pilot, node, commandGuid));
         }
         return State.Success;
      }

      public State DropOffCargo() {
         commandGuid = Guid.NewGuid();
         foreach (var pilot in pilots) {
            var cargo = pilot.GetComponent<CargoComponent>();

            pilot.SetTargetPosition(dropOffPoint);
            Node node = new Sequence(new Node[] {
               new Node(pilot.GetRouteTo),
               new Node(pilot.MoveToPosition),
               new Node(cargo.DropCargo),
               new Node(CompleteCommand),
            });

            pilot.SetCommandNode(node);
            pilotCommands.Add(new PilotCommand(pilot, node, commandGuid));
         }
         return State.Success;
      }
   }
}