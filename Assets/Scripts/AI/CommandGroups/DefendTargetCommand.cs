using UnityEngine;

using AI.BehaviourTree;
using AI.Factions;

namespace AI.CommandGroups {
   public class DefendTargetCommand : FollowTargetCommand {

      private Target currentAttackTarget;

      public DefendTargetCommand(Target target, float range) {
         this.target = target;
         this.range = range;

         commandDefinition = new Selector(new Node[] {
            new Sequence(new Node[] {
               new Inverter(new Node(TargetIsAlive)),          // if the target is not alive, the command has Failed
               new Node(FailCommand),
            }),
            new Sequence(new Node[] {
               new Node(InFollowRange),                        // if we are in follow range, then find nearby targets and attack them
               new Selector(new Node[] {
                  new Sequence(new Node[] {                    // if we already have a target
                     new Node(TargetInZone),                   // check if it is still nearby
                     new Inverter(new Node(CommandNodeSet)),   // if we don't have a command to attack it yet, then set one
                     new Node(GroupAttackTarget),
                  }),
                  new Node(FindTargetInZone)                   // if we don't have a target, then look for nearby targets
               }),
            }),
            new Selector(new Node[] {
               new Node(CommandNodeSet),                       // if we are outside of follow range, then move into follow range
               new Node(MoveIfOutsideRange)
            }),
         });
      }

      public State TargetInZone() {
         if (currentAttackTarget == null || (currentAttackTarget.GetPosition() - target.GetPosition()).sqrMagnitude > range * range) {
            return State.Failure;
         }
         return State.Success;
      }

      public State FindTargetInZone() { 
         currentAttackTarget = commandGroup.majorFaction.GetClosestEnemyInSphere(target.GetPosition(), range);
         if (currentAttackTarget) {
            commandGuid = System.Guid.NewGuid();
            return State.Success;
         }
         return State.Failure;
      }

      public State GroupAttackTarget() {
         commandGuid = System.Guid.NewGuid();
         for (int i = 0; i < pilots.Count; i++) {
            pilots[i].SetTarget(currentAttackTarget);
            pilots[i].SetCommandNode(new Node(() => { return State.Failure; }));

            pilotCommands.Add(new PilotCommand(pilots[i], new Node(() => { return State.Failure; }), commandGuid));
         }
         return State.Success;
      }
   }
}