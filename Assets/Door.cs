using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    bool opened = false;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        opened = true;
        anim.SetBool("Opened", true);
    }

    public bool IsOpen()
    {
        return opened;
    }
}
