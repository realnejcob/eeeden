using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(SnapToLayer))]
public class SnapToLayerEditor : Editor {
    public override void OnInspectorGUI() { 
        DrawDefaultInspector();
        if (GUILayout.Button("Snap!")) {
            var monoBehaviour = (MonoBehaviour)target;
            var snapToLayer = monoBehaviour.GetComponent<SnapToLayer>();
            snapToLayer.Snap();
        }
    }
}
