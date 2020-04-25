using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{

    public Rigidbody2D projectile;
    public Vector2 emissionVector;
    public float projectileSpeed;
    public float t1;
    public float t2;

    float time = 0.0f;
    float nextAt = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        nextAt = Next();
        emissionVector.Normalize();
    }

    float Next()
    {
        return Random.Range(t1, t2);
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= nextAt)
        {
            var p = Instantiate(projectile, transform.position, transform.rotation);
            p.velocity = emissionVector * projectileSpeed;
            nextAt = Next();
            time = 0.0f;
        }
    }
}
