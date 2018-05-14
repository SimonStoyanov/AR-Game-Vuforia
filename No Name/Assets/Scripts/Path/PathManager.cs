using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager
{
    private List<List<GameObject>> paths = new List<List<GameObject>>();

    public List<List<GameObject>> GetPaths()
    {
        return paths;
    }

    public void InitPaths()
    {
        PathInstance[] creator_paths= Object.FindObjectsOfType<PathInstance>();

        for(int i = 0; i < creator_paths.Length; ++i)
        {
            List<GameObject> path = creator_paths[i].GetPath();

            paths.Add(path);
        }
    }
}
