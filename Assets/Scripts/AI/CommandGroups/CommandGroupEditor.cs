using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using AI.Factions;

namespace AI.CommandGroups {
   [CustomEditor(typeof(CommandGroup))]
   public class CommandGroupDebug : Editor {
      Vector3 point;
      float threshold = 100f;

      int patrolPointsCount;

      public override void OnInspectorGUI() {
         base.OnInspectorGUI();
         CommandGroup cg = (CommandGroup)target;
         
         EditorGUILayout.Toggle(cg.Completed());

         point = EditorGUILayout.Vector3Field("Point", point);
         threshold = EditorGUILayout.FloatField("Threshold", threshold);
         if (GUILayout.Button("Run move to point command")) {
            cg.SetCurrentCommand(new MoveToPointCommand(point, threshold));
         }

         Vector3[] patrolPoints = new Vector3[] {new Vector3(0, 0, 400),new Vector3(-400, 0, 0),new Vector3(0, 0, -400), new Vector3(400, 0, 0)};
         if (GUILayout.Button("Run patrol command")) {
            cg.SetCurrentCommand(new PatrolCommand(patrolPoints, threshold));
         }

         if (GUILayout.Button("Run attack target command")) {
            cg.SetCurrentCommand(new AttackTargetCommand(cg.targetTransform));
         }

         if (GUILayout.Button("Run follow target command")) {
            cg.SetCurrentCommand(new FollowTargetCommand(cg.targetTransform, threshold));
         }

         if (GUILayout.Button("Run deliver cargo command")) {
            cg.SetCurrentCommand(new DeliverCargoCommand(cg.pickUp.position, cg.dropOff.position, threshold));
         }

         if (GUILayout.Button("Run defend target command")) {
            cg.SetCurrentCommand(new DefendTargetCommand(cg.targetTransform, 200f));
         }
      }
   }
}