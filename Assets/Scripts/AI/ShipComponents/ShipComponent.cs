using UnityEngine;
using System.Collections.Generic;
using AI.BehaviourTree;

namespace AI {
   public abstract class ShipComponent : MonoBehaviour {

      public CompositeHealth clusterHealth;

      public bool destroyed;

      protected AIShip ship;
      protected virtual void Start() {
         ship = GetComponent<AIShip>();

         if (clusterHealth) {
            clusterHealth.OnDeath += OnHealthDestroyed;
            clusterHealth.OnDamaged += () => {};
            clusterHealth.SetupComponents(
               new List<ComponentHealth>(clusterHealth.transform.GetComponentsInChildren<ComponentHealth>())
            );
         } else {
            Debug.LogWarning("Warning: clusterHealth for " + name + "#" + this.GetType() + " has not been set...");
         }
      }

      public virtual void OnHealthDestroyed(Health compHealth) {
         destroyed = true;
      }
      
      public State IsActive() {
         return destroyed ? State.Failure : State.Success;
      }
   }
}