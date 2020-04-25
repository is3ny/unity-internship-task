using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool onGround = false;
    bool changed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
    public bool HasChanged()
    {
        if (changed)
        {
            changed = false;
            return true;
        }

        return true;
    }*/

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Debug.Log("Got " + other.contactCount + " contacts.");
            ContactPoint2D[] contacts = { };
            other.GetContacts(contacts);
            for (int ci = 0; ci < other.contactCount; ci++) {
                Debug.Log("Normal: " + other.GetContact(ci).normal.ToString());
                if (other.GetContact(ci).normal == Vector2.up)
                {
                    onGround = true;
                    changed = true;
                    transform.parent.GetComponent<PlayerBehavior>().GroundUpdate();
                    Debug.Log("On ground.");
                    break;
                }
            }
        }
        /*
        var normal = other.GetContact(0).normal;
        if (normal == Vector2.right || normal == Vector2.left) {
            rb.AddForce(transform.right * rb.velocity.x, ForceMode2D.Force);
        }*/
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            onGround = false;
            Debug.Log("Not on ground.");
        }
    }
}
