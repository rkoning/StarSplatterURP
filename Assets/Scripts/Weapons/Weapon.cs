using System.Linq;
using UnityEngine;

using AI.Factions;

public class Weapon : MonoBehaviour
{

    public GameObject projectorPrefab;

    public float coolDown;
    public int damage;
    protected float nextShot;

    public delegate void FireAction();
    public event FireAction OnFire;

    public delegate void HoldAction();
    public event HoldAction OnHold;

    public delegate void ReleaseAction();
    public event ReleaseAction OnRelease;

    private FireMode fireMode;
    private Projector[] projectors;

    public Projector[] Projectors {
        get { return projectors; }
    }
    
    public Target owner;

    private Vector3 firePoint;
    public Vector3 FirePoint {
        get { return firePoint; }
    }

    private void Awake() {
        Assemble();
    }

    private void Assemble() {
        // Get all firemodes on this weapon
        var modes = GetComponentsInChildren<FireMode>();
        foreach (var mode in modes)
        {
            mode.Weapon = this;
        }
        // the first firemode will be called OnFire() and will call subsequent firemodes after it.
        fireMode = modes[0];
        OnFire += fireMode.Fire;
        OnHold += fireMode.Hold;
        OnRelease += fireMode.Release;
        
        // assign a weapon and damage to all projectors, and register callbacks for OnHitAny and OnHitDamage
        projectors = GetComponentsInChildren<Projector>();
        foreach(var p in projectors) {
            p.Weapon = this;
            p.Damage = damage;
            p.OnHitDamage += HitDamage;
            p.OnHitAny += HitAny;
        }
    }

    public void ResetProjectors() {
        projectors = new Projector[0];
    }

    public void AddProjectorTo(Transform t) {
        var newWeaponModel = GameObject.Instantiate(projectorPrefab, t);
        var newProjector = newWeaponModel.GetComponentInChildren<Projector>();
        newProjector.Weapon = this;
        newProjector.Damage = damage;
        newProjector.OnHitDamage += HitDamage;
        newProjector.OnHitAny += HitAny;
        projectors = projectors.Append(newProjector).ToArray();
    }

    public void SetupFiremodes() {
        var modes = GetComponentsInChildren<FireMode>();
        foreach (var mode in modes) {
            mode.SetProjector(projectors[0]);
        }
    }

    /**
     * Calls OnFire() on the first firemode if the weapon is not on cooldown, then starts the weapons cooldown time.
     * returns true if the weapon was fired.
     */
    public bool Fire() {
        if (Time.fixedTime > nextShot) {
            OnFire();
            nextShot = Time.fixedTime + coolDown;
            return true;
        }
        return false;
    }

    public void Hold() {
        OnHold();
    }

    public void Release() {
        OnRelease();
    }

    public void HitDamage() {
        // Debug.Log("Hit Damage!");
    }
    
    public void HitAny() {
        // Debug.Log("Hit Something!");
    }

    public void StartCooldown() {
        nextShot = Time.fixedTime + coolDown;
    }
    /// <summary>
    /// Gets the range of the first projector if it exists, returns 0 otherwise.
    /// </summary>
    /// <returns>Range of the first projector or zero if it does not exist.</returns>
    public float GetRange() {
        if (projectors.Length < 0) {
            return 0f;
        }
        return projectors[0].range;
    }

    private void OnDestroy() {
        foreach(var projector in projectors) {
            Destroy(projector.gameObject);
        }
    }
}
