using UnityEngine;
using UnityEngine.Events;

using AI.Factions;

public abstract class Health : MonoBehaviour
{

    public delegate void DamagedAction();
    public event DamagedAction OnDamaged;
    public UnityEvent OnDamagedEvent = new UnityEvent();

    public delegate void DeathAction(Health health);
    public event DeathAction OnDeath;
    public UnityEvent OnDeathEvent = new UnityEvent();

    protected MajorFaction faction;
    public MajorFaction Faction {
        get { return faction; }
        set { faction = value; }
    }
    
    public float maxHealth;
    protected float currentHealth;

    protected bool dead;
    public bool Dead {
        get { return dead; }
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnDamaged += () => {};
    }

    protected MajorFaction lastDamagedBy;

    /// <summary>
    /// Gets a health component on this object that should be used to target, on larger ships this will be a subcomponent
    /// </summary>
    /// <returns>Health component</returns>
    public abstract Health GetTarget();

    /// <summary>
    /// Deals an integer amount of damage to the object, usually will call the Die() method after enough damage is taken
    /// </summary>
    /// <param name="damage">Amount of damage to apply</param>
    /// <param name="faction">Faction of the weapon that dealt damage</param>
    /// <returns>True if damage was applied</returns>
    public abstract bool TakeDamage(float damage, MajorFaction faction, bool structureDamage);

    public abstract void Heal(float health);

    public MajorFaction GetLastDamagedBy() {
        return lastDamagedBy;
    }
    
    /// <summary>
    /// Should be called to kill the damageable object
    /// </summary>
    public abstract void Die();

    public float GetCurrentHealth() {
        return currentHealth;
    }

    /// <summary>
    /// Calls OnDamaged delegate
    /// </summary>
    protected void Damage() {
        OnDamaged();
        OnDamagedEvent.Invoke();
    }

    /// <summary>
    /// Calls OnDeath delegate
    /// </summary>
    protected void Death() {
        OnDeath(this);
        OnDeathEvent.Invoke();
    }
}
