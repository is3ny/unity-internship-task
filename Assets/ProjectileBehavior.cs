using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float timeout;
    float time = 0.0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Ground")
            || other.gameObject.tag.Equals("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time >= timeout)
        {
            Destroy(this.gameObject);
        }
    }
}
