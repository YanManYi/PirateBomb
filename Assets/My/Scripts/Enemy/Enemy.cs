using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要被继承的类，适用不一样敌人都可用
/// </summary>
public class Enemy : MonoBehaviour
{


    #region 变量
    private GameObject alarmSign;//感叹号

    [Header("Base State")]
    public float health;//生命
    public float maxHealth;
    public bool isBoss;
    [HideInInspector]
    public bool hasBomb;//大块头用的，判断有没有炸弹在身上
    [HideInInspector]
    public bool isDead;//是否死亡




    [Header("MoveMent")]
    public float speed;
    public Transform PointA, PointB;//给定两个目标位置,然后来改变下面的targetPoint
    public Transform targetPoint;//目标点，需要list来区分人物炸弹，最近的优先
    public List<Transform> attackList = new List<Transform>();//检测到所需层级的对象集合,目的是优先范围内的目标不一样行为

    [Header("Attack Setting")]
    public float attackRate;//间隔
    private float nextAttack = 0;//cd计时器初始
    public float attackRange, skillRange;//到范围才攻击，技能


    EnemyBaseState currentBaseState;//申明 currentBaseState对象  current翻译 当前的
    public PatrolState patrolState = new PatrolState();//巡逻状态对象
    public AttackState attackState = new AttackState();//攻击状态对象

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public int animState;//给巡逻类调用，修改动画切换条件 


  
   private  GameObject UiBossHealthBar;//血条显示

    #endregion

    /// <summary>
    /// 子类不一样的敌人可能有不一样的初始赋值，所以可能重写
    /// </summary>
    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(2).gameObject;

        UiBossHealthBar = GameObject.FindGameObjectWithTag("BossHeartBar");
       

        // GameManager.instance.IsEnemy(this);//加入GameManager的敌人集合里

    }

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        UiBossHealthBar.SetActive(false);
        GameManager.instance.IsEnemy(this);//加入GameManager的敌人集合里

        TransitionToState(patrolState);//Transition翻译：过渡/转变
    }



    public virtual void Update()
    {
        anim.SetBool("dead", isDead);


        if (isDead)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            GameManager.instance.EnemyDead(this);//移除敌人列表
          
        
            return;
        }//敌人死亡后不至于一直报空访问目标点

        if (!targetPoint)
            TransitionToState(patrolState);//如果是空的，爆炸时候丢失目标位置，让他重新回归巡逻状态，重新


        if (isBoss)
        {
            //是boss的话，给与uiboss的最大值血条相等



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


        currentBaseState.OnUpdate(this);//需要实时监测当前的状态里的
        anim.SetInteger("state", animState);
    }

    /// <summary>
    /// 转换状态函数,形参就是各种不一样的状态，如巡逻或者攻击
    /// </summary>
    public void TransitionToState(EnemyBaseState state)//给什么状态就对应的状态
    {

        //当前基础状态
        currentBaseState = state;//对象赋值  切换一下形参就是不一样的状态了，例如AttackState
        currentBaseState.EnterState(this);//现在是巡逻对象点开始函数用

        //以上两句相当于state.EnterState(this);
    }

    /// <summary>
    /// 运动到目标点
    /// </summary>
    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        FilpDirection();

    }

    /// <summary>
    /// 给targetPoint交换一下PointA或者PointB
    /// </summary>
    public void SwitchPoint()
    {
        if (Mathf.Abs(PointA.position.x - transform.position.x) > Mathf.Abs(PointB.position.x - transform.position.x))

            targetPoint = PointA;
        else
            targetPoint = PointB;
    }


    /// <summary>
    /// 反转人物左右朝向
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




    //子物体的刚体触发可以在父物体代码里用检测（自己猜测的，待以后验证）
    private void OnTriggerStay2D(Collider2D collision)
    {
        //需要一直触发
        if (!attackList.Contains(collision.transform) && !hasBomb&&!isDead)
            attackList.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)//死亡了就不发感叹号了
            StartCoroutine("OnAlarm");//开启协程

    }
    /// <summary>
    /// 协程，等待感叹号播放完成后，重新隐藏
    /// </summary>
    /// <returns></returns>
    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        //等待动画播放的时间长度
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);


    }




    #region 
    /// <summary>
    /// 攻击玩家统一状态动画
    /// </summary>
    public void AttackAction()
    {


        if (Vector2.Distance(transform.position, targetPoint.position) <= attackRange)//距离小于攻击范围播放
        {
            if (Time.time > nextAttack)//计时器cd
            {

                //播放攻击动画
                anim.SetTrigger("attack");
                nextAttack = Time.time + attackRate;
               
            }

        }

    }

    /// <summary>
    ///技能释放,被继承的对象使用的技能不一样，需要重写
    /// </summary>
    public virtual void SkillAction()
    {


        if (Vector2.Distance(transform.position, targetPoint.position) <= skillRange)
        {
            if (Time.time > nextAttack)//计时器cd
            {

                //播放技能动画
                anim.SetTrigger("skill");
                nextAttack = Time.time + attackRate;

                TransitionToState(patrolState);

            }


        }
     

    }
    #endregion


}
