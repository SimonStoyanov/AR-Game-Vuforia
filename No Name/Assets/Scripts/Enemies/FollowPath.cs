using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    private float curr_speed = 0;

    private List<GameObject> path = new List<GameObject>();
    private int curr_path_index = 0;

    public void SetPath(List<GameObject> _path)
    {
        curr_path_index = 0;
        path = _path;
    }

    public void Stop()
    {
        curr_speed = 0;
    }

    public void Move()
    {
        curr_speed = speed;
    }

    public void Start()
    {
        Move();
    }

    public void Update()
    {
        if(path.Count > 0)
        {
            if(path.Count > curr_path_index)
            {
                Vector3 target = path[curr_path_index].transform.position;

                Vector3 norm_direction = (target - transform.position).normalized;

                transform.position += norm_direction * speed * Time.deltaTime;

                if(Vector3.Distance(target, transform.position) < 2)
                {
                    ++curr_path_index;
                }
            }
        }
    }
}
