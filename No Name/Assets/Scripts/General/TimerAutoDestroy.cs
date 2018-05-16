using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerAutoDestroy : MonoBehaviour
{
    [SerializeField] private float destroy_time = 20.0f;

    private Timer timer = new Timer();

    private void Start()
    {
        timer.Start();
    }

    void Update ()
    {
        if (timer.ReadTime() > destroy_time)
            Destroy(gameObject);
	}
}
