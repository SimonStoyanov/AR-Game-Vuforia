using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager
{
    private List<Path> paths = new List<Path>();

    public class Path
    {
        public Path(string name, List<GameObject> path)
        {
            path_name = name;
            gos = path;
        }

        public string GetName()
        {
            return path_name;
        }

        public List<GameObject> GetPathList()
        {
            return gos;
        }

        public Vector3 GetStartPos()
        {
            Vector3 ret = Vector3.zero;

            if (gos.Count > 0)
            {
                ret = gos[0].transform.position;
            }

            return ret;
        }

        private string path_name;
        List<GameObject> gos = new List<GameObject>();
    }

    public List<Path> GetPaths()
    {
        return paths;
    }

    public Path GetPathByPathName(string name)
    {
        Path ret = null;

        for(int i = 0; i < paths.Count; ++i)
        {
            if(paths[i].GetName() == name)
            {
                ret = paths[i];
                break;
            }
        }

        return ret;
    }

    public Path GetCloserPath(Vector3 pos)
    {
        Path ret = null;

        float min_distance = float.PositiveInfinity;
        for(int i = 0; i < paths.Count; ++i)
        {
            float curr_distance = Vector3.Distance(paths[i].GetStartPos(), pos);

            if (curr_distance < min_distance)
            {
                ret = paths[i];
                min_distance = curr_distance;
            }
        }

        return ret;
    }

    public void InitPaths()
    {
        PathInstance[] creator_paths= Object.FindObjectsOfType<PathInstance>();

        for(int i = 0; i < creator_paths.Length; ++i)
        {
            List<GameObject> path = creator_paths[i].GetPath();
            string path_name = creator_paths[i].GetName();

            Path p = new Path(path_name, path);

            paths.Add(p);
        }
    }
}
