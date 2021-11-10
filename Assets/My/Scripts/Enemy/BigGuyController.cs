using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuyController : Enemy,IDamageable
{

    public float powerForce;//��
    private  Transform pickuoPoint;//ը�����������µ�λ�õ�

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



    //animation ��Event

        /// <summary>
        /// ����ը��
        /// </summary>
    public void PickUpBomb() {

        if (targetPoint.CompareTag("Bomb") && !hasBomb)
        {

            targetPoint.gameObject.transform.position = pickuoPoint.position;

            targetPoint.SetParent(pickuoPoint);//��ը�����ø����������Ӹ�����

            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            hasBomb = true;
            Invoke("_HasBomb", 1);
        }
        else TransitionToState(patrolState);
    

    }

    /// <summary>
    /// �������֡����ʱ���޷�������ը������Ϊը����ǰ��ʧ��
    /// </summary>
    void _HasBomb() {

        //�����⣬�ڱ�ը�У�ը����ǰ��ʧ��û�취�ָ���hasbomb=false,�����ڼ���ը����ʱ����ӳ�һ��ָ�����Ч��
        hasBomb = false;//���ӳ�ȥ��ʱ�򣬾�ֱ�ӻָ�����ѧ
    }

    //animation ��Event
    //��ը��
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

        //  hasBomb = false;//���ӳ�ȥ��ʱ�򣬻ָ�����ѧ


    }




}
