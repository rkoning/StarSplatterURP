using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float orbitSpeed;
    public float rotationSpeed;
    public Transform origin;

    private void Update() {
        transform.RotateAround(origin.position, origin.up, orbitSpeed * Time.deltaTime);
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }    
}
