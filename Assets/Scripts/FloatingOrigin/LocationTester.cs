using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTester : MonoBehaviour
{
    public Transform scaled;
    public LocationContext context;

    private void Update() {
        scaled.position = context.GetScaledPoint(transform.position);
        scaled.rotation = context.GetScaledRotation(transform);
    }
}
