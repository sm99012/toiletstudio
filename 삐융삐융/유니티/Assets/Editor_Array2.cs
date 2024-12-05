using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Editor_Array2 : Editor
{
    int[,] debug2d;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("2차원 배열");

        debug2d = new int[3, 3];

        for (int i = 0; i < 3; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int k = 0; k < 3; k++)
                debug2d[i, k] = EditorGUILayout.IntField(debug2d[i, k]);
            EditorGUILayout.EndHorizontal();
        }
    }
}
