using UnityEditor;
using UnityEngine;

namespace AI.Factions {
   
   [CustomEditor(typeof(MinorFaction))]
   public class MinorFactionEditor : Editor {
      public override void OnInspectorGUI() {
         base.OnInspectorGUI();
         MinorFaction mf = (MinorFaction)target;

         if (GUILayout.Button("Run AttackTargetObjective")) {
            mf.SetObjective(new AttackTargetObjective(mf.target));
         }

         if (GUILayout.Button("Run DefendLocationObjective")) {
            mf.SetObjective(new DefendLocationObjective(mf.targetLocation));
         }
      }
   }
}