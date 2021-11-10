using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    BoxCollider2D coll;

    
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();

        coll.enabled = false;
    }



    /// <summary>
    /// Game Manager 调用
    /// </summary>
    public void OpenDoor()
    {
        anim.Play("Opening");
        coll.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) {

            ///Game Manager 去到下一个房间
            ///
            GameManager.instance.NextLevel();

        }
        
    }



}
