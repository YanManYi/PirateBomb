using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//粉色队长
public class CaptainController : Enemy, IDamageable
{
    SpriteRenderer sprite;

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


    //重写父类的赋值额外的个体需要
    public override void Init()
    {
        base.Init();

        sprite = GetComponent<SpriteRenderer>();
    }

    //重写方法
    public override void SkillAction()
    {
        base.SkillAction();

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("skill"))//如果该动画在播放的情况下
     
        {
            sprite.flipX = true;//改个反向

            if (transform.position.x > targetPoint.position.x)//自身在炸弹目标点的右边
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left, speed * 2 * Time.deltaTime);

            }
        }
        else { 
            //如果突然爆炸消失了，就不会改成需要检测
            sprite.flipX = false;
          
        }

    }

    public override void Update()
    {
        base.Update();
        if (animState == 0)//失去了目标，所以返回idle的时候，代表切换回去巡逻状态了
            sprite.flipX = false;

    }
}
