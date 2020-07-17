using UnityEngine;
using AI.BehaviourTree;

namespace AI {
   public class CargoComponent : ShipComponent
   {
      private AICapital ship;

      public GameObject cargoPrefab;

      public Transform cargoParent;
      private CargoConnectorHealth[] cargoComponents;
      private bool hasCargo;
      
      protected override void Start() {
         if (cargoParent) {
            cargoComponents = cargoParent.GetComponentsInChildren<CargoConnectorHealth>();
         } else {
            Debug.LogWarning("Warning: cargoParent not set on: " + gameObject.name);
         }
      }

      public State DropCargo() {
         if (!hasCargo) {
            return State.Failure;
         }
         for (int i = 0; i < cargoComponents.Length; i++) {
            if (cargoComponents[i] != null) {
               cargoComponents[i].DropCargo();
            }
         }
         hasCargo = false;
         return State.Success;
      }

      public State PickUpCargo() {
         if (hasCargo) {
            return State.Failure;
         }
         for (int i = 0; i < cargoComponents.Length; i++) {
            if (cargoComponents[i] != null) {
               cargoComponents[i].SetCargo(cargoPrefab);
            }
         }
         hasCargo = true;
         return State.Success;
      }

      public State HasCargo() {
         if (hasCargo) {
            return State.Success;
         }
         return State.Failure;
      }

      public override void OnHealthDestroyed(Health h) {
         DropCargo();
         base.OnHealthDestroyed(h);
      }
   }
}