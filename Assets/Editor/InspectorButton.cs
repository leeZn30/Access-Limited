using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueManager))]
public class InspectorButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueManager generator = (DialogueManager)target;


        if (GUILayout.Button("goLine"))
        {
            generator.goLine(DialogueManager.Instance.nowLine);
        }
    }
}
