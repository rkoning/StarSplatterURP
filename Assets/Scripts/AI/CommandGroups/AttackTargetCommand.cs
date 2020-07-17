using System;

using AI.BehaviourTree;
using UnityEngine;

namespace AI.CommandGroups {

   public class AttackTargetCommand : Command {
      public Targetable target;

      public AttackTargetCommand(Targetable target) {
         this.target = target;
         commandDefinition = new Selector(new Node[] {
            new Sequence(new Node[] {
               new Inverter(new Node(TargetIsAlive)),
               new Node(CompleteCommand),
            }),
            new Selector(new Node[] {
               // new Node(CommandNodeSet),
               new Node(GroupAttackTarget)
            })
         });
      }
      
      public State TargetIsAlive() {
         if (target != null) {
            return State.Success;
         }
         return State.Failure;
      }

      public State GroupAttackTarget() {
         commandGuid = Guid.NewGuid();
         for (int i = 0; i < ships.Count; i++) {
            ships[i].SetTarget(target);
            ships[i].SetCommandNode(new Node(() => { return State.Failure; }));

            pilotCommands.Add(new PilotCommand(ships[i], new Node(() => { return State.Failure; }), commandGuid));
         }
         return State.Success;
      }
   }
}