using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    public Camera MainCamera {
        get { return mainCamera; }
        set { mainCamera = value; }
    }

    private Transform cameraTransform;
    public Transform CameraTransform {
        get { return cameraTransform; }
        set { cameraTransform = value; }
    }

    private Player player;
    public Player Player {
        get { return player; }
        set { player = value; }
    }


    private ICameraBehaviour current;

    public ICameraBehaviour SetCameraBehaviour(ICameraBehaviour cameraBehaviour) {
        if (current != null) {
            current.RemoveControl();
        }
        cameraBehaviour.GrantControl(this);
        current = cameraBehaviour;
        return current;
    }
}
