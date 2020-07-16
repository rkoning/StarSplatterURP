using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamProjector : Projector {
 
    protected LineRenderer beam;

    public float duration;

    public LayerMask mask;

    public float maxWidth = 0.25f;
    public AnimationCurve widthCurve = AnimationCurve.Constant(0, 1, 1f);


    protected Coroutine emitting;


    protected override void Start() {
        base.Start();
        beam = GetComponentInChildren<LineRenderer>();
        beam.enabled = false;  
    }

    public override void Emit() {
        if (emitting == null) {
            base.Emit();        
            emitting = StartCoroutine(EmitOverDuration(duration));
        }
    }

    protected virtual IEnumerator EmitOverDuration(float duration) {
        beam.enabled = true;
        float now = 0f;
        float dT = Time.deltaTime;
        while (now < duration) {
            Ray ray = new Ray(transform.position, ConvergePoint(transform, range));
            RaycastHit hit;

            Vector3 hitPoint;
            if (Physics.Raycast(ray, out hit, range, mask)) {
                DealDamage(hit.collider.gameObject);
                hitPoint = hit.point;
            } else {
                hitPoint = ray.GetPoint(range);
            }

            beam.SetPositions(new Vector3[] {transform.position, hitPoint});

            float width = widthCurve.Evaluate(now / duration) * maxWidth;
            beam.startWidth = width;
            beam.endWidth = width;

            now += dT;
            yield return null;
        }
        beam.enabled = false;
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
