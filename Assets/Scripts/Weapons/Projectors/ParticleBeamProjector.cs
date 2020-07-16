using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBeamProjector : Projector {
 
    protected ParticleSystem beamParticles;

    public float duration;

    public float beamRadius;
    
    public LayerMask mask;

    protected Coroutine emitting;

    protected override void Start() {
        base.Start();
        beamParticles = GetComponentInChildren<ParticleSystem>();
        beamParticles.Stop();  
    }

    public override void Emit() {
        if (emitting == null) {
            base.Emit();        
            emitting = StartCoroutine(EmitOverDuration(duration));
        }
    }

    protected virtual IEnumerator EmitOverDuration(float duration) {
        if (beamParticles.isStopped) {
           beamParticles.Play();
        }
        float now = 0f;
        float dT = Time.deltaTime;
        while (now < duration) {
            Ray ray = new Ray(transform.position, ConvergePoint(transform, range));
            RaycastHit hit;

            Vector3 hitPoint;
            if (Physics.SphereCast(transform.position, beamRadius, ConvergePoint(transform, range), out hit, range, mask)) {
                DealDamage(hit.collider.gameObject);
                hitPoint = hit.point;
            } else {
                hitPoint = ray.GetPoint(range);
            }



            now += dT;
            yield return null;
        }
        if (beamParticles.isPlaying) {
         beamParticles.Stop();
        }
        emitting = null;
    }

    public override bool DealDamage(GameObject other) {
        HitAny();
        var t = other.GetComponent<Targetable>();
        if (t) {
            t.AttackedBy(weapon.owner);
        }

        var hp = other.GetComponent<Health>();
        if (hp != null ) { // damage is not applied if the weapon and hp have the same non-null faction
            bool dealtDamage = hp.TakeDamage((damage * Time.deltaTime) / duration, structureDamage);
            if (dealtDamage)
                HitDamage();
            return true;
        }
        return false;
    }
}
