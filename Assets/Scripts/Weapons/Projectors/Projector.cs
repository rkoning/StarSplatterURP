using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projector : MonoBehaviour
{

    protected AudioSource sound;
    public delegate void HitDamageAction();
    public event HitDamageAction OnHitDamage;
    public delegate void HitAnyAction();
    public event HitAnyAction OnHitAny;
    public float range;

    protected bool structureDamage;
    public bool StructureDamage {
        set {
            structureDamage = value;
        }
    }

    protected float damage;
    public float Damage {
        set { 
            damage = value;
        }
    }

    protected Weapon weapon;
    public Weapon Weapon {
        get { return weapon; }
        set { weapon = value; }
    }

    protected virtual void Start() {
        sound = GetComponent<AudioSource>();
    }

    public virtual void Emit() {
        if (sound != null)
            sound.Play();
    }

    /**
     * Deals the weapon's damage to a gameObject, returns true if the object was damageable
     * @param other the GameObject to damage.
     */
    public virtual bool DealDamage(GameObject other) {
        OnHitAny();
        var t = other.GetComponent<Targetable>();
        if (t) {
            t.AttackedBy(weapon.owner);
        }

        var hp = other.GetComponent<Health>();
        if (hp != null) {
            bool dealtDamage = hp.TakeDamage(damage, structureDamage);
            if (dealtDamage)
                OnHitDamage();
            return true;
        }
        return false;
    }

    protected Vector3 ConvergePoint(Transform converge, float distance) {
        return ((converge.position + converge.forward * distance) - transform.position);
    }

    protected void HitAny() {
        OnHitAny();
    }

    protected void HitDamage() {
        OnHitDamage();
    }
}
