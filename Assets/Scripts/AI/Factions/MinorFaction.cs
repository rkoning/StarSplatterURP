using System.Collections.Generic;
using UnityEngine;

using AI.CommandGroups;

namespace AI.Factions {
   public class MinorFaction : ComplexTarget
   {
      private Dictionary<Specialization, List<CommandGroup>> commandGroups;
      private Objective currentObjective;

      public Target target; // for debugging purposes
      public Location targetLocation; // for debugging purposes

      private Objective selfDefense;

      protected override void Start() {
         base.Start();
         commandGroups = new Dictionary<Specialization, List<CommandGroup>>();

         ForChildrenDo((target) => {
            var cmdGroup = target.GetComponent<CommandGroup>();
            if (cmdGroup) {
               AddCommandGroup(cmdGroup);
            }
            target.Parent = this;
         });

         OnAttacked += DefendSelf;
      }

      /// <summary>
      /// Adds a new group by it's specialization. If the specialization is not already 
      /// in the MinorFaction, then it is added and the new group is added to it
      /// </summary>
      /// <param name="group">CommandGroup to add</param>
      public void AddCommandGroup(CommandGroup group) {
         if (!commandGroups.ContainsKey(group.specialization)) {
            commandGroups.Add(group.specialization, new List<CommandGroup>());
         }
         commandGroups[group.specialization].Add(group);
         Debug.Log(commandGroups.Values);
      }

      /// <summary>
      /// Returns a list of CommandGroups based on their specialization
      /// </summary>
      /// <param name="spec">Specialization to find groups by</param>
      /// <returns>List of faction groups with the selected specialization</returns>
      public List<CommandGroup> GetGroups(Specialization spec) {
         if (commandGroups.ContainsKey(spec)) {
            return commandGroups[spec];
         }
         return new List<CommandGroup>();
      }

      public void SetObjective(Objective objective) {
         currentObjective = objective;
         currentObjective.OnFailed += FailObjective;
         currentObjective.OnCompleted += CompleteObjective;
         currentObjective.Perform(this);
      }

      public void CompleteObjective() {
         currentObjective = null;
      }

      public void FailObjective() {
         currentObjective = null;
      }

      private void DefendSelf(Target attacker) {
         if (currentObjective == null) {
            SetObjective(new DefendTargetObjective(this));
         }
      }
   }
}