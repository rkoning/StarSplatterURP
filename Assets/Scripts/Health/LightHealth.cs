using UnityEngine;

using AI.Factions;

public class LightHealth : Health
{

    private bool damaged;
    public bool Damaged {
        get { return damaged; }
    }

    public float invincibiltyDelay;
    private float nextHit;

    public ParticleSystem DamageParticles;

    public ParticleSystem ExplosionParticles;

    private Shield shield;

    protected override void Start() {
        base.Start();
        shield = GetComponent<Shield>();    
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        TakeDamage(10, null, false);
    //    }
    //}

    /// <summary>
    /// Deals damage to this ship/component and set it's state to damaged, play damage particles and call the OnDamaged delegate.
    /// if this has already been damaged and the current time between the last hit is greater than the invuln delay, call Die().
    /// </summary>
    /// <param name="damage">Amount of damage to deal.</param>
    /// <returns>True if this was damaged by the hit.</returns>
    public override bool TakeDamage(float damage, MajorFaction faction, bool structureDamage) {
        // if the invincibily delay has not timed out, then return false.
        if (Time.fixedTime < nextHit) {
            return false;
        }

        nextHit = invincibiltyDelay + Time.fixedTime;
        lastDamagedBy = faction;
        Damage();

        // if shielded, then redirect damage onto the shield and return true
        if (shield && shield.Active) {
            shield.currentHealth -= damage;
            if (shield.currentHealth <= 0) {
                shield.BreakShield();
            }
            return true;
        }

        currentHealth -= damage;

        // if unshielded deal damage, play particles and return true
        if (!damaged && currentHealth < maxHealth / 2f) {
            damaged = true;
            DamageParticles.Play();
            return true;
        }

        // if already damaged and unshielded, Die() and return true
        if (currentHealth < 1f) {
            Die();
        }
        return true;
    }

    /// <summary>
    /// Plays ExplosionParticles and destroys them after their duration. Calls the OnDeath delegate.
    /// </summary>
    public override void Die() {
        dead = true;
        ExplosionParticles.transform.SetParent(null);
        ExplosionParticles.Play();
        Destroy(ExplosionParticles, ExplosionParticles.main.duration);
        Death();
    }

    public override void Heal(float health) {
        currentHealth += health;
        if (currentHealth >= maxHealth / 2f) {
            damaged = false;
            if (DamageParticles.isPlaying) {
                DamageParticles.Stop();
            }
        }
    }

    /// <summary>
    /// LightHealth components have no targetable sub-components, returns this.
    /// </summary>
    /// <returns>this</returns>
    public override Health GetTarget() => this;
}