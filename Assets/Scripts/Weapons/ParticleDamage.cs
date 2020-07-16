using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    private float damage;
    private bool structureDamage;

    public void SetDamage(float damage, bool structureDamage) {
        this.damage = damage;
        this.structureDamage = structureDamage;
    }

    private void OnParticleCollision(GameObject other) {
        var t = other.GetComponent<Targetable>();

        var h = other.GetComponent<Health>();
        if (h) {
            h.TakeDamage(damage, structureDamage);
        }
    }
}
