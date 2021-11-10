using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//黄瓜
public class CucumberController :Enemy,IDamageable
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
    }

    //Animation Event  吹灭
    public void SetOff()
    {
        if (targetPoint&&targetPoint.GetComponent<BombController>())
        targetPoint.GetComponent<BombController>().TurnOff();
       // targetPoint.GetComponent<BombController>()?.TurnOff();//非空判断，作用和上面的一样
        

    }

}
