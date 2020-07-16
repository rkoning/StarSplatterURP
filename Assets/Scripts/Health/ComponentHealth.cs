using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHealth : LightHealth {
    private CompositeHealth parent;
    public CompositeHealth Parent { set { parent = value; } }
    public bool destroyOnDeath = true;
    protected override void Start() {
        base.Start();
        DamageParticles = GameObject.Instantiate(DamageParticles, transform.position, transform.rotation, transform.parent);
        ExplosionParticles = GameObject.Instantiate(ExplosionParticles, transform.position, transform.rotation, transform.parent);

        OnDamaged += () => { };
        OnDeath += (Health h) => { };
    }

    public override void Die() {
        if (!dead) {
            base.Die();
            DamageParticles.transform.SetParent(parent.transform);
            parent.TakeComponentDamage(this);
            if (destroyOnDeath) {
                Destroy(gameObject);
            }
        }
    }
}
