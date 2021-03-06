using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Snake))]
public class SnakeEditor : Editor {
   public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      var snake = ((Snake) target); 

      if (GUILayout.Button("Split")) {
         snake.SplitAt(snake.splitIndex);
      }
   }
}