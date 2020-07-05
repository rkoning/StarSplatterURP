using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireMode : FireMode
{
    public float chargeTime = 1f;
    public float maxHoldTime = 3f;
    private float currentCharge = 0f;

    private bool chargeStarted = false;

    public ParticleSystem chargeParticles;

    public FireMode nextFireMode;

    public override void Fire() {
        if (!chargeStarted) {
            chargeStarted = true;
        }
        if (chargeParticles) {
            chargeParticles.Play();
        }
    }

    public override void Hold() {
        if (chargeStarted) {
            if (currentCharge <= maxHoldTime) {
                currentCharge += Time.deltaTime;
            } else {
                if (chargeParticles) {
                    chargeParticles.Stop();
                }
                nextFireMode.Fire();
                chargeStarted = false;
                weapon.StartCooldown();
                currentCharge = 0f;
            }
        }
    }

    public override void Release() {
        if (chargeParticles) {
            chargeParticles.Stop();
        }
        Debug.Log(chargeStarted + " " + currentCharge + " " + chargeTime);
        if (chargeStarted && currentCharge >= chargeTime) {
            nextFireMode.Fire();
            weapon.StartCooldown();
        }
        
        currentCharge = 0f;
        chargeStarted = false;
    }

}
