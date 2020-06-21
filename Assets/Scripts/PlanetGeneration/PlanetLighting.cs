using UnityEngine;

public class PlanetLighting : MonoBehaviour {
   public Light mainLight;
   public Color dayLightColor;
   public Color nightLightColor;

   public float dayIntensity;
   public float nightIntensity;

   public float transitionLength = 1e10f;

   public Transform sunLocation;

   public float orbitRadius = 5000;
   public float orbitSpeed = 0.5f;

   private float angle;
   public Transform player;

   void Update()
   {
      // Light direction is always from the player transform's location
      Vector3 playerDirection = transform.position - player.position;
      mainLight.transform.rotation = Quaternion.LookRotation(playerDirection);

      float dayNightSide = (Vector3.Dot(playerDirection, transform.position - sunLocation.position)) / transitionLength;
      dayNightSide = (Mathf.Clamp(dayNightSide, -1, 1) + 1) / 2;
      mainLight.color = Color.Lerp(nightLightColor, dayLightColor, dayNightSide);
      mainLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, dayNightSide);
   }

   void FixedUpdate()
   {
      angle += orbitSpeed * Time.fixedDeltaTime;
      var offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
      sunLocation.position = offset * orbitRadius + transform.position;
      sunLocation.rotation = Quaternion.LookRotation(transform.position - sunLocation.position);
   }
}