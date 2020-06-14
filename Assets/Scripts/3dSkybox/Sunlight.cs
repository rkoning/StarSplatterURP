using UnityEngine;

public class Sunlight : MonoBehaviour {
   public Light skyboxLight;
   public Light realLight;

   public Transform scaledViewer;
   public LayerMask scaledLayers;

   public float defaultIntensity;
   public float shadowIntensity;

   private void Start() {
      realLight.color = skyboxLight.color;
      realLight.colorTemperature = skyboxLight.colorTemperature;
      realLight.intensity = defaultIntensity;
   }

   private void Update() {
      Vector3 direction = scaledViewer.transform.position;
      direction.x -= skyboxLight.transform.position.x;
      direction.y -= skyboxLight.transform.position.y;
      direction.z -= skyboxLight.transform.position.z;

      realLight.transform.rotation = Quaternion.LookRotation(direction);
      if (Physics.Raycast(skyboxLight.transform.position, direction, float.MaxValue, scaledLayers)) {
         realLight.intensity = shadowIntensity;
      } else {
         realLight.intensity = defaultIntensity;
      }
   }
}