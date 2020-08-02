using System.Linq;
using UnityEngine;
using AI.BehaviourTree;
using AI.CommandGroups;
using System.Collections.Generic;

   namespace AI {
      public class TurretSystemComponent : ShipComponent
      {
         
         public Transform turretParent;

         private Weapon turretWeapon;
         
         protected TurretProjector[] turrets = new TurretProjector[0];

         public float range;

         protected override void Start()
         {
            base.Start();
            if (turretParent != null) {
              SetTurretParent(turretParent);
            }
         }

         public void SetTurretParent(Transform turretParent) {
            this.turretParent = turretParent;
            turrets = turretParent.GetComponentsInChildren<TurretProjector>();

            turretWeapon = turretParent.GetComponent<Weapon>();
            turretWeapon.projectors = new List<Projector>();

            for (int i = 0; i < turrets.Length; i++) {
               var turret = turrets[i].GetComponent<TurretProjector>();
               turretWeapon.projectors.Add(turret);
               turret.Setup(
                  turretWeapon, 
                  turretWeapon.damage, 
                  turretWeapon.HitDamage, 
                  turretWeapon.HitAny
               );
            }

            Node offenseNode = new Sequence(new Node[] {
               new Node(IsActive),
               new Node(TargetInRange),
               new Node(TurretsAttackOther)
            });
            if (ship == null) {
               ship = GetComponent<AIShip>();
            }
            ship.AppendOffenseNode(offenseNode);
            Node defenseNode = new Sequence(new Node[] {
               new Node(IsActive),
               // new Node(TurretsDefendSelf)
               new Node(TurretsAttackOther)
            });

            ship.AppendDefenseNode(defenseNode);
         }

         public State TurretsAttackOther() {
            if (ship.target != null) {
               turretWeapon.target = ship.target;
               turretWeapon.Fire();
               return State.Success;
            }
            return State.Failure;
         }

         private State TargetInRange() {
            if (ship.target != null && ship.GetSquaredDistanceTo(ship.target.transform.position) <= range * range) {
               return State.Success;
            }
            return State.Failure;
         }
      }
   }