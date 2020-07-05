using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisjointedBeamProjector : BeamProjector
{

    protected override IEnumerator EmitOverDuration(float duration) {
        beam.enabled = true;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Vector3 hitPoint;
        if (Physics.Raycast(ray, out hit, range, mask)) {
            DealDamage(hit.collider.gameObject);
            hitPoint = hit.point;
        } else {
            hitPoint = ray.GetPoint(range);
        }

        beam.SetPositions(new Vector3[] {transform.position, hitPoint});
        yield return new WaitForSeconds(duration);
        beam.enabled = false;
        emitting = null;
    }
}
