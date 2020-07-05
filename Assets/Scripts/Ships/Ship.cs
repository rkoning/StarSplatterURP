using UnityEngine;

using AI.Factions;

public abstract class Ship : MonoBehaviour
{

    public Target target;
    
    protected Target self;
    public Target Self {
        get { return self; }
    }

    protected Rigidbody rb;
    public Rigidbody Rigidbody {
        get { return rb; }
    }

    protected Health health;

    public Weapon primary;
    public Weapon secondary;
    public Weapon equipment;

    public Transform[] primaryAnchors;
    public Transform[] secondaryAnchors;
    public Transform[] equipmentAnchors;

    public bool docked;
    protected Transform dockTransform;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        self = GetComponent<Target>();
        health.OnDeath += OnDeath;
        health.OnDamaged += OnDamaged;
        SetupWeapons();
    }

    public void SetupWeapons() {
        if (primary) {
            primary.owner = self;
        }
        if (secondary) {
            secondary.owner = self;
        }
        if (equipment) {
            equipment.owner = self;
        }
    }

    public void StartAllParticles(ParticleSystem[] particleGroup) {
        foreach (ParticleSystem p in particleGroup) {
            if (!p.isPlaying) {
                p.Play();
            }
        }
    }

    public void StopAllParticles(ParticleSystem[] particleGroup) {
        foreach (ParticleSystem p in particleGroup) {
            if (p.isPlaying) {
                p.Stop();
            }            
        }
    }

    protected virtual void OnDeath(Health health) {
        // faction.RemoveTargetable(health);
    }

    protected virtual void OnDamaged() {
        // Debug.Log("damaged");
    }
    
    public bool IsDocked() {
        return docked;
    }

    public void DockWith(Transform dockTransform) {
        this.dockTransform = dockTransform;
        this.docked = true;

        if (rb == null) {
            rb = GetComponent<Rigidbody>();
        }
    }
}
