using System;
using UnityEngine;

public class Launchable : MonoBehaviour {
   
   private Rigidbody rb;

   public float launchForceMultiplier = 20f;
   public float launchForceLimit;

   private void Start() {
      rb = GetComponent<Rigidbody>();
   }

   public void Launch(Vector3 target) {
      // Get vector to target
      Vector3 targetDirection = (target - transform.position).normalized;

      // Cancel forces
      float currentForce = rb.velocity.sqrMagnitude;
      rb.velocity = Vector3.zero;

      // Debug.DrawLine(transform.position, target, Color.cyan, 10f);
      // Debug.DrawLine(transform.position, targetDirection * 1000f, Color.yellow, 10f);

      // Debug.DrawLine(transform.position, transform.position + targetDirection * currentForce, Color.yellow, 100f);
      // add force equal to the current force in target direction
      float force = currentForce * launchForceMultiplier;
      if (launchForceLimit > 0) {
         force = Mathf.Clamp(force, 0, launchForceLimit);
      }
      rb.AddForce(targetDirection * launchForceLimit);
   }
}