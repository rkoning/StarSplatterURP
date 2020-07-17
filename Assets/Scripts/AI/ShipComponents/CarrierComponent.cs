// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using BehaviourTree;

// public class CarrierComponent : ShipComponent
// {

//     public GameObject fighterPrefab;
//     private FighterShip[][] fighters = new FighterShip[0][];

//     public Transform dockParent;
//     private Transform[] dockLocations;

//     public Vector3 attackRotation;

//     private bool allFightersLaunched = false;
//     public float launchDelay = 5f;
//     public float launchForce = 15000f;
//     private float nextLaunch;
//     private int currentGroup = 0;

//     private CommandGroup[] squadrons;

//     public float protectDuration = 30f;
//     private float protectEnd;

//     protected override void Start()
//     {
//         base.Start();
//         var faction = shipPilot.Health.Faction;
//         if (dockParent != null) {
//             int i = 0;
//             squadrons = new CommandGroup[dockParent.childCount];
//             fighters = new FighterShip[dockParent.childCount][];

//             foreach(Transform group in dockParent) {
//                 int j = 0;
//                 fighters[i] = new FighterShip[group.childCount];
//                 squadrons[i] = new CommandGroup();
//                 squadrons[i].stance = GroupStance.Defensive;

//                 foreach(Transform dock in group) {
//                     var f = Instantiate(fighterPrefab, dock.position, dock.rotation, faction.transform);
//                     fighters[i][j] = f.GetComponent<FighterShip>();

//                     // setup faction
//                     fighters[i][j].Faction = faction;
//                     faction.AddTargetable(f.GetComponent<Health>());

//                     // set the state of the fighter as inactive
//                     fighters[i][j].DockWith(dock);
                    
//                     // Add Pilots to command groups
//                     var pilot = fighters[i][j].GetComponent<AIPilot>();
//                     pilot.CommandGroup = squadrons[i];
//                     squadrons[i].AddPilot(pilot);
//                     j++;
//                 }
//                 faction.commandGroups.Add(squadrons[i]);
//                 i++;
//             }
//         } else {
//             Debug.LogWarning("DockParent of AICarrier: " + name + " has not been assigned!");
//         }
        
//         Node offenseNode = new Sequence(new Node[] {
//             new Node(IsActive),
//             new Node(shipPilot.InBattle),
//             new Node(shipPilot.GetPriorityTarget),
//             new Node(shipPilot.AttackAny),
//             new Node(shipPilot.GetRouteTo),                       // get a route to it
//             new Node(shipPilot.MoveTo),                           // Move to the next position in the route
//             new Selector(
//                 new Node[] {
//                     new Sequence(new Node[] {
//                         new Node(AnyFightersLaunched),
//                         new Node(FightersAttackOther),
//                     }),
//                     new Sequence(
//                         new Node[] {
//                             new Inverter(                       // If all fighters have not been launched, launch fighters
//                                 new Node(AllFightersLaunched)
//                             ),
//                             new Node(CanLaunch),
//                             new Node(DeployFighters),
//                             new Node(FightersAttackOther),
//                         }
//                     ),
//                 }
//             )
//         });

//         Node defenseNode = new Selector(new Node[] {
//             new Sequence(new Node[] {
//                 new Node(IsActive),
//                 new Node(AnyFightersLaunched),
//                 new Node(ProtectOrder)
//             }),
//             new Sequence(new Node[] {
//                 new Node(IsActive),
//                 new Node(CanLaunch),           
//                 new Node(DeployFighters),
//                 new Node(ProtectOrder)
//             })
//         });
//         shipPilot.AppendDefenseNode(defenseNode);
//         shipPilot.AppendOffenseNode(offenseNode);   
//     }

//     public State DeployFighters() {
//         if (fighters.Length > 0 && currentGroup < fighters.Length) {
//             for (int i = 0; i < fighters[currentGroup].Length; i++) {
//                 if (fighters[currentGroup][i] != null && fighters[currentGroup][i].IsDocked()) {
//                     fighters[currentGroup][i].Undock();
//                     fighters[currentGroup][i].Rigidbody.AddForce(
//                         fighters[currentGroup][i].transform.forward * launchForce
//                     );
//                 }
//             }
//             currentGroup++;
//             if (currentGroup >= fighters.Length) {
//                 allFightersLaunched = true;
//             }
//             return State.Success;
//         }
//         return State.Failure;
//     }

//     public State AllFightersLaunched() {
//         if (allFightersLaunched) {
//             return State.Success;
//         }
//         return State.Failure;
//     }

//     public State AnyFightersLaunched() {
//         if (currentGroup > 0) {
//             return State.Success;
//         }
//         return State.Failure;
//     }

//     /// <summary>
//     /// Checks if enough time has passed between the last fighter launch, if so sets the next launch to the current time plus the launchDelay.
//     /// </summary>
//     /// <returns>Success if the delay has passed.</returns>
//     public State CanLaunch() {
//         if (Time.fixedTime > nextLaunch) {
//             nextLaunch = Time.fixedTime + launchDelay;
//             return State.Success;
//         }
//         return State.Failure;
//     }

//     /// <summary>
//     /// Orders the most recently launched squadron that isn't dead to protect this ship.
//     /// </summary>
//     /// <returns>Success if there is a squadron to command, Failure otherwise.</returns>
//     protected State ProtectOrder() {
//         if (protectEnd < Time.fixedTime) {
//             protectEnd = Time.fixedTime + protectDuration;
//         } else {
//             return State.Failure;
//         }
//         int group = currentGroup - 1;
//         while(group >= 0) {
//             if (!squadrons[group].Dead) {
//                 squadrons[group].AddCommand(new ProtectTargetCommand(shipPilot.Health, 400f, protectDuration), 1f);
//                 return State.Success;
//             }
//             group--;
//         }
//         return State.Failure;
//     }

//     protected State FightersAttackOther() {
//         int group = currentGroup - 1;
//         while(group >= 0) {
//             if (!squadrons[group].Dead) {
//                 squadrons[group].AddCommand(new AttackTargetCommand(shipPilot.target), 1f);
//                 return State.Success;
//             }
//             group--;
//         }
//         return State.Failure;
//     }
// }
