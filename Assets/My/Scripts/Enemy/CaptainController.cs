using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ɫ�ӳ�
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


    //��д����ĸ�ֵ����ĸ�����Ҫ
    public override void Init()
    {
        base.Init();

        sprite = GetComponent<SpriteRenderer>();
    }

    //��д����
    public override void SkillAction()
    {
        base.SkillAction();

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("skill"))//����ö����ڲ��ŵ������
     
        {
            sprite.flipX = true;//�ĸ�����

            if (transform.position.x > targetPoint.position.x)//������ը��Ŀ�����ұ�
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left, speed * 2 * Time.deltaTime);

            }
        }
        else { 
            //���ͻȻ��ը��ʧ�ˣ��Ͳ���ĳ���Ҫ���
            sprite.flipX = false;
          
        }

    }

    public override void Update()
    {
        base.Update();
        if (animState == 0)//ʧȥ��Ŀ�꣬���Է���idle��ʱ�򣬴����л���ȥѲ��״̬��
            sprite.flipX = false;

    }
}
