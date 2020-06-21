using UnityEngine;

namespace PlanetGeneration
{
   
   [CreateAssetMenu(fileName = "Biome", menuName = "LWRPTest2/Biome", order = 0)]
   public class Biome : ScriptableObject {
      [Header("Noise Params")]
      public int minOctaves = 2;
      public int maxOctaves = 3;

      public float minLacunarity = 0.9f;
      public float maxLacunarity = 1.2f;

      public float minPersistence = 2.0f;
      public float maxPersistence = 2.3f;
      
      public float minNoiseScale = 1.9f;
      public float maxNoiseScale = 2.1f;

      public float minNoiseWeight = 2;
      public float maxNoiseWeight = 2.2f;

      
      public float minWeightMultipiler = 1;
      public float maxWeightMultipiler = 2;

      public float minRadius = 200;
      public float maxRadius = 300;
      public Material material;

      public float minTextureOffset = 200f;
      public float minMaxTextureOffset = 1000f;

      [Header("Rings")]
      public float ringsChance = 1f;

      public Material[] ringMaterials;

      [Header("Water")]
      public float waterChance = 1f;
      public Material[] waterMaterials;

      public float waterHeight = 1.2f;

      [Header("Atmosphere")]      
      public float atmosphereChance = 1f;
      public Material[] atmosphereMaterials;

      [Header("Prefabs")]
      public DepositPrefab[] prefabs;
   }

   [System.Serializable]
   public class DepositPrefab {
      public GameObject prefab;

      public float rarity;
   }
}