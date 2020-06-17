using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginManager : MonoBehaviour
{
    public static OriginManager instance;

    public FloatingOrigin floatingOrigin;

    public SkyboxCamera skyboxCamera;
    public Sunlight sunlight;

    public static float skyboxScale = 100f;
    public float scale;

    public GameObject skybox;

    public Player player;
        
    private void Awake() {
        OriginManager.instance = this;
        OriginManager.skyboxScale = scale;
    }

    public void EnterLocation(LocationContext locationContext) {
        floatingOrigin.enabled = false;

        // move player to relative location
        Vector3 playerPosition = (skyboxCamera.transform.position - locationContext.transform.position) * OriginManager.skyboxScale;
        floatingOrigin.SetPosition(player.ship.transform, playerPosition);

        // move skybox such that this location is the new 0,0,0
        Vector3 skyboxOrigin = locationContext.transform.position;
        Vector3 shiftedSkyboxPosition = (skybox.transform.position - skyboxOrigin);
        skybox.transform.position = shiftedSkyboxPosition;        
    }

    public void ExitLocation(LocationContext locationContext) {

        // move player to relative location
        Vector3 playerPosition = (skyboxCamera.transform.position - locationContext.transform.position) * scale;

        floatingOrigin.SetPosition(player.ship.transform, playerPosition);
        
        // reset skybox position
        skybox.transform.position = Vector3.zero;
    }
}
