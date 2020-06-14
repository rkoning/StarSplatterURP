using UnityEngine;

public class Player : MonoBehaviour {
   public PlayerInput input;
   public PlayerFighter ship;
   public new PlayerCamera camera;
   public CameraController cameraController;

   private void Awake() {
      camera.Target = ship.transform;
      cameraController.Player = this;
      cameraController.MainCamera = camera.camera;
      cameraController.SetCameraBehaviour(camera);
   }
}