using UnityEngine;

using AI.BehaviourTree;

namespace AI {
   public class AICapital : AIShip {
      
      public Weapon primaryWeapon;
      public float attackRange;
      public float minAttackRange;

      protected Dynamic defenseNode = new Dynamic(new Selector(new Node[] {}));
      protected Dynamic offenseNode = new Dynamic(new Selector(new Node[] {}));

      [Header("Physics Forces")]
      public float turnSpeed;
      public float baseThrustForce;
      public float thrustForce;     
      public float verticalForce;
      
      protected override void Start() {
         base.Start();

         Node bT = new Selector(new Node[] {
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
         ((Selector) defenseNode.node).AppendNode(node);
      }

      public void AppendOffenseNode(Node node) {
         ((Selector) offenseNode.node).AppendNode(node);
      }

      public State MoveTo() {
         if (agent.SquaredDistanceToTarget < Mathf.Pow(minAttackRange, 2)) {
               return State.Success;
         }

         Move(agent.NextPosition);

         if (agent.SquaredDistanceToTarget < Mathf.Pow(attackRange, 2)) {
               return State.Success;
         }
         return State.Running;
      }

      public override State MoveToPosition() {
         Move(agent.NextPosition);
         if (agent.SquaredDistanceToTarget < Mathf.Pow(agent.size, 2)) {
            return State.Success;
         }
         return State.Running;
      }

      protected void Move(Vector3 position) {
         rb.AddForce(
               transform.forward *
               (baseThrustForce + thrustForce) * 
               agent.GetTargetSpeed()
         );

         float yDiff = Mathf.Clamp(position.y - transform.position.y, -1f, 1f);
         
         rb.AddForce(
               transform.up *
               verticalForce * yDiff
         );

         Quaternion desiredRotation = Quaternion.Slerp(
               transform.rotation, 
               Quaternion.LookRotation(new Vector3(position.x, transform.position.y, position.z) - transform.position),
               turnSpeed * Time.deltaTime
         );
         transform.rotation = desiredRotation;
      }

      protected State AttackTarget() {
         if (primaryWeapon != null) {
               primaryWeapon.Fire();
               return State.Success;
         }
         return State.Failure;
      }

      public Vector3[] GetCircularPath(Transform t, float distance) {
         Vector3[] points = new Vector3[8];
         float increment = (float) 360 / points.Length;
         for (int i = 0; i < points.Length; i++) {
               points[i] = t.position + t.right * distance * Mathf.Cos(increment * i) + t.forward * distance * Mathf.Sin(increment * i);
         }
         return points;
      }

      protected State InRange() {
         if (GetSquaredDistanceTo(target.transform.position) < (attackRange * attackRange)) {
               return State.Success;
         }
         return State.Failure;
      }
   }
}