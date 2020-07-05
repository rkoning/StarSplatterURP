using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public float rechargeDelay = 1f;
    public GameObject shield;
    public ParticleSystem breakEffect;
    public AudioSource breakSound;

    public ParticleSystem rechargeEffect;
    public AudioSource rechargeSound;

    public delegate void OnShieldBreak();
    public OnShieldBreak onShieldBreak;

    public delegate void OnShieldRecharge();
    public OnShieldRecharge onShieldRecharge;

    private Coroutine recharge;

    public float maxHealth;
    public float currentHealth;

    private bool active = true;

    public bool Active {
        get { return active; }
    }

    private void Start()
    {
        onShieldBreak += () => { }; // set default event
        onShieldRecharge += () => { };
    }

    public void BreakShield() {
        active = false;
        if (recharge == null) {
            recharge = StartCoroutine(RechargeAfterDelay(rechargeDelay));
        }

        if (breakEffect) {
            breakEffect.Play();
        }

        if (breakSound) {
            breakSound.Play();
        }

        if (shield) {
            shield.SetActive(false);
        }
        onShieldBreak();
    }

    public void Recharge() {
        active = true;
        if (recharge != null)
            StopCoroutine(recharge);
        recharge = null;

        currentHealth = maxHealth;

        if (rechargeEffect) {
            rechargeEffect.Play();
        }

        if (rechargeSound) {
            rechargeSound.Play();
        }

        if (shield) {
            shield.SetActive(true);
        }
        onShieldRecharge();
    }

    private IEnumerator RechargeAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        Recharge();
    }
}
