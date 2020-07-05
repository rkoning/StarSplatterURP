using System;

using AI.BehaviourTree;
using AI.Factions;

namespace AI.CommandGroups {

   public class AttackTargetCommand : Command {
      public Target target;

      public AttackTargetCommand(Target target) {
         this.target = target;
         commandDefinition = new Selector(new Node[] {
            new Sequence(new Node[] {
               new Inverter(new Node(TargetIsAlive)),
               new Node(CompleteCommand),
            }),
            new Selector(new Node[] {
               new Node(CommandNodeSet),
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
         UnityEngine.Debug.Log("Sending Attack command");
         for (int i = 0; i < pilots.Count; i++) {
            pilots[i].SetTarget(target);
            pilots[i].SetCommandNode(new Node(() => { return State.Failure; }));

            pilotCommands.Add(new PilotCommand(pilots[i], new Node(() => { return State.Failure; }), commandGuid));
         }
         return State.Success;
      }
   }
}