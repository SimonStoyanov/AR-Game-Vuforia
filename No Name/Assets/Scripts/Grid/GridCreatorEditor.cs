﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GridCreator))]
public class GridCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridCreator grid_creator = (GridCreator)target;
        if (GUILayout.Button("Create Grid"))
        {
            grid_creator.CreateGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            grid_creator.ClearGrid();
        }

        if (GUILayout.Button("Finish Grid"))
        {
            grid_creator.FinishGrid();
        }
    }
}
#endif