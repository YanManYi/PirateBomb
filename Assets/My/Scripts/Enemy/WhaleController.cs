using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleController : Enemy, IDamageable
{


    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;

 

        }
        anim.SetTrigger("hit");

        if(transform.localScale.y>0.5f&&!isDead)
        transform.localScale /= 1.2f;

   
    }

    //Animation死亡动画的Event
    public void Bombs() {
        //死亡后吞的炸弹全部点燃激活
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
   

        Transform bombs = transform.parent.GetChild(3);
        int bombsNum = bombs.childCount;


        for (int i = 0; i < bombsNum; i++)
        {
            //bombs.GetChild(i).parent = null;
            bombs.GetChild(i).position = transform.position;
            bombs.GetChild(i).gameObject.SetActive(true);

            //力
            bombs.GetChild(i).GetComponent<Rigidbody2D>().AddForce((Random.Range(-5, 5) * Vector2.right*2 + Vector2.up * 10), ForceMode2D.Impulse);

            //点燃
            bombs.GetChild(i).GetComponent<BombController>().TurnOn();

          
        }
    }

    //animation 的Event
    public void Swalow() {//吞

        if (targetPoint.GetComponent<BombController>())
        {
            targetPoint.GetComponent<BombController>().TurnOff();

            health++;
            health = Mathf.Clamp(health++, 0, maxHealth);

            //   Destroy(targetPoint.gameObject);
            targetPoint.SetParent(transform.parent.GetChild(3));
            targetPoint.gameObject.SetActive(false);

        }

        if (transform.localScale.y < 1.5f)
            transform.localScale *= 1.2f;
    }


}
