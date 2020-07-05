using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.BehaviourTree;

namespace AI {
    public class AITurret : AIShip
    {

        public float accuracy;
        public float rotationSpeed;
        public Transform turretPivot;
        public Transform barrelPivot;

        public float attackRange = 400f;
        public float minRotation;
        public float maxRotation;

        public float minElevation;
        public float maxElevation;

        public float accuracySpread = 20f;
        public float accuracyIncreasePerShot = 0.5f;
        private float accuracyModifier = 0f;

        protected override void Start() {
            base.Start();

            Node entry = new Selector(new Node[] {
                commandNode,
                new Sequence(new Node[] {
                    new Node(HasTarget),
                    new Node(AttackTarget)
                }),
                new Node(Idle)              // if a target was not found, practice neutral chi
            });
            SetBehaviourTreeRoot(entry);
        }

        private State TargetInView() {
            if (Physics.Linecast(barrelPivot.position + barrelPivot.forward * 20, targetPosition)) {
                return State.Failure;
            }
            return State.Success;
        }

        private State AttackTarget() {
            Vector3 localTargetPos = transform.InverseTransformPoint(targetPosition);

            Quaternion rotationGoal = Quaternion.LookRotation(new Vector3(localTargetPos.x, 0f, localTargetPos.z));
            Quaternion newRotation = Quaternion.RotateTowards(turretPivot.localRotation, rotationGoal, rotationSpeed * Time.deltaTime);
            turretPivot.localRotation = newRotation;


            localTargetPos = turretPivot.InverseTransformPoint(targetPosition);
            Quaternion barrelGoal = Quaternion.LookRotation(new Vector3(0f, localTargetPos.y, localTargetPos.z));
            Quaternion barrelRotation = Quaternion.RotateTowards(barrelPivot.localRotation, barrelGoal, rotationSpeed * Time.deltaTime);
            barrelPivot.localRotation = barrelRotation;
            float x = barrelPivot.localEulerAngles.x;
            // if (x > 0 && x < 270) {
            //     x = 0;
            // }
            barrelPivot.localEulerAngles = new Vector3(x, 0f, 0f);

            // Ray ray = new Ray(barrelPivot.position + barrelPivot.forward * 10f, targetPosition - (barrelPivot.position + barrelPivot.forward * 10f));

            RaycastHit hit;
            if (Physics.Raycast(barrelPivot.position, targetPosition - barrelPivot.position, out hit)) {
                if (hit.collider.gameObject != target.gameObject) {
                    return State.Failure;       
                }
            }
            
            if (Vector3.Dot(barrelPivot.forward, (targetPosition - barrelPivot.position).normalized) > accuracy) {
                primary.Fire();
                // return State.Running;
            }

            return State.Running;
        }

        public override State MoveToPosition() {
            return State.Success;
        }
    }
}