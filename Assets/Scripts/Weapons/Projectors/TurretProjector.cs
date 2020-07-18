using System.Collections;
using UnityEngine;

public class TurretProjector : Projector {
   public Projector[] guns;

   public float accuracy;
   public float rotationSpeed;
   public Transform turretPivot;
   public Transform barrelPivot;

   private Coroutine attacking;

   public override void Emit() {
      attacking = StartCoroutine(RotateToAttack());
   }

   public override void Setup(Weapon weapon, float damage, HitDamageAction hitDamage, HitAnyAction hitAny) {
      base.Setup(weapon, damage, hitDamage, hitAny);
      for( int i = 0; i < guns.Length; i++) {
         guns[i].Setup(weapon, damage, hitDamage, hitAny);
      }
   }

   private IEnumerator RotateToAttack() {
      // the parent weapon or the target may get destroyed during the coroutine, so ensure that they still exist
      if (weapon && weapon.target) {
         
         Vector3 targetPosition = weapon.target.GetPosition();

         RotateTurret(targetPosition);
         bool canFire = true;
         RaycastHit hit;
         if (Physics.Raycast(barrelPivot.position, targetPosition - barrelPivot.position, out hit)) {
            if (hit.collider.gameObject != weapon.target.gameObject) {
               canFire = false;
            }
         }
         
         if (!(Vector3.Dot(barrelPivot.forward, (targetPosition - barrelPivot.position).normalized) > accuracy)) {
            canFire = false; 
         }
         if (!canFire)
            yield return null;

         for (int i = 0; i < guns.Length; i++) {
            guns[i].Emit();
         }
      } 
   }

   private void RotateTurret(Vector3 targetPosition) {

      // Vector3 localTargetPos = transform.InverseTransformPoint(targetPosition);

      // Quaternion rotationGoal = Quaternion.LookRotation(new Vector3(localTargetPos.x, 0f, localTargetPos.z));
      // Quaternion newRotation = Quaternion.RotateTowards(turretPivot.localRotation, rotationGoal, rotationSpeed * Time.deltaTime);
      // turretPivot.localRotation = newRotation;


      // localTargetPos = turretPivot.InverseTransformPoint(targetPosition);
      // Quaternion barrelGoal = Quaternion.LookRotation(new Vector3(0f, localTargetPos.y, localTargetPos.z));
      // Quaternion barrelRotation = Quaternion.RotateTowards(barrelPivot.localRotation, barrelGoal, rotationSpeed * Time.deltaTime);
      // barrelPivot.localRotation = barrelRotation;
      // float x = barrelPivot.localEulerAngles.x;

      // barrelPivot.localEulerAngles = new Vector3(x, 0f, 0f);

      barrelPivot.LookAt(weapon.target.GetPosition());
   }
}