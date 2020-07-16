   // using UnityEngine;
   // using AI.BehaviourTree;
   // using AI.CommandGroups;

   // namespace AI {
   //    public class TurretSystemComponent : CapitalComponent
   //    {
         
   //       public Transform turretParent;

   //       protected AITurret[] turrets = new AITurret[0];

   //       private CommandGroup turretCommandGroup;

   //       private float range;

   //       public float disableTime = 30f;
   //       private float disableEnd = -1f;
   //       private bool disabling = false;

   //       protected override void Start()
   //       {
   //          base.Start();

   //          if (turretParent != null) {
   //             turretCommandGroup = turretParent.GetComponent<CommandGroup>();
   //             turrets = GetComponentsInChildren<AITurret>();
   //             for (int i = 0; i < turrets.Length; i++) {
   //                // turrets[i].Faction = shipPilot..Faction;
   //                var pilot = turrets[i].GetComponent<AIPilot>();
                  
   //                turretCommandGroup.pilots.Add(pilot);
   //                // range = ((AITurret) pilot).attackRange;
   //             }
   //          } else {
   //             Debug.LogWarning("TurretParent of AICapital: " + name + " has not been assigned!");
   //          }

   //          Node offenseNode = new Sequence(new Node[] {
   //             new Node(IsActive),
   //             new Node(TargetInRange),
   //             new Node(TurretsAttackOther)
   //          });

   //          shipPilot.SetOffenseNode(offenseNode);
   //          Node defenseNode = new Sequence(new Node[] {
   //             new Node(IsActive),
   //             // new Node(TurretsDefendSelf)
   //             new Node(TurretsAttackOther)
   //          });

   //          shipPilot.SetDefenseNode(defenseNode);
   //       }

   //       private void Update() {
   //          // if (disabling && disableEnd < Time.fixedTime) {
   //          //    for (int i = 0; i < turrets.Length; i++) {
   //          //       turrets[i].SetAlive(true);
   //          //    }
   //          //    disabling = false;
   //          // }
   //       }

   //       public override void OnHealthDestroyed(Health compHealth) {
   //          // base.OnHealthDestroyed(compHealth);
   //          // for (int i = 0; i < turrets.Length; i++) {
   //          //    turrets[i].Pilot.SetAlive(false);
   //          // }
   //          // disableEnd = disableTime + Time.fixedTime;
   //          // disabling = true;
   //       }

   //       public State TurretsAttackOther() {
   //          if (shipPilot.target != null) {
   //             turretCommandGroup.SetCurrentCommand(new AttackTargetCommand(shipPilot.target));
   //             return State.Success;
   //          }
   //          return State.Failure;
   //       }

   //       private State TargetInRange() {

   //          if (shipPilot.target != null && shipPilot.GetSquaredDistanceTo(shipPilot.target.transform.position) <= range * range) {
   //             return State.Success;
   //          }
   //          return State.Failure;
   //       }
   //    }
   // }