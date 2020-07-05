using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI.Factions;

public class ParticleDamage : MonoBehaviour
{
    private MajorFaction faction;
    private float damage;
    private bool structureDamage;

    public void SetDamage(float damage, bool structureDamage) {
        this.damage = damage;
        this.structureDamage = structureDamage;
    }

    public void SetFaction(MajorFaction faction) {
        this.faction = faction;
    }

    private void OnParticleCollision(GameObject other) {
        var t = other.GetComponent<Target>();
        if (t) {
            t.AttackedBy(faction);
        }
        var h = other.GetComponent<Health>();
        if (h && h.Faction != this.faction) {
            h.TakeDamage(damage, this.faction, structureDamage);
        }
    }
}
