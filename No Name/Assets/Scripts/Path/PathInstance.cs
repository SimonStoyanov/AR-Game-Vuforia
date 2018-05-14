using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathInstance : MonoBehaviour
{
    [SerializeField]

    List<GameObject> path = new List<GameObject>();

    void CreatePath()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            GameObject curr_go = transform.GetChild(i).gameObject;
            
            path.Add(curr_go);   
        }
    }
}
