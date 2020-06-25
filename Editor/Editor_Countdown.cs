using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CountDown))]
public class Editor_Countdown : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Place numbers in ascending order!", MessageType.Info);

        base.OnInspectorGUI();
    }
}
