using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerAutoDisable : MonoBehaviour
{
    [SerializeField] private float disable_time = 4.0f;

    private Timer timer = new Timer();

    private void Start()
    {
        timer.Start();
    }

    private void OnEnable()
    {
        timer.Start();
    }

    private void Update ()
    {
        Debug.Log(timer.ReadFixedTime());

        if (timer.ReadFixedTime() > disable_time)
        {
            gameObject.SetActive(false);
        }
	}
}
