using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleProjector : Projector
{

    private ParticleSystem emitter;
    public int numParticles = 1;

    protected override void Start() {
        base.Start();
        emitter = GetComponent<ParticleSystem>();
    }
    public override void Emit() {
        base.Emit();
        if (emitter != null) 
            emitter.Emit(numParticles);
    }

    private void OnParticleCollision(GameObject other) {
        DealDamage(other);
    }
}
