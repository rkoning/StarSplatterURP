using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginManager : MonoBehaviour {

    public static OriginManager instance;

    public FloatingOrigin floatingOrigin;

    public SkyboxCamera skyboxCamera;
    public Sunlight sunlight;

    public static float skyboxScale = 100f;
    public float scale;

    public GameObject skybox;

    public Transform viewer;

    private void Awake() {
        OriginManager.instance = this;
        OriginManager.skyboxScale = scale;
    }

    public void EnterLocation(LocationContext locationContext) {
        floatingOrigin.enabled = false;

        // move player to relative location
        Vector3 playerPosition = locationContext.scaled.InverseTransformPoint(skyboxCamera.transform.position) * OriginManager.skyboxScale;
        Quaternion playerRotation = Quaternion.Inverse(skyboxCamera.transform.rotation) * Quaternion.Inverse(locationContext.scaled.rotation);
        floatingOrigin.SetPosition(viewer, playerPosition, playerRotation);

        // move skybox such that this location is the new 0,0,0
        Vector3 skyboxOrigin = locationContext.scaled.position;
        Vector3 shiftedSkyboxPosition = (skybox.transform.position - skyboxOrigin);
        skybox.transform.position = shiftedSkyboxPosition;
        skyboxCamera.currentLocation = locationContext;

        sunlight.currentLocation = locationContext;
    }

    public void ExitLocation(LocationContext locationContext) {
        Quaternion playerRotation = skyboxCamera.transform.rotation;
        // move player to relative location
        skybox.transform.position = Vector3.zero;

        Vector3 playerPosition = (locationContext.scaled.rotation * viewer.position);
        viewer.position = playerPosition;
        viewer.rotation = playerRotation;
        // floatingOrigin.SetPosition(viewer, playerPosition, playerRotation);
        
        // reset skybox position

        skyboxCamera.currentLocation = null;
        floatingOrigin.enabled = true;

        sunlight.currentLocation = null;
    }
}
