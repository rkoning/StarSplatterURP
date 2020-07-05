using System.Collections.Generic;
using UnityEngine;

namespace AI.Factions {
   public class MajorFaction : ComplexTarget {
      public List<Target> enemies = new List<Target>();

      protected override void Start() {
         base.Start();
         this.majorFaction = this;
         ForChildrenDo((target) => {
            target.majorFaction = this;
            target.Parent = this;
         });
         OnAttacked += AddEnemy;
      }

      public Target GetClosestEnemyInSphere(Vector3 position, float radius) {
         Target closest = null;
         float closestDist = Mathf.Infinity;
         for (int i = 0;  i < enemies.Count; i++) {
            Target enemy = enemies[i].GetClosestTargetInSphere(position, radius);
            if (enemy == null)
               continue;

            float squaredDist = (enemy.GetPosition() - position).sqrMagnitude;
            if (squaredDist < radius * radius && squaredDist < closestDist) {
               closest = enemy;
               closestDist = squaredDist;
            }
         }
         return closest;
      }

      /// <summary>
      /// Adds a target to the enemies list of this faction if the target is not already an enemy
      /// </summary>
      /// <param name="target">Target to add to the enemies list</param>
      public void AddEnemy(Target target) {
         // TODO: Refactor enemies as a Dictionary instead
         if (enemies.IndexOf(target.majorFaction) < 0) {
            enemies.Add(target.majorFaction);
         }
      }
   }
}