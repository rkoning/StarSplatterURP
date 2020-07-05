using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FactionDebugger))]
public class FactionDebuggerEditor : Editor {
   public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      
   }
}