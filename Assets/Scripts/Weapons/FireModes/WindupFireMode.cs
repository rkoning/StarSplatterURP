using System;
using System.Collections;
using UnityEngine;

public class WindupFireMode : FireMode
{
   public float windupTime = 1f;

   private Coroutine windup;
   public GameObject windupParticlePrefab;
   public AudioSource windupSound;

   private ParticleSystem[] windupParticles;

   public FireMode nextFireMode;

   private void Start() {
      windupParticles = new ParticleSystem[weapon.Projectors.Count];
      for(int i = 0; i < weapon.Projectors.Count; i++) {
         var part = GameObject.Instantiate(windupParticlePrefab, weapon.Projectors[i].transform).GetComponent<ParticleSystem>();
         windupParticles[i] = part;
      } 
   }
    
   public override void Fire() {
      if (windup == null) {
         ForEachParticleDo((ParticleSystem p) => { p.Play(); });
         windupSound.Play();
         windup = StartCoroutine(WaitThenFire(windupTime));
      }
   }

    private IEnumerator WaitThenFire(float duration) {
      yield return new WaitForSeconds(duration);
      ForEachParticleDo((ParticleSystem p) => { p.Stop(); });
      windupSound.Stop();
      nextFireMode.Fire();
      weapon.StartCooldown();
      windup = null;
    }

    public override void Hold() {
        return;
    }

    public override void Release() {
       return;
    }


    private void ForEachParticleDo(Action<ParticleSystem> action) {
        for (int i = 0; i < windupParticles.Length; i++) {
            action(windupParticles[i]);
        }
    }
}
