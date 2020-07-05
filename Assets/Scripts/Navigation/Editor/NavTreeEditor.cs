using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Navigation {
    [CustomEditor(typeof(NavTreeAnchor))]
    public class NavTreeEditor : Editor
    {
        override public void OnInspectorGUI() {
            base.DrawDefaultInspector();
            if (GUILayout.Button("Build and Save")) {
                ((NavTreeAnchor) target).BuildAndSave();
            }
        }
    }
}
