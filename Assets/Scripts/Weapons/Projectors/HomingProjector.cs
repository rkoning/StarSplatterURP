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
        var projectile = ObjectPool.Create(projectilePrefab.name, transform.position, transform.rotation, null).GetComponent<Projectile>();
        projectile.Projector = this;
        projectile.OnEmit();
        projectile.SetTarget(pilot.target.transform);
    }
}
