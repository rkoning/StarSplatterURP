using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public abstract class Health : MonoBehaviour
{

    public delegate void DamagedAction();
    public event DamagedAction OnDamaged;
    public UnityEvent OnDamagedEvent = new UnityEvent();

    public delegate void DeathAction(Health health);
    public event DeathAction OnDeath;
    public UnityEvent OnDeathEvent = new UnityEvent();

    public float maxHealth;
    public float currentHealth;

    protected bool dead;
    public bool Dead {
        get { return dead; }
    }

    public bool damageFlashEnabled = false;
    public Color flashColor;
    public MeshRenderer flashRenderer;
    private Coroutine flashing;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnDamaged += () => {};
        OnDeath += (Health health) => {};
    }

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
    public abstract bool TakeDamage(float damage, bool structureDamage);

    public abstract void Heal(float health);
    
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
        if (damageFlashEnabled) {
            if (flashing == null) {
                flashing = StartCoroutine(DamageFlash());
            }
        }
        OnDamagedEvent.Invoke();
    }

    /// <summary>
    /// Calls OnDeath delegate
    /// </summary>
    protected void Death() {
        OnDeath(this);
        OnDeathEvent.Invoke();
    }

    protected IEnumerator DamageFlash() {
        var origColor = flashRenderer.material.color;
        flashRenderer.material.color = flashColor;
        yield return new WaitForSeconds(0.2f);
        flashRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        flashRenderer.material.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        flashRenderer.material.color = origColor;
        flashing = null;
    }
}
