using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuyController : Enemy,IDamageable
{

    public float powerForce;//力
    private  Transform pickuoPoint;//炸弹拿起来后新的位置点

    private void Start()
    {
        pickuoPoint = transform.GetChild(3);
    }

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



    //animation 的Event

        /// <summary>
        /// 捡起炸弹
        /// </summary>
    public void PickUpBomb() {

        if (targetPoint.CompareTag("Bomb") && !hasBomb)
        {

            targetPoint.gameObject.transform.position = pickuoPoint.position;

            targetPoint.SetParent(pickuoPoint);//给炸弹设置父级，这样子跟着走

            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            hasBomb = true;
            Invoke("_HasBomb", 1);
        }
        else TransitionToState(patrolState);
    

    }

    /// <summary>
    /// 解决播放帧动画时候，无法播放仍炸弹，因为炸弹提前消失了
    /// </summary>
    void _HasBomb() {

        //有问题，在爆炸中，炸弹提前消失，没办法恢复到hasbomb=false,所以在捡起炸弹的时候就延迟一秒恢复物理效果
        hasBomb = false;//当扔出去的时候，就直接恢复物理学
    }

    //animation 的Event
    //扔炸弹
    public void ThrowBomb() {
        if (targetPoint)
        {

            if (hasBomb)
            {

                if (targetPoint.GetComponent<Rigidbody2D>())

                    targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                targetPoint.SetParent(null);


            }


            if (FindObjectOfType<PlayerController>().gameObject.transform.position.x - transform.position.x > 0)
            {
                if (targetPoint.GetComponent<Rigidbody2D>())
                    targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * powerForce, ForceMode2D.Impulse);

            }
            else
            {
                if (targetPoint.GetComponent<Rigidbody2D>())
                    targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * powerForce * 2, ForceMode2D.Impulse);

            }

        }

        //  hasBomb = false;//当扔出去的时候，恢复物理学


    }




}
