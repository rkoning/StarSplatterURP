using UnityEngine;

public class Sunlight : MonoBehaviour {
   public Light skyboxLight;
   public Light realLight;

   public Transform scaledViewer;
   public LayerMask scaledLayers;

   public float defaultIntensity = 4f;
   public float intensityScale = 0.01f;

   public LocationContext currentLocation;

   private void Start() {
      realLight.color = skyboxLight.color;
      realLight.colorTemperature = skyboxLight.colorTemperature;
      realLight.intensity = defaultIntensity;
   }

   private void LateUpdate() {
      Vector3 direction = scaledViewer.transform.position;
      direction.x -= skyboxLight.transform.position.x;
      direction.y -= skyboxLight.transform.position.y;
      direction.z -= skyboxLight.transform.position.z;
      if (!currentLocation) {
         realLight.transform.forward = direction.normalized;
         realLight.intensity = defaultIntensity - direction.sqrMagnitude * intensityScale;
      } else {
         realLight.transform.forward = Quaternion.Inverse(currentLocation.transform.rotation) * direction.normalized;
         realLight.intensity = defaultIntensity - direction.sqrMagnitude * intensityScale;
      }
   }
}