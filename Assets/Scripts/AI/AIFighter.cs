using System.Collections.Generic;
using UnityEngine;
using AI.BehaviourTree;
using Navigation;

namespace AI {
    public class AIFighter : AIShip
    {

        [Header("AI Values")]
        public float minAttackRange = 50f;
        public float maxAttackRange = 300f;

        public Vector3 strafePoint;

        public bool evading;

        public float evadeDistance = 100f;

        public float evadeTime = 3f;
        private float evadeEnd;
        private Vector3 evadeDestination;

        private bool launched;
        public float launchDuration = 3f;

        private bool strafing;
        private bool strafeStartSet = false;

        public float accuracySpread = 20f;
        public float accuracyIncreasePerShot = 0.5f;
        private float accuracyModifier = 0f;

        public float loseInterestAfter = 5f;
        public float intrestLostPerShot = 1f;
        private float remainingInterest;
        private float regainIntrestAt;

        [Header("Physics Values")]

        public float turnSpeed;

        public float baseThrustForce;
        public float brakeForce;
        public float thrustForce;

        public float boostForce;
        private int boostLevel = 0;
        public int BoostLevel {
            get { return boostLevel; }
            set { boostLevel = value; }
        }

        protected override void Start() {
            base.Start();
            remainingInterest = loseInterestAfter;

            AppendOffenseNode(new Selector(new Node[] {
                new Sequence(new Node[] {
                    new Node(IsEvading),
                    new Node(Evade),                     // check if we should be evading
                    new Node(GetRouteTo),
                    new Node(MoveToPosition)             // go to the evade point
                }),
                new Sequence(new Node[] {
                    new Node(HasTarget),
                    new Inverter(new Sequence(new Node[] {                   // check if we should keep shooting at the same target
                        new Inverter(new Node(CheckInterest)),
                        new Node(EvadeStart),                   // if intrest is lost, start evading to a random point
                    })),
                    new Sequence(new Node[] {
                        new Node(GetRouteTo),                   // if we have intrest, try to attack the target until we loose it
                        new Node(SeekTarget),                   // Follow target
                        new Node(AttackTarget)                  // Attack the target if it is in range
                    })
                })
            }));

            AppendDefenseNode(
                new Sequence(new Node[] {       
                    new Node(EvadeStart),           // if so, start evading
                    new Node(GetRouteTo)            // get route to the evade point
                })
            );
        }

        /// <summary>
        /// Checks if the ship is docked
        /// </summary>
        /// <returns>Success if the ship is not docked and should take actions, Failure if it is docked</returns>
        public State IsActive() {
            if (!IsDocked()) {
                return State.Success;
            }
            return State.Failure;
        }

        /// <summary>
        /// Rotates towards the agent's next position and applys a force to the ship's Rigidbody
        /// </summary>
        private void Move() {
            // Debug.DrawLine(transform.position, Agent.NextPosition, Color.cyan);
            var targetRotation = Quaternion.LookRotation(agent.NextPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            float force = baseThrustForce + thrustForce + boostForce * BoostLevel;
            if (agent.GetTargetSpeed() > 0.9f) {
                // TryBoost();            
            } else if (agent.GetTargetSpeed() < 0.3f) {
                force = brakeForce;
            }

            rb.AddForce(transform.forward * (force));
        }

        /// <summary>
        /// Moves to the current target position by calling Move()
        /// </summary>
        /// <returns>Success if the ship is near the target position, Running otherwise</returns>
        public override State MoveToPosition() {
            Move();
            // Debug.DrawLine(transform.position, targetPosition, Color.magenta);
            if (agent.SquaredDistanceToTarget < Mathf.Pow(50f, 2)) {
                return State.Success;
            }
            return State.Running;
        }

        public State SeekTarget() {
            Move();
    
            // Debug.DrawLine(transform.position, agent.NextPosition, Color.blue);
            if (agent.SquaredDistanceToTarget < Mathf.Pow(maxAttackRange, 2)) {
                remainingInterest -= Time.deltaTime;
                if (Vector3.Angle(transform.forward, GetAttackPoint(currentTarget.transform, accuracySpread - accuracyModifier) - transform.position) < 2f) {
                    return State.Success;
                }
                return State.Success;
            }
            return State.Running;
        }

        private State CheckInterest() {
            if (remainingInterest <= 0f) {
                return State.Failure;
            }
            return State.Success;
        }
        
        public State AttackTarget() {
            if (primary && primary.Fire()) {
                remainingInterest -= intrestLostPerShot;
                accuracyModifier = accuracyModifier < accuracySpread ? accuracyModifier + accuracyIncreasePerShot : accuracyModifier;
            }
            if (agent.SquaredDistanceToTarget > Mathf.Pow(minAttackRange, 2)) {
                return State.Running;
            }
            accuracyModifier = 0;
            remainingInterest = 0;
            EvadeStart();
            return State.Failure;
        }

        private Vector3 GetPositionWithLoS(Transform target, float distance) {
            RaycastHit hit;
            for (int i = 0; i < 1000; i++) {
                Vector3 sample = Random.onUnitSphere * distance + target.position;
                Ray ray = new Ray(sample, target.position - sample);
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider.gameObject == target.gameObject) {
                        return sample;
                    }
                }
            }
            return target.position;
        }

        private State EvadeStart() {
            if (evading) {
                return State.Failure;
            } else {
                evading = true;
                evadeEnd = Time.fixedTime + evadeTime;
                regainIntrestAt = 0f;
                targetPosition = GetPositionWithLoS(transform, evadeDistance);
                return State.Success;
            }
        }

        private State Evade() {
            if (GetSquaredDistanceTo(targetPosition) < 50f || Time.fixedTime > evadeEnd) {
                evading = false;
                remainingInterest = loseInterestAfter;
                return State.Failure;
            }
            return State.Success;
        }

        private State IsEvading() {
            if (evading) {
                return State.Success;
            }
            return State.Failure;
        }

        protected override void OnDamaged() {
            base.OnDamaged();
            hit = true;
        }
    }
}