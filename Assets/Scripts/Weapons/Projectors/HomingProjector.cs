using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjector : GameObjectProjector
{
    protected Ship pilot;

    protected override void Start() {
        pilot = GetComponentInParent<Ship>();
        base.Start();
    }

    public override void Emit() {
        base.Emit();
        projectiles[currentProjectile].gameObject.SetActive(true);
        projectiles[currentProjectile].Projector = this;
        projectiles[currentProjectile].transform.position = transform.position;
        projectiles[currentProjectile].transform.rotation = transform.rotation;
        projectiles[currentProjectile].OnEmit();
        projectiles[currentProjectile].SetTarget(pilot.target.transform);
        currentProjectile++;
        currentProjectile = currentProjectile % maxProjectiles;
    }
}
