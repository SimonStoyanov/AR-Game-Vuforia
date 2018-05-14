using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathCreator : MonoBehaviour
{
    private GameObject curr_parent = null;

    public void Awake()
    {
        curr_parent = GameObject.FindGameObjectWithTag("Path");
    }

    public void CreatePath()
    {
        ClearPath();

        curr_parent = new GameObject();
        curr_parent.name = "Path";
        curr_parent.tag = "Path";

        curr_parent.AddComponent<PathInstance>();
    }

    public void CreatePathNode()
    {
        if(curr_parent != null)
        {
            GameObject child = new GameObject();
  
            child.transform.parent = curr_parent.transform;

            child.name = "Node: " + curr_parent.transform.childCount;

            PathInstance instance = curr_parent.GetComponent<PathInstance>();

            if(instance != null)
                instance.CreatePath(); 
        }
    }

    public void ClearPath()
    {
        curr_parent = GameObject.FindGameObjectWithTag("Path");
        DestroyImmediate(curr_parent);
    }

    public void FinishPath()
    {
        curr_parent = GameObject.FindGameObjectWithTag("Path");
        curr_parent.name = "FinishedPath";
        curr_parent.tag = "FinishedPath";
    }

    public bool HasPath()
    {
        return curr_parent != null;
    }
}
