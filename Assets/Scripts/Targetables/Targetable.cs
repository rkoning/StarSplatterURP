using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{

      public delegate bool OnDestroyedAction(Targetable t);
      public event OnDestroyedAction OnDestroyed;

      public delegate void OnAttackedAction(Targetable t);
      public event OnAttackedAction OnAttacked;

    //   public MajorFaction majorFaction;

      public Targetable parent;
      public Targetable Parent {
         set { parent = value; }
      }

      protected virtual void Start()
      {
         OnDestroyed += (Targetable t) => { return true; };
         OnAttacked += (Targetable t) => { return; };
      }

      public virtual Targetable GetTarget() {
         return this;
      }

      public virtual Targetable GetClosestTargetInSphere(Vector3 position, float radius) {
         if ((this.GetPosition() - position).sqrMagnitude > radius * radius) {
            return this;
         }
         return null;
      }

      public virtual Vector3 GetPosition() {
         return transform.position;
      }

      public virtual void AttackedBy(Targetable t) {
         OnAttacked(t);
         if (parent) {
            parent.AttackedBy(t);
         }
      }

      private void OnDestroy() {
         if (this == null)
            return;
         if (parent) {
            ((ComplexTargetable)parent).children.Remove(this);
         }
         OnDestroyed(this);
      }
}
