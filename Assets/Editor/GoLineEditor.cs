using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueManager), true)]

public class GoLineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueManager generator = (DialogueManager)target;


        if (GUILayout.Button("goLine"))
        {
            generator.goTurn(DialogueManager.Instance.nowTurn);
        }
    }
}
