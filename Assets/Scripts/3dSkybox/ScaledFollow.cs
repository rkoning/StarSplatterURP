using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaledFollow : MonoBehaviour
{
    public Transform parent;
    public LocationContext currentLocation;
    
    private void Update() {
        if (currentLocation) {
            transform.position = currentLocation.GetScaledPoint(parent.position);
            transform.rotation = currentLocation.GetScaledRotation(parent);
        } else {
            transform.position = parent.position / OriginManager.skyboxScale;
            transform.rotation = parent.rotation;
        }
    }
}
