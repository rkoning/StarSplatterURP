using UnityEngine;

public class HookProjectile : Projectile
{

    public LineRenderer line;

    public bool attached;

    public Transform attachedTo;

    public float maxHookStrength;
    
    private HookProjector hP;
    protected override void Start() {
        base.Start();
        hP = (HookProjector) projector;
    }

    protected override void Update() {
        if (hP.ParentRb) {
            line.SetPositions(new Vector3[] { transform.position, hP.transform.position });
        }
        if (!attached) {
            base.Update();
        }
    }

    public void Detach() {
        attached = false;
        rb.isKinematic = false;
        var launchable = transform.GetComponentInParent<Launchable>();
        if (launchable) {
            Vector3 launchPoint;
            RaycastHit hit;
            if (Physics.Raycast(projector.transform.position, projector.transform.forward, out hit)) {
                launchPoint = hit.point;
            } else {
                launchPoint = projector.transform.forward * 1000f;
            }
            launchable.Launch(launchPoint);
        }
        transform.SetParent(null);
        GetComponent<Collider>().enabled = true;
    }
    
    protected override void Die(Vector3 position, Quaternion rotation) {
        line.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    protected override void OnCollisionEnter(Collision other) {
        if (!attached && now > noCollisionEnd) {
            var otherRb = other.gameObject.GetComponent<Rigidbody>();
            if (otherRb && otherRb.isKinematic == false && otherRb.mass < maxHookStrength) {
                if (hitParticle) {
                    hitParticle.transform.position = transform.position;
                    hitParticle.transform.rotation = Quaternion.LookRotation(other.contacts[0].normal);
                    hitParticle.Play();
                }
                attached = true;
                rb.isKinematic = true;
                GetComponent<Collider>().enabled = false;
                transform.SetParent(other.transform);
                hP.OnAttachRb(otherRb);
            }
        }
    }
}