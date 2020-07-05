using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{
    public Health target;

    public float turnSpeed;
    public float thrustForce;

    public float launchDelay = 1f;
    private float launchWait;
    private bool targeted;

    public override void OnEmit(Transform target = null) {
        base.OnEmit(target);
        var tg = target.gameObject.GetComponent<Health>();
        if (target) {
            this.target = tg.GetTarget();
        }
        launchWait = launchDelay + Time.fixedTime;
        targeted = false;
    }

    protected override void Update() {
        base.Update();
        
        if (target && launchWait < now) {
            if (!targeted) {
                rb.velocity = Vector3.zero;
                targeted = true;
            }
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            rb.AddForce(transform.forward * thrustForce);
            // transform.position += (target.transform.position - transform.position).normalized * thrustForce;
        }
    }

    public override void SetTarget(Transform target) {
        this.target = target.GetComponent<Health>().GetTarget();
    }
    
    protected void OnDisable() {
        target = null;
    }
}
