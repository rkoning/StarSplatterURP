   using UnityEngine;
   using AI.BehaviourTree;
   using AI.CommandGroups;

   namespace AI {
      public class TurretSystemComponent : ShipComponent
      {
         
         public Transform turretParent;

         protected AITurret[] turrets = new AITurret[0];

         private CommandGroup turretCommandGroup;

         public float range;

         public float disableTime = 30f;
         private float disableEnd = -1f;
         private bool disabling = false;

         protected override void Start()
         {
            base.Start();

            if (turretParent != null) {
              SetTurretParent(turretParent);
            }
         }

         public void SetTurretParent(Transform turretParent) {
            this.turretParent = turretParent;
            turretCommandGroup = turretParent.GetComponent<CommandGroup>();
            turrets = turretParent.GetComponentsInChildren<AITurret>();
            for (int i = 0; i < turrets.Length; i++) {
               // turrets[i].Faction = ship..Faction;
               var ship = turrets[i].GetComponent<AIShip>();
               turretCommandGroup.ships.Add(ship);
            }

            Node offenseNode = new Sequence(new Node[] {
               // new Node(IsActive),
               // new Node(TargetInRange),
               new Node(TurretsAttackOther)
            });

            ship.AppendOffenseNode(offenseNode);
            Node defenseNode = new Sequence(new Node[] {
               new Node(IsActive),
               // new Node(TurretsDefendSelf)
               new Node(TurretsAttackOther)
            });

            ship.AppendDefenseNode(defenseNode);
         }

         private void Update() {
            // if (disabling && disableEnd < Time.fixedTime) {
            //    for (int i = 0; i < turrets.Length; i++) {
            //       turrets[i].SetAlive(true);
            //    }
            //    disabling = false;
            // }
         }

         public override void OnHealthDestroyed(Health compHealth) {
            // base.OnHealthDestroyed(compHealth);
            // for (int i = 0; i < turrets.Length; i++) {
            //    turrets[i].Pilot.SetAlive(false);
            // }
            // disableEnd = disableTime + Time.fixedTime;
            // disabling = true;
         }

         public State TurretsAttackOther() {
            if (ship.target != null) {
               turretCommandGroup.SetCurrentCommand(new AttackTargetCommand(ship.target));
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