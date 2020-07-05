using System.Collections.Generic;
using UnityEngine;

using AI.Factions;

public class CompositeHealth : Health {

    public List<ComponentHealth> components;

    public float MaxHealth {
        get { return maxHealth; }
    }
    public float CurrentHealth {
        get { return currentHealth; }
    }

    public float maxStructurePoints;
    private float currentStructurePoints;

    private List<Transform> hullTransforms;

    public List<Transform> HullTransforms {
        set { hullTransforms = value; }
    }

    protected override void Start() {
        currentStructurePoints = maxStructurePoints;
    }
    
    public override bool TakeDamage(float damage, MajorFaction faction, bool structureDamage) {
        if (structureDamage) {
            lastDamagedBy = faction;
            currentStructurePoints -= damage;
            OnStructureDamaged(currentStructurePoints, maxStructurePoints, damage);
            Damage();
            if (currentStructurePoints <= 0) {
                Die();
            }
            return true;

        }
        return false;
    }

    public void TakeComponentDamage(ComponentHealth ch, MajorFaction faction) {
        currentHealth -= 1;
        lastDamagedBy = faction;
        components.Remove(ch);
        Damage();
        if (currentHealth <= 0) {
            Die();
        }
    }

    public override Health GetTarget() {
        return components[Random.Range(0, components.Count)].GetTarget();
    }

    public override void Die() {
        dead = true;
        Death();
    }

    public void OnStructureDamaged(float current, float max, float instance) {
        float random = Random.Range(0, max - current);
        if (random > 1f) {
            for (int i = 0; i < Mathf.CeilToInt(instance); i += 5) {
                try {
                    var comp = components[Random.Range(0, components.Count)];
                    if (comp) {
                        comp.TakeDamage(instance, lastDamagedBy, false);
                    }
                } catch {}
            }
        }
    }

    public Transform GetRandomHullTransform() {
        return hullTransforms[Random.Range(0, hullTransforms.Count)];
    }

    public override void Heal(float health) {
        currentStructurePoints += health;
        if (currentStructurePoints > maxHealth) {
            currentStructurePoints = maxHealth;
        }
    }

    private void OnCollisionEnter(Collision other) {
        // if (other.impulse.sqrMagnitude > 100) {
        //     Debug.Log(Mathf.Log(other.impulse.sqrMagnitude));
        //     TakeDamage((int) (other.impulse.sqrMagnitude / 1000f), null);
        // }    
    }

    public void SetupComponents(List<ComponentHealth> components) {
        this.components = components;
        maxHealth = components.Count;
        currentHealth = maxHealth;
        for (int i = 0; i < maxHealth; i++) {
            this.components[i].OnDeath += (Health h) => { TakeComponentDamage((ComponentHealth) h, h.GetLastDamagedBy()); };
        }
    }
}