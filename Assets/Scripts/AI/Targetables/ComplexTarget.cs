using System;
using System.Collections.Generic;

using UnityEngine;
namespace AI.Factions
{
   public class ComplexTarget : Target {
      public List<Target> children;
      public Transform childTargetTransform;
      protected override void Start() {
         base.Start();
         ForChildrenDo((target) => {
            target.OnDestroyed += children.Remove;
            target.Parent = this;
         });
      }

      public void ForChildrenDo(Action<Target> action) {
         if (childTargetTransform == null) {
            childTargetTransform = transform;
         }
         children = new List<Target>(childTargetTransform.GetComponentsInChildren<Target>());
         int selfIndex = -1;
         for (int i = 0; i < children.Count; i++) {
            if (children[i] == this) {
               selfIndex = i;
               continue;
            }
            action(children[i]);
         }

         children.RemoveAt(selfIndex);
      }

      public override Target GetTarget() {
         if (children.Count == 0) {
            return null;
         }
         return children[UnityEngine.Random.Range(0, children.Count)].GetTarget();
      }

      public override Target GetClosestTargetInSphere(Vector3 position, float radius) {
         Target closest = null;
         float closestDist = Mathf.Infinity;
         for (int i = 0; i < children.Count; i++) {
            if (children[i] == null) {
               continue;
            }
            float squaredDist = (children[i].GetPosition() - position).sqrMagnitude;
            if (squaredDist < radius * radius && squaredDist < closestDist)
               closest = children[i];
         }
         return closest;
      }

      public override Vector3 GetPosition() {
         Vector3 centroid = Vector3.zero;
         for (int i = 0; i < children.Count; i++) {
            if (children[i] == this || children[i] == null)
               continue;
            centroid += children[i].GetPosition();
         }
         centroid = centroid / children.Count;
         return centroid;
      }
   }
}