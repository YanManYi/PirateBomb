using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҫ���̳е��࣬���ò�һ�����˶�����
/// </summary>
public class Enemy : MonoBehaviour
{


    #region ����
    private GameObject alarmSign;//��̾��

    [Header("Base State")]
    public float health;//����
    public float maxHealth;
    public bool isBoss;
    [HideInInspector]
    public bool hasBomb;//���ͷ�õģ��ж���û��ը��������
    [HideInInspector]
    public bool isDead;//�Ƿ�����




    [Header("MoveMent")]
    public float speed;
    public Transform PointA, PointB;//��������Ŀ��λ��,Ȼ�����ı������targetPoint
    public Transform targetPoint;//Ŀ��㣬��Ҫlist����������ը�������������
    public List<Transform> attackList = new List<Transform>();//��⵽����㼶�Ķ��󼯺�,Ŀ�������ȷ�Χ�ڵ�Ŀ�겻һ����Ϊ

    [Header("Attack Setting")]
    public float attackRate;//���
    private float nextAttack = 0;//cd��ʱ����ʼ
    public float attackRange, skillRange;//����Χ�Ź���������


    EnemyBaseState currentBaseState;//���� currentBaseState����  current���� ��ǰ��
    public PatrolState patrolState = new PatrolState();//Ѳ��״̬����
    public AttackState attackState = new AttackState();//����״̬����

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public int animState;//��Ѳ������ã��޸Ķ����л����� 


  
   private  GameObject UiBossHealthBar;//Ѫ����ʾ

    #endregion

    /// <summary>
    /// ���಻һ���ĵ��˿����в�һ���ĳ�ʼ��ֵ�����Կ�����д
    /// </summary>
    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(2).gameObject;

        UiBossHealthBar = GameObject.FindGameObjectWithTag("BossHeartBar");
       

        // GameManager.instance.IsEnemy(this);//����GameManager�ĵ��˼�����

    }

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        UiBossHealthBar.SetActive(false);
        GameManager.instance.IsEnemy(this);//����GameManager�ĵ��˼�����

        TransitionToState(patrolState);//Transition���룺����/ת��
    }



    public virtual void Update()
    {
        anim.SetBool("dead", isDead);


        if (isDead)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            GameManager.instance.EnemyDead(this);//�Ƴ������б�
          
        
            return;
        }//��������������һֱ���շ���Ŀ���

        if (!targetPoint)
            TransitionToState(patrolState);//����ǿյģ���ըʱ��ʧĿ��λ�ã��������»ع�Ѳ��״̬������


        if (isBoss)
        {
            //��boss�Ļ�������uiboss�����ֵѪ�����



            if (targetPoint.CompareTag("Player"))
            {
                UiBossHealthBar.SetActive(true);

                UIManager.instance.SetBossHealth(maxHealth);
                UIManager.instance.UpdateBossHealth(health);

            }
            else
            {
                UiBossHealthBar.SetActive(false);
                isBoss = false;
            }

        }
        else if (targetPoint.CompareTag("Player")) isBoss = true;


        currentBaseState.OnUpdate(this);//��Ҫʵʱ��⵱ǰ��״̬���
        anim.SetInteger("state", animState);
    }

    /// <summary>
    /// ת��״̬����,�βξ��Ǹ��ֲ�һ����״̬����Ѳ�߻��߹���
    /// </summary>
    public void TransitionToState(EnemyBaseState state)//��ʲô״̬�Ͷ�Ӧ��״̬
    {

        //��ǰ����״̬
        currentBaseState = state;//����ֵ  �л�һ���βξ��ǲ�һ����״̬�ˣ�����AttackState
        currentBaseState.EnterState(this);//������Ѳ�߶���㿪ʼ������

        //���������൱��state.EnterState(this);
    }

    /// <summary>
    /// �˶���Ŀ���
    /// </summary>
    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        FilpDirection();

    }

    /// <summary>
    /// ��targetPoint����һ��PointA����PointB
    /// </summary>
    public void SwitchPoint()
    {
        if (Mathf.Abs(PointA.position.x - transform.position.x) > Mathf.Abs(PointB.position.x - transform.position.x))

            targetPoint = PointA;
        else
            targetPoint = PointB;
    }


    /// <summary>
    /// ��ת�������ҳ���
    /// </summary>
    public void FilpDirection()
    {

        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }




    //������ĸ��崥�������ڸ�����������ü�⣨�Լ��²�ģ����Ժ���֤��
    private void OnTriggerStay2D(Collider2D collision)
    {
        //��Ҫһֱ����
        if (!attackList.Contains(collision.transform) && !hasBomb&&!isDead)
            attackList.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)//�����˾Ͳ�����̾����
            StartCoroutine("OnAlarm");//����Э��

    }
    /// <summary>
    /// Э�̣��ȴ���̾�Ų�����ɺ���������
    /// </summary>
    /// <returns></returns>
    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        //�ȴ��������ŵ�ʱ�䳤��
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);


    }




    #region 
    /// <summary>
    /// �������ͳһ״̬����
    /// </summary>
    public void AttackAction()
    {


        if (Vector2.Distance(transform.position, targetPoint.position) <= attackRange)//����С�ڹ�����Χ����
        {
            if (Time.time > nextAttack)//��ʱ��cd
            {

                //���Ź�������
                anim.SetTrigger("attack");
                nextAttack = Time.time + attackRate;
               
            }

        }

    }

    /// <summary>
    ///�����ͷ�,���̳еĶ���ʹ�õļ��ܲ�һ������Ҫ��д
    /// </summary>
    public virtual void SkillAction()
    {


        if (Vector2.Distance(transform.position, targetPoint.position) <= skillRange)
        {
            if (Time.time > nextAttack)//��ʱ��cd
            {

                //���ż��ܶ���
                anim.SetTrigger("skill");
                nextAttack = Time.time + attackRate;

                TransitionToState(patrolState);

            }


        }
     

    }
    #endregion


}
