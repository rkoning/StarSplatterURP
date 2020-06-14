using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCamera : MonoBehaviour
{
    public Transform parentCameraTransform;
    public Transform skyboxCameraTransform;
    public float scale;
    
    private void LateUpdate() {
        transform.position = parentCameraTransform.position / scale;
        transform.rotation = parentCameraTransform.rotation;
    }
}
