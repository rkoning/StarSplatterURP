using UnityEngine;

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

    public AudioSource ExplosionSound;

    private Shield shield;
    public bool pooled;

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
    public override bool TakeDamage(float damage, bool structureDamage) {
        // if the invincibily delay has not timed out, then return false.
        if (Time.fixedTime < nextHit) {
            return false;
        }

        nextHit = invincibiltyDelay + Time.fixedTime;
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
            if (DamageParticles) 
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
        if (ExplosionParticles) {
            ExplosionParticles.transform.SetParent(null);
            ExplosionParticles.Play();
            if (!pooled)
                Destroy(ExplosionParticles, ExplosionParticles.main.duration);
        }
        if (ExplosionSound) {
            ExplosionSound.Play();
        }
        Death();
        if (pooled) {
            gameObject.SetActive(false);
        } else {
            Destroy(gameObject);
        }
    }

    public override void Heal(float health) {
        currentHealth += health;
        if (currentHealth >= maxHealth / 2f) {
            damaged = false;
            if (DamageParticles && DamageParticles.isPlaying) {
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