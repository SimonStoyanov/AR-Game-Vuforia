﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    bool started = false;
    private float start_time = 0.0f;
    private float start_fixed_time = 0.0f;

    public void Start()
    {
        started = true;
        start_time = Time.timeSinceLevelLoad;
        start_fixed_time = Time.fixedUnscaledTime;
    }

    public float ReadTime()
    {
        if (started)
            return Time.timeSinceLevelLoad - start_time;
        else
            return 0.0f;
    }

    public float ReadFixedTime()
    {
        if (started)
            return Time.fixedUnscaledTime - start_fixed_time;
        else
            return 0.0f;
    }
}
