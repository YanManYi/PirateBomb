using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    #region 申明变量

    

    private Animator anim;//动画
    private CircleCollider2D coll;//刚体，用来取消在物理检测周围刚体的时候不让自身检测进去

    private float startTime;//游戏开始时间
    public float WaitTime;//变成爆炸等待时间
    public float bombFore;//物理检测到物体后可以施加力
    public float damage;//爆炸伤害

    [Header("Check")]
    public float radius;//检测范围变
    public LayerMask targetLayer;//所需检测层级

    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;//产生后初始时间
        coll = GetComponent<CircleCollider2D>();

      

    }


    void Update()
    {
        //被吹灭状态动画的情况下不该执行爆炸
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bomb-Off"))
        {
            //变相的计时器
            if (Time.time > startTime + WaitTime)
            {
                //可以直接切换播放的动画，当然也可以设置条件，状态机切换
                anim.Play("Bomb-Explotion");//变爆炸特效
            }
        }
    }


    #region 有关函数

    /// <summary>
    ///  找到其他对象施加力（爆炸前调用的Event）
    /// </summary>
    public void Explotion()
    {

       


        coll.enabled = false;//使其不在检测范围，炸弹自己就不施加力给自己,要不然会向上飞
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);//物理检测周围带有刚体组件（给定了相对应的层级力找）
        coll.enabled = true;//恢复，防止掉落

        //遍历找到的对象
        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;//获取方向向量

            if (item.CompareTag("Player"))
                item.GetComponent<PlayerController>().isHurt = true;//在这一帧没有input的覆盖

            item.GetComponent<Rigidbody2D>().AddForce((-pos * bombFore + Vector3.up * bombFore / 2), ForceMode2D.Impulse);
            //主角被检测到却不会被水平施加力，因为被主角自身代码FixedUpdate()里的检测施加力给覆盖了

            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Bomb-Off"))//如果是炸弹，且该炸弹动画状态是被熄灭的情况
            {
                item.GetComponent<BombController>().TurnOn();//被点燃
            }


            if (item.CompareTag("Player") || item.CompareTag("Enemy"))//炸弹检测是玩家或者Enemy，然后调用扣血
            {
                item.GetComponent<IDamageable>().GetHit(damage);//调用接口统一的掉血方法

            }
        }

    }



    /// <summary>
    /// 炸弹爆炸后Event
    /// </summary>
    public void FinishThis()
    {
        CutSound.Instance.BombSound();
        Destroy(gameObject);
        //  gameObject.SetActive(false);
    }

    /// <summary>
    /// 被吹灭
    /// </summary>
    public void TurnOff()
    {

        anim.Play("Bomb-Off");//动画录制改变Order in Layer //吹灭的炸弹担当背景图

        gameObject.layer = LayerMask.NameToLayer("Enemy");//把它层级改变，就不会被作为目标了

    }

    /// <summary>
    /// 炸弹点燃炸弹
    /// </summary>
    public void TurnOn()
    {
        startTime = Time.time;//让计时器在变成启动那一刻时间

        anim.Play("Bomb-On");//动画录制改变Order in Layer 

        gameObject.layer = LayerMask.NameToLayer("Bomb");

    }








    /// <summary>
    /// 显示爆炸范围
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);//点和范围圆半径
    }
    #endregion
}
