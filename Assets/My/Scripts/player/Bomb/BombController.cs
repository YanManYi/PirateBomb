using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    #region ��������

    

    private Animator anim;//����
    private CircleCollider2D coll;//���壬����ȡ������������Χ�����ʱ�����������ȥ

    private float startTime;//��Ϸ��ʼʱ��
    public float WaitTime;//��ɱ�ը�ȴ�ʱ��
    public float bombFore;//�����⵽��������ʩ����
    public float damage;//��ը�˺�

    [Header("Check")]
    public float radius;//��ⷶΧ��
    public LayerMask targetLayer;//������㼶

    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;//�������ʼʱ��
        coll = GetComponent<CircleCollider2D>();

      

    }


    void Update()
    {
        //������״̬����������²���ִ�б�ը
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bomb-Off"))
        {
            //����ļ�ʱ��
            if (Time.time > startTime + WaitTime)
            {
                //����ֱ���л����ŵĶ�������ȻҲ��������������״̬���л�
                anim.Play("Bomb-Explotion");//�䱬ը��Ч
            }
        }
    }


    #region �йغ���

    /// <summary>
    ///  �ҵ���������ʩ��������ըǰ���õ�Event��
    /// </summary>
    public void Explotion()
    {

       


        coll.enabled = false;//ʹ�䲻�ڼ�ⷶΧ��ը���Լ��Ͳ�ʩ�������Լ�,Ҫ��Ȼ�����Ϸ�
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);//��������Χ���и�����������������Ӧ�Ĳ㼶���ң�
        coll.enabled = true;//�ָ�����ֹ����

        //�����ҵ��Ķ���
        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;//��ȡ��������

            if (item.CompareTag("Player"))
                item.GetComponent<PlayerController>().isHurt = true;//����һ֡û��input�ĸ���

            item.GetComponent<Rigidbody2D>().AddForce((-pos * bombFore + Vector3.up * bombFore / 2), ForceMode2D.Impulse);
            //���Ǳ���⵽ȴ���ᱻˮƽʩ��������Ϊ�������������FixedUpdate()��ļ��ʩ������������

            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Bomb-Off"))//�����ը�����Ҹ�ը������״̬�Ǳ�Ϩ������
            {
                item.GetComponent<BombController>().TurnOn();//����ȼ
            }


            if (item.CompareTag("Player") || item.CompareTag("Enemy"))//ը���������һ���Enemy��Ȼ����ÿ�Ѫ
            {
                item.GetComponent<IDamageable>().GetHit(damage);//���ýӿ�ͳһ�ĵ�Ѫ����

            }
        }

    }



    /// <summary>
    /// ը����ը��Event
    /// </summary>
    public void FinishThis()
    {
        CutSound.Instance.BombSound();
        Destroy(gameObject);
        //  gameObject.SetActive(false);
    }

    /// <summary>
    /// ������
    /// </summary>
    public void TurnOff()
    {

        anim.Play("Bomb-Off");//����¼�Ƹı�Order in Layer //�����ը����������ͼ

        gameObject.layer = LayerMask.NameToLayer("Enemy");//�����㼶�ı䣬�Ͳ��ᱻ��ΪĿ����

    }

    /// <summary>
    /// ը����ȼը��
    /// </summary>
    public void TurnOn()
    {
        startTime = Time.time;//�ü�ʱ���ڱ��������һ��ʱ��

        anim.Play("Bomb-On");//����¼�Ƹı�Order in Layer 

        gameObject.layer = LayerMask.NameToLayer("Bomb");

    }








    /// <summary>
    /// ��ʾ��ը��Χ
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);//��ͷ�ΧԲ�뾶
    }
    #endregion
}
