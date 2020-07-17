using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireMode : FireMode
{
    public float chargeTime = 1f;
    public float maxHoldTime = 3f;
    private float currentCharge = 0f;

    private bool chargeStarted = false;

    public GameObject chargeParticlePrefab;

    public ParticleSystem[] chargeParticles;

    public FireMode nextFireMode;

    private void Start() {
        if (chargeParticles == null) {
            chargeParticles = new ParticleSystem[weapon.Projectors.Length];
            for(int i = 0; i < weapon.Projectors.Length; i++) {
                var part = GameObject.Instantiate(chargeParticlePrefab, weapon.Projectors[i].transform).GetComponent<ParticleSystem>();
                chargeParticles[i] = part;
            } 
        }
    }
    
    public override void Fire() {
        Debug.Log("Fire");
        if (!chargeStarted) {
            chargeStarted = true;
        }
        ForEachParticleDo((ParticleSystem p) => { p.Play(); });
    }

    public override void Hold() {
        Debug.Log("HOLD");
        if (chargeStarted) {
            if (currentCharge <= maxHoldTime) {
                currentCharge += Time.deltaTime;
            } else {
                ForEachParticleDo((ParticleSystem p) => { p.Stop(); });
                nextFireMode.Fire();
                chargeStarted = false;
                weapon.StartCooldown();
                currentCharge = 0f;
            }
        } else {
           Fire();
        }
    }

    public override void Release() {
        ForEachParticleDo((ParticleSystem p) => { p.Stop(); });
        
        if (chargeStarted && currentCharge >= chargeTime) {
            nextFireMode.Fire();
            weapon.StartCooldown();
        }
        
        currentCharge = 0f;
        chargeStarted = false;
    }


    private void ForEachParticleDo(Action<ParticleSystem> action) {
        for (int i = 0; i < chargeParticles.Length; i++) {
            action(chargeParticles[i]);
        }
    }
}
