using AI.BehaviourTree;
using Navigation;
using UnityEngine;

namespace AI {
   public abstract class AIShip : Ship {
       protected Vector3 targetPosition;
        protected Node root;
        protected NavTreeAgent agent;
        public NavTreeAgent Agent {
            get { return agent; }
        }

        public Targetable currentTarget;

        public bool alive = true;

        protected Dynamic commandNode;
        protected Dynamic defenseNode = new Dynamic(new Parallel(new Node[] {}));
        protected Dynamic offenseNode = new Dynamic(new Parallel(new Node[] {}));

        protected bool hit;
        protected Node bT;
        protected override void Start() {
            base.Start();
            agent = GetComponent<NavTreeAgent>();
            // the command node should always return a failure if there is not a command running.
            commandNode = new Dynamic(new Node(() => { return State.Failure;}));
            
            bT = new Selector(new Node[] {
                new Sequence(new Node[] {
                    new Node(HasBeenHit),
                    defenseNode
                }),
                commandNode,
                offenseNode,
                new Node(Idle)
            });
            SetBehaviourTreeRoot(bT);
        }

        public void SetDefenseNode(Node node) {
            defenseNode.SetNode(node);
        }

        public void SetOffenseNode(Node node) {
            offenseNode.SetNode(node);
        }

        public void AppendDefenseNode(Node node) {
            ((Parallel) defenseNode.node).AppendNode(node);
        }

        public void AppendOffenseNode(Node node) {
            ((Parallel) offenseNode.node).AppendNode(node);
        }


        public void SetBehaviourTreeRoot(Node root) {
            this.root = root;
        }

        protected virtual void Update() {
            if (alive) {
                // root node is evaluated every frame.
                root.Evaluate();
            }
        }

        public void SetCommandNode(Node commandNode) {
            this.commandNode.node = commandNode;
        }
        
        public Node GetCommandNode() {
            return this.commandNode.node;
        }

        /// <summary>
        /// Fallback if the ship has no other behaviors to execute
        /// </summary>
        /// <returns>State.Running</returns>
        protected virtual State Idle() {
            return State.Running;
        }

        /// <summary>
        /// Checks the pilot's target transfrom
        /// </summary>
        /// <returns>Success if the pilot's target is not null, otherwise Failure</returns>
        protected virtual State HasTarget() {
            if (target != null) { 
                // if a target exists, but no current target is assigned, then see if a target can be found
                if (currentTarget == null) {
                    currentTarget = target.GetTarget();

                    // if no target could be found, the target has been destroyed
                    if (currentTarget == null) {
                        target = null;
                        return State.Failure;
                    }
                }
                // if a current target exists, then assign the target position to the current target's position
                targetPosition = currentTarget.transform.position;
                return State.Success;
            }
            return State.Failure;
        }


        /// <summary>
        /// Sets the destination of the route to the current target position.
        /// </summary>
        /// <returns>State.Success</returns>
        public State GetRouteTo() {
            Agent.Destination = targetPosition;
            Agent.CheckRoute(targetPosition);
            return State.Success;
        }

        /// <summary>
        /// Checks if the ship has been damaged in the last frame.
        /// </summary>
        /// <returns>Success if the ship has been damaged in the last frame.</returns>
        protected State HasBeenHit() {
            if (hit) {
                hit = false;
                return State.Success;
            }
            return State.Failure;
        }

        public virtual Targetable GetTarget() {
            return target;
        }

        public float GetSquaredDistanceTo(Vector3 position) {
            return (transform.position - position).sqrMagnitude;
        }

        /// <summary>
        /// Gets a random point along a circle around a target.
        /// Used to reduce the accuracy of weapons.
        /// </summary>
        /// <param name="target">Transform of the target</param>
        /// <param name="accuracy">radius of the circle</param>
        /// <returns>Random point on the circle around the target</returns>
        public Vector3 GetAttackPoint(Transform target, float accuracy) {
            float value = Random.Range(0, 360);
            Vector3 offset = accuracy * (target.right * Mathf.Cos(value) + target.up * Mathf.Cos(value));
            return target.position + offset;
        }

        public void SetTargetPosition(Vector3 position) {
            targetPosition = position;
        }

        public virtual Vector3 GetTargetPosition() {
            return target.transform.position;
        }

        public void SetTarget(Targetable target) {
            this.target = target;
        }

        public abstract State MoveToPosition();
   }
}