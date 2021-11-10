using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ƹ�
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

    //Animation Event  ����
    public void SetOff()
    {
        if (targetPoint&&targetPoint.GetComponent<BombController>())
        targetPoint.GetComponent<BombController>().TurnOff();
       // targetPoint.GetComponent<BombController>()?.TurnOff();//�ǿ��жϣ����ú������һ��
        

    }

}
