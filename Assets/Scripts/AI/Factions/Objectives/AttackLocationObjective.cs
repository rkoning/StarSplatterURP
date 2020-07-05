using AI.CommandGroups;

namespace AI.Factions {
   public class AttackLocationObjective : Objective {
      public Location location;

      public AttackLocationObjective(Location location) {
         this.location = location;
      }

      public override bool IsComplete() {
         // if (location.owner == null) {
         //    Complete();
         //    return true;
         // }
         return false;
      }

      public override void Complete() {

      }

      public override void Fail() {

      }

      public override void Perform(MinorFaction f) {
         var offenseGroups = f.GetGroups(Specialization.Offense);
         for (int i = 0; i < offenseGroups.Count; i++) {
            // offenseGroups[i].SetCurrentCommand(new AttackTargetCommand(location.GetClosestLocalTarget(location.owner,  )));
         }

         var defenseGroups = f.GetGroups(Specialization.Defense);
         for (int i = 0; i < defenseGroups.Count; i++) {
            // defenseGroups[i].commandGroup.SetCurrentCommand(new DefendTargetCommand())
         }
      }
   }
}