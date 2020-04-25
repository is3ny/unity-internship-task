using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public float INSTANT_JUMP_VELOCITY = 14f;
    public float JUMP_SUPPRESSION = 0.5f;

    bool gotCoin = false;
    public int coinCount = 0;

    GroundChecker groundChecker;
    JumpState jumping = JumpState.Grounded;

    bool dying = false;

    Door door;

    enum JumpState
    {
        Grounded,
        Raising,
        Falling
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        door = GameObject.Find("Door").GetComponent<Door>();
        Debug.Log("Door is null: " + (door == null).ToString());

        Debug.Log(transform.childCount);
        groundChecker = transform.GetChild(0).GetComponent<GroundChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && groundChecker.onGround) {
            JumpInstantly();
        }

        if (!Input.GetButton("Jump") && jumping == JumpState.Raising) {
            if (rb.velocity.y > 0) {
                rb.AddForce(transform.up * (-rb.velocity.y * JUMP_SUPPRESSION), ForceMode2D.Impulse);
                jumping = JumpState.Falling;
            }
        }

        if (rb.velocity.y < 0) {
            jumping = JumpState.Falling;
        }

        if (dying)
        {
            // Make it impossible to change the animation
            //Debug.Log("Dying!!!");
        }
        else if (jumping == JumpState.Grounded)
        {
            if (Input.GetAxis("Horizontal") == 0.0)
            {
                anim.SetInteger("ID", 1);
            }
            else
            {
                SetAppropriateXFlip();
                anim.SetInteger("ID", 2);
            }
        } 
        else if (jumping == JumpState.Raising)
        {
            anim.SetInteger("ID", 3);
        } 
        else if (jumping == JumpState.Falling)
        {
            anim.SetInteger("ID", 4);
        }
    }

    public void GroundUpdate()
    {
        if (groundChecker.onGround)
            jumping = JumpState.Grounded;
        else
            jumping = JumpState.Raising;
    }

    public void OnBecameInvisible()
    {
        StartCoroutine("Die");
    }

    void SetAppropriateXFlip()
    {
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);

        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 12f, rb.velocity.y);
    }

    void JumpInstantly()
    {
        jumping = JumpState.Raising;
        rb.AddForce(transform.up * INSTANT_JUMP_VELOCITY, ForceMode2D.Impulse);
    }

    void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (String.Equals(other.gameObject.tag, "Coin") && !gotCoin)
        {
            Debug.Log("Got coin " + other.gameObject.name + " via " + this.gameObject.name);
            other.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Destroy(other.gameObject);
            coinCount++;
            gotCoin = true;
        }

        if (other.gameObject.tag == "Key" && !door.IsOpen())
        {
            Destroy(other.gameObject);
            door.Open();
        }

        if (other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Bump");
            StartCoroutine("Die");
        }

        if (other.gameObject.tag.Equals("Door") && door.IsOpen())
        {
            Debug.Log("Yes!!");
            Application.LoadLevel("Victory");
        }
    }

    IEnumerator Die()
    {
        if (dying)
        {
            yield break;
        }

        dying = true;

        // Prevent player from moving
        rb.isKinematic = false;
        rb.simulated = false;

        var deathHash = Animator.StringToHash("Base Layer.Death");

        anim.CrossFadeInFixedTime(deathHash, 0.6f);

        while (anim.GetCurrentAnimatorStateInfo(0).fullPathHash != deathHash)
        {
            yield return null;
        }

        Debug.Log("It's my time!!!");
        float timeElapsed = 0;
        float waitTime = anim.GetCurrentAnimatorStateInfo(0).length;

        anim.SetInteger("ID", 6);

        //Now, Wait until the current state is done playing
        while (timeElapsed < (waitTime))
        {
            Debug.Log("Go!!!");
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        ReloadLevel();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (String.Equals(other.gameObject.tag, "Coin"))
        {
            gotCoin = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

    }

    void OnCollisionExit2D(Collision2D other)
    {
        /*
        if (other.gameObject.tag == "Ground"
            && other.otherCollider == groundChecker)
        {
            onGround = false;
            Debug.Log("Not on ground.");
        }*/
    }

    void PickUpCoin(GameObject coin)
    {
        //Destroy(coin);
        //Debug.Log("Coin!");
        //coinCount++;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(6, 6, 100, 30), "Coins: " + coinCount);
    }
}
