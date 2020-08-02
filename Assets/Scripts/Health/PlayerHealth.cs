using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LightHealth
{
    private Rigidbody rb;

    public LayerMask damagingLayers;

    private Player player;

    protected override void Start() {
        base.Start();
        player = GetComponent<PlayerFighter>().player;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        if (damagingLayers == (damagingLayers | 1 << other.gameObject.layer)){
            rb.AddForce(other.GetContact(0).normal * Mathf.Clamp(other.impulse.sqrMagnitude, 2000, 10000));
            TakeDamage(maxHealth / 2f, false);
        }
    }

    public override void Die() {
        player.RespawnAfter(5f);
        base.Die();
    }
}
