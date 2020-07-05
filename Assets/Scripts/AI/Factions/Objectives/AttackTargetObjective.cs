using AI.CommandGroups;

namespace AI.Factions {
   public class AttackTargetObjective : Objective {
      public Target target;

      public AttackTargetObjective(Target t) {
         this.target = t;
      }

      public override bool IsComplete() {
         if (target == null) {
            Complete();
            return true;
         }
         return false;
      }

      public override void Complete() {

      }

      public override void Fail() {

      }

      public override void Perform(MinorFaction f) {
         var offenseGroups = f.GetGroups(Specialization.Offense);
         for (int i = 0; i < offenseGroups.Count; i++) {
            offenseGroups[i].SetCurrentCommand(new AttackTargetCommand(target));
         }

         var defenseGroups = f.GetGroups(Specialization.Defense);
         for (int i = 0; i < defenseGroups.Count; i++) {
            // defenseGroups[i].commandGroup.SetCurrentCommand(new DefendTargetCommand())
         }
      }
   }
}