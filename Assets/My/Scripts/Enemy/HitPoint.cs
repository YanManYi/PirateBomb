using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    private  int dir;//������
    public bool bombAvilable;//�Ƿ���Զ�ը��������,����˵�߿�����ʹ���ƶ�����ͷ�ֵ���ʹ��
    

    public float damage;//�����˺�
    public float addForce;//�߿�����


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //���Ҳ��ʱ�������
        if (transform.position.x > collision.transform.position.x)
            dir = -1;
        else dir = 1;

        if (collision.CompareTag("Player")) {

            collision.GetComponent<IDamageable>().GetHit(damage);//�������ǣ��͵��ýӿ�ͳһ�Ŀ�Ѫ����
        
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1f) * addForce*0.2f, ForceMode2D.Impulse);
        }
        
        //��ը��
        if (collision.CompareTag("Bomb")&&bombAvilable)
        {
          
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2 (dir,1)*addForce,ForceMode2D.Impulse);
        }
    }
}
