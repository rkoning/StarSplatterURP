public interface ICameraBehaviour
{
   void GrantControl(CameraController cameraController);
   void RemoveControl();

   bool IsActiveCamera();
}