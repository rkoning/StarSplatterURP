using UnityEngine;

namespace AI.Factions {
   public class Target : MonoBehaviour {
   
      public delegate bool OnDestroyedAction(Target t);
      public event OnDestroyedAction OnDestroyed;

      public delegate void OnAttackedAction(Target t);
      public event OnAttackedAction OnAttacked;

      public MajorFaction majorFaction;

      public Target parent;
      public Target Parent {
         set { parent = value; }
      }

      protected virtual void Start()
      {
         OnDestroyed += (Target t) => { return true; };
         OnAttacked += (Target t) => { return; };
      }

      public virtual Target GetTarget() {
         return this;
      }

      public virtual Target GetClosestTargetInSphere(Vector3 position, float radius) {
         if ((this.GetPosition() - position).sqrMagnitude > radius * radius) {
            return this;
         }
         return null;
      }

      public virtual Vector3 GetPosition() {
         return transform.position;
      }

      public virtual void AttackedBy(Target t) {
         OnAttacked(t);
         if (parent) {
            parent.AttackedBy(t);
         }
      }

      private void OnDestroy() {
         if (parent) {
            ((ComplexTarget)parent).children.Remove(this);
         }
         OnDestroyed(this);
      }
   }
}