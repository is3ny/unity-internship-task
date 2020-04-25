using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SawEnemy : MonoBehaviour
{

    Vector2 startPos;
    Vector2 targetPos;

    public Vector2 relativeDestination;
    public float period;   // in seconds

    float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + relativeDestination;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        time -= period * (float)Math.Floor(time / period); // taking modulo

        var halfPeriod = period / 2;
        var direction = targetPos - startPos;
        var progress = time / halfPeriod;
        if (progress < 1.0f)
        {
            transform.localPosition = startPos + progress * direction;
        }
        else
        {
            transform.localPosition = targetPos - (progress - 1.0f) * direction;
        }

        transform.localRotation *= Quaternion.Euler(0.0f, 0.0f, 10.0f);
    }
}
