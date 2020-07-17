// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using BehaviourTree;

// public class BattleShipComponent : ShipComponent
// {
//     public Weapon primaryWeapon;

//     public float minRange;
//     public float maxRange;

//     private int currentPoint = 0;
//     private Vector3[] path = new Vector3[0];

//     protected override void Start()
//     {
//         base.Start();
//         Node offenseNode = new Sequence(new Node[] {
//             new Node(IsActive),
//             new Node(shipPilot.InBattle),
//             new Node(shipPilot.GetPriorityTarget),
//             new Node(shipPilot.AttackAny),
//             new Selector(new Node[] {  
//                 new Sequence(new Node[] {
//                     new Node(InRange),                      // if in range
//                     new Selector(new Node[] {
//                         new Node(HasPath),                  // check if we have an attack path
//                         new Node(GetPath)                   // if not get a new path
//                     }),
//                     new Node(shipPilot.GetRouteTo),                   // get the route to the next point on the path
//                     new Parallel(new Node[] {               
//                         new Sequence(new Node[] {
//                             new Node(shipPilot.MoveToPosition),       // move to the next position
//                             new Node(() => { 
//                                 currentPoint++;
//                                 return State.Success;       // if we are at that position, increment the currentPoint on the path
//                             })
//                         }),
//                         new Node(AttackTarget)              // while moving, fire primary weapon
//                     }),
//                 }),         
//                 new Node(shipPilot.MoveTo)                            // if out of range move into range
//             }),
//         });

//         shipPilot.AppendOffenseNode(offenseNode);
//     }

//     private State AttackTarget() {
//         if (primaryWeapon) {
//             primaryWeapon.Fire();
//             return State.Success;
//         }
//         return State.Failure;
//     }

//     private State InRange() {
//         float dist = shipPilot.GetSquaredDistanceTo(shipPilot.target.transform.position);
//         if (dist <= maxRange * maxRange && dist >= minRange * minRange) {
//             return State.Success;
//         }
//         return State.Failure;
//     }

//         /// <summary>
//     /// Clears the current attack path
//     /// </summary>
//     /// <returns>Success</returns>
//     private State ClearAttackPath() {
//         path = new Vector3[0];
//         return State.Success;
//     }
    
//     /// <summary>
//     /// Checks if this battleship has an attack path and if there are points remaining on that path
//     /// </summary>
//     /// <returns>Failure if the path does not exist, Success if it does</returns>
//     private State HasPath() {
//         if (path.Length == 0 || currentPoint >= path.Length) {
//             return State.Failure;
//         }
//         shipPilot.SetTargetPosition(path[currentPoint]);
//         return State.Success;
//     }

//     /// <summary>
//     /// Gets a circular path around the target's current position
//     /// </summary>
//     /// <returns>Success</returns>
//     private State GetPath() {
//         path = shipPilot.GetCircularPath(shipPilot.GetTarget().transform, (maxRange - minRange) / 2 + minRange);
//         currentPoint = 0;
//         shipPilot.SetTargetPosition(path[0]);

//         return State.Success;
//     }
// }
