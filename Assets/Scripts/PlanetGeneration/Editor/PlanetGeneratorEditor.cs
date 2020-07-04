using UnityEngine;
using UnityEditor;

namespace PlanetGeneration
{
    
   [CustomEditor(typeof(PlanetGenerator))]
   public class PlanetGeneratorEditor : Editor {
      public override void OnInspectorGUI() {
         base.OnInspectorGUI();
         PlanetGenerator gen = (PlanetGenerator)target;

         
         if (GUILayout.Button("Generate All")) {
            gen.GenerateFull();
         }

         if (GUILayout.Button("Setup")) {
            gen.Setup();
         }

         if (GUILayout.Button("Generate")) {
            gen.GenerateReal();
         }

         if (GUILayout.Button("GenerateScaled")) {
            gen.GenerateScaled();
         }

         if (GUILayout.Button("Sample")) {
            gen.Sample();
         }

         if (GUILayout.Button("Place Prefabs")) {
            gen.PlacePrefabs();
         }
      }
   }
}