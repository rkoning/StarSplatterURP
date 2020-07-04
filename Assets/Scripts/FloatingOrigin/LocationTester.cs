using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTester : MonoBehaviour
{
    public PlayerInput input;
    public float speed = 10f;
    private void Update() {
        if (input.PrimaryFireHeld) {
            transform.position += transform.forward * Time.deltaTime * speed;
        } else if (input.SecondaryFireHeld) {
            transform.position -= transform.forward * Time.deltaTime * speed;
        }
    }
}
