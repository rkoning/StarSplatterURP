using UnityEngine;

public class Planet : MonoBehaviour {
   public Transform scaledChunkHolder;
   public Transform realChunkHolder;
   public Transform scaled { get; private set; }
   public Biome biome;
   private LocationContext locationContext;
}