using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathInstance : MonoBehaviour
{
    [SerializeField] private string name;

    List<GameObject> path = new List<GameObject>();

    public void CreatePath()
    {
        path.Clear();

        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject curr_go = transform.GetChild(i).gameObject;

            path.Add(curr_go);
        }
    }

    public string GetName()
    {
        return name;
    }

    public List<GameObject> GetPath()
    {
        return path;
    }

    private void DrawPath()
    {
        for (int i = 0; i < path.Count; ++i)
        {
            if (i + 1 < path.Count)
            {
                Vector3 start = path[i].transform.position;
                Vector3 end = path[i + 1].transform.position;

                Debug.DrawLine(start, end, Color.red);
            }
        }
    }

    private void Awake()
    {
        CreatePath();
    }

    void Update()
    {
        DrawPath();
    }
}
