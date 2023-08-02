using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChainBase), true)]
public class ChainCustomEditor : Editor {
    public override void OnInspectorGUI() { 
        DrawDefaultInspector();

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Ping")) {
            var monoBehaviour = (MonoBehaviour)target;
            var chain = monoBehaviour.GetComponent<ChainBase>();
            chain.DebugPing();
        }

        /*if (GUILayout.Button("Swell")) {
            var monoBehaviour = (MonoBehaviour)target;
            var chain = monoBehaviour.GetComponent<ChainBase>();
            chain.DebugSwell();
        }*/

        GUILayout.EndHorizontal();
    }
}
