using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectProjector : Projector
{

    public GameObject projectilePrefab;
    public int maxProjectiles;
    protected int currentProjectile;
    protected Projectile[] projectiles;

    protected override void Start() {
        base.Start();
        projectiles = new Projectile[maxProjectiles];
        for (int i = 0; i < maxProjectiles; i++) {
            projectiles[i] = Instantiate(
                projectilePrefab,
                transform.position,
                transform.rotation,
                null
            ).GetComponent<Projectile>();

            projectiles[i].damage = weapon.damage;
            
            projectiles[i].gameObject.SetActive(false);
        }
    }

    public override void Emit() {
        base.Emit();
        projectiles[currentProjectile].gameObject.SetActive(true);
        projectiles[currentProjectile].Projector = this;
        projectiles[currentProjectile].transform.position = transform.position;
        projectiles[currentProjectile].transform.rotation = transform.rotation;
        projectiles[currentProjectile].OnEmit();
        currentProjectile++;
        currentProjectile = currentProjectile % maxProjectiles;
    }

    
    private void OnDestroy() {
        foreach (var p in projectiles) {
            Destroy(p.gameObject);
        }
    }
}
