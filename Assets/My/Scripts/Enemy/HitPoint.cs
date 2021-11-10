using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    private  int dir;//力方向
    public bool bombAvilable;//是否可以对炸弹有作用,比如说踢开它，使其移动，光头怪单独使用
    

    public float damage;//攻击伤害
    public float addForce;//踢开的力


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //在右侧的时候给左力
        if (transform.position.x > collision.transform.position.x)
            dir = -1;
        else dir = 1;

        if (collision.CompareTag("Player")) {

            collision.GetComponent<IDamageable>().GetHit(damage);//碰到主角，就调用接口统一的扣血方法
        
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1f) * addForce*0.2f, ForceMode2D.Impulse);
        }
        
        //踢炸弹
        if (collision.CompareTag("Bomb")&&bombAvilable)
        {
          
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2 (dir,1)*addForce,ForceMode2D.Impulse);
        }
    }
}
