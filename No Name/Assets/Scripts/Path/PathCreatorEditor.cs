using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathCreator path_creator = (PathCreator)target;
        if (GUILayout.Button("Create Path"))
        {
            path_creator.CreatePath();
        }

        if (GUILayout.Button("Clear Path"))
        {
            path_creator.ClearPath();
        }

        if (path_creator.HasPath())
        {
            if(GUILayout.Button("Create Node"))
            {
                path_creator.CreatePathNode();
            }
        }

        if (GUILayout.Button("Finish Path"))
        {
            path_creator.FinishPath();
        }
    }
}
#endif
