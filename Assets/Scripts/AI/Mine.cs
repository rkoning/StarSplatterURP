using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private Targetable target;

    public Health health;
    public bool seekTarget;
    public float seekForce;   
    public ParticleSystem seekParticles;

    private Rigidbody rb;

    private void Start() {
        if (seekTarget) {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate() {
        if (seekTarget && target) {
            rb.AddForce((target.transform.position - transform.position).normalized * seekForce);
        }
    }

    public void SetTarget(GameObject t) {
        var tgt = t.GetComponent<Targetable>();
        if (tgt) {
            this.target = tgt;
            if (seekParticles && seekParticles.isStopped) {
                seekParticles.Play();
            }
        }
    }

    public void Explode() {
        health.Die();
    }
}
