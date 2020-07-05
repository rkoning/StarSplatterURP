using AI.CommandGroups;
using UnityEngine;

namespace AI.Factions {
   public class DefendLocationObjective : Objective {

      private MinorFaction faction;
      public Location location;
      public DefendLocationObjective(Location location) {
         this.location = location;
      }
      
      public override bool IsComplete() {
         return false;
      }

      public override void Perform(MinorFaction f) {
         faction = f;
         location.OnTargetEnter += GroupsAttackTarget;

         var offenseGroups = f.GetGroups(Specialization.Offense);
         for (int i = 0; i < offenseGroups.Count; i++) {            
            offenseGroups[i].SetCurrentCommand(new PatrolCommand(location.GetPatrolPoints(), 50f));
         }
      }

      public void GroupsAttackTarget(MajorFaction majorFaction, Target target) {
         if (faction.majorFaction.enemies.Contains(majorFaction)) {
         
            var offenseGroups = faction.GetGroups(Specialization.Offense);
            for (int i = 0; i < offenseGroups.Count; i++) {            
               offenseGroups[i].SetCurrentCommand(new AttackTargetCommand(target));
            }
         }
      }
   }
}