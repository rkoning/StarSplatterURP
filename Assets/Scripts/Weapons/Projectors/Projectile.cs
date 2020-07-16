using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;

    public float launchForce;
    protected Rigidbody rb;

    protected Projector projector;
    public Projector Projector {
        set { projector = value; }
    }

    public ParticleSystem hitParticle;
    private TrailRenderer trail;
    public float lifeTime = 5f;
    private float lifeEnd;

    public float noCollisionDuration = 1f;
    protected float noCollisionEnd;

    protected float now;

    protected virtual void Start() {
        Setup();
    }

    private void Setup() {
        hitParticle.transform.SetParent(null);
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }

    public virtual void OnEmit(Transform target = null) {
        if (rb == null) {
            Setup();
        }
        if (trail) {
            trail.Clear();
        }
        noCollisionEnd = Time.fixedTime + noCollisionDuration;
        lifeEnd = lifeTime + Time.fixedTime;
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * launchForce);
    }

    protected virtual void Update() {
        now = Time.fixedTime;
        if (lifeEnd < now) {
            Die(transform.position, transform.rotation);
        }
    }

    protected virtual void Die(Vector3 position, Quaternion rotation) {
        if (hitParticle) {
            hitParticle.transform.position = transform.position;
            hitParticle.transform.rotation = rotation;
            hitParticle.Play();
        }

        gameObject.SetActive(false);
    }
    
    protected virtual void OnCollisionEnter(Collision other) {
        if (now > noCollisionEnd) {
            projector.DealDamage(other.gameObject);
            Die(transform.position, Quaternion.Euler(other.contacts[0].normal));
        }
    }

    public virtual void SetTarget(Transform target) {
        Debug.LogWarning("Warning: SetTarget(Targetable) not implemented by this projectile. " + gameObject.name);
        return;
    }

    public virtual void SetTarget(Vector3 targetPosition) {
        Debug.LogWarning("Warning: SetTarget(Vector3) not implemented by this projectile. " + gameObject.name);
        return;
    }
}
