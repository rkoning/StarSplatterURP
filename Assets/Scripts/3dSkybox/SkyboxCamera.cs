using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCamera : MonoBehaviour
{
    public Transform parentCameraTransform;
    public Transform skyboxCameraTransform;
    public float scale;

    public LocationContext currentLocation;
    
    private void Update() {
        if (currentLocation) {
            transform.position = currentLocation.GetScaledPoint(parentCameraTransform.position);
            transform.rotation = currentLocation.GetScaledRotation(parentCameraTransform);
        } else {
            transform.position = parentCameraTransform.position / scale;
            transform.rotation = parentCameraTransform.rotation;
        }
    }
}
