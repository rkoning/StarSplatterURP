using UnityEngine;

public class AsteroidGenerator : MonoBehaviour {
   public MeshGenerator generator;
   public AsteroidFieldDensity density;

   public Material asteroidMaterial;
   public void Generate() {
      generator.mat = asteroidMaterial;
      generator.generateColliders = true;
      generator.Run();
   }
}