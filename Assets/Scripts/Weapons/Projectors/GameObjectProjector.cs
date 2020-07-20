using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectProjector : Projector
{

    public GameObject projectilePrefab;
    public int maxProjectiles;

    protected override void Start() {
        base.Start();
        ObjectPool.Register(projectilePrefab.name, projectilePrefab, maxProjectiles);
    }

    public override void Emit() {
        base.Emit();
        var projectile = ObjectPool.Create(projectilePrefab.name, transform.position, transform.rotation, null).GetComponent<Projectile>();
        projectile.Projector = this;
        projectile.OnEmit();
    }
}
