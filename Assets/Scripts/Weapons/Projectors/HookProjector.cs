using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookProjector : Projector
{
    public HookProjectile projectile;
    private SpringJoint joint;

    public LineRenderer line;
    private Rigidbody parentRb;
    public Rigidbody ParentRb {
        get { return parentRb; }
    }

    protected override void Start() {
        base.Start();
        line.gameObject.SetActive(false);
        projectile.transform.SetParent(null);
        projectile.gameObject.SetActive(false);
        parentRb = Weapon.GetComponentInParent<Rigidbody>();
        projectile.Projector = this;
    }

    public override void Emit() {
        base.Emit();
        projectile.gameObject.SetActive(true);
        line.gameObject.SetActive(true);
        projectile.Projector = this;
        projectile.line = line;
        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;
        projectile.OnEmit();
    }

    public void Detach() {
        projectile.Detach();
        projectile.gameObject.SetActive(false);
        if (joint) {
            Destroy(joint);
        }
        line.gameObject.SetActive(false);
    }

    public void OnAttachRb(Rigidbody other) {
        joint = parentRb.gameObject.AddComponent<SpringJoint>();     
        joint.maxDistance = 20f;
        joint.massScale = 0.001f;
        joint.connectedBody = other;
        joint.autoConfigureConnectedAnchor = false;
        joint.enableCollision = true;
    }

    private void OnDestroy() {
        if (projectile) {
            Destroy(projectile.gameObject);
        }
    }
}
