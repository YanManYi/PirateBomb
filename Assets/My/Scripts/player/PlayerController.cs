using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour,IDamageable
{

    #region 申明所需变量

    

    private Rigidbody2D rb;//刚体组件
    private Animator anim;//动画

  
    

    //创建bool变量
    public  bool isHurt;//受伤就取消input的输入覆盖，让其被炸飞

    [Header("Player State")]//生命状态
    public float health;//生命
    public bool isDead;//是否死亡


    public float speed;//速度变量
    public float jumpForce;//跳跃力大小的变量

    [Header("Ground Check")]//地面检测
    public Transform groundCheck; //脚底下用来检测的对象（小点点）
    public float checkRadius;//检测范围
    

  
    [HideInInspector]//隐藏起来
    public bool isGround;//是否脚底踩到所需图层
    [HideInInspector]
    public bool isJump;//给动画切换所需判断
    private  bool canJump;//是否按下检测跳

    [Header("JumpFx")]
    public GameObject jumpFx;//起跳脚底特效
    public GameObject landFx;//下落后脚底的特效
    public GameObject RunFx;//跑动脚底的特效

    [Header("Attack Settings")]
    public GameObject bombPrefab;//炸弹对象
    [HideInInspector]
    public float nextAttack = 0;//变相计时器的用法
    public float attackRate;//攻击速度就是cd



    //移动端部分

    VariableJoystick joystick;


    #endregion

    #region 生命周期函数

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();//获取赋值
        anim = GetComponent<Animator>();
    

        health = GameManager.instance.LoadHealth();//键值对里拿血量
        CutSound.Instance.BackSound();

        UIManager.instance.UpdateHealth(health);
        Time.timeScale = 1;//游戏暂停，1是游戏恢复正常


        joystick = GameObject.FindObjectOfType<VariableJoystick>();

    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject)//防止UI触发跳跃。如果手机上没有这个问题，因为都是事件触发
        {
            return;
        }

        anim.SetBool("Dead",isDead);//自己死亡了就播放死亡动画

        if (isDead){
         
            return;//停止检测
        }
       

        //用来判断是否正在播放受伤动画，然后取消被input的覆盖
      //  if(anim.GetCurrentAnimatorStateInfo(1).IsName("7-Hit"))//在炸弹爆炸的时候也变ture,这样子炸弹也可以炸飞主角不至于被覆盖

             isHurt = anim.GetCurrentAnimatorStateInfo(1).IsName("7-Hit");



        CheckInput();//检测输入
       

    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;//人物暂停或者被炸飞死亡了不至于一直在完成之前的位置目标移动
            rb.gravityScale = 5;//空中重力变5，要不然人物在空中炸死后下落状态是慢悠悠
            return;
        }

        PhysicsCheck();

      
        //非受伤状态才可以移动和跳跃
        if (!isHurt)
        {
            Movement();//input会覆盖爆炸Rigidbody的速度，所以用isHurt来锁定就可以让 Player 被击飞了
            Jump();
        }

      
     
    }

    #endregion



    

    /// <summary>
    /// 检查是否按跳跃//Update里每一帧
    /// </summary>
    void CheckInput()
    {

        if (Input.GetMouseButtonDown(0) && isGround&& !JoystickIsActive())
        {
            canJump = true;

          

        }


        if (Input.GetMouseButtonDown(1))
        {
            //放炸弹
            Attack();

            

        }

    }
    #region 移动的函数
    public void CheckInput_APK_JUMP()
    {

        if (isGround)
        {
            canJump = true;



        }




    }
    public void CheckInput_APK_Bomb()
    {


        //放炸弹
        Attack();




    } 
    #endregion





    #region FixedUpdate()里用的函数



    /// <summary>
    /// 脚底是否检测到isGround的返回值
    /// </summary>
    void PhysicsCheck()
    {
        //这里是点的一定范围内有没有对应的层级
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, 1 << 8);//位置点-范围距离-检测所需层级（）
        if (isGround) 
        {   
            rb.gravityScale = 1;
            isJump = false;//动画判断的
        }                   
        else rb.gravityScale = 5;//空中重力变5


    }


    public bool JoystickIsActive()
    { 
        if (!joystick) return false ;
        else return true;
    }

    /// <summary>
    /// 左右移动
    /// </summary>
    void Movement()
    {
        // float horizontal_Input = Input.GetAxisRaw("Horizontal");//-1~1  不包含小数      


        float horizontal_Input;

        if(JoystickIsActive())  horizontal_Input = joystick.Horizontal;
        else horizontal_Input = Input.GetAxisRaw("Horizontal");


        rb.velocity = new Vector2(horizontal_Input * speed, rb.velocity.y);//给一个方向力

        if (horizontal_Input != 0)//这里覆盖了炸弹施加左右的力，重新变0了，所以不动
        {
            //在移动杆上这样子不行了，因为值不是1或者-1，而是过渡
            //  transform.localScale = new Vector3(horizontal_Input, 1, 1);//改变左右朝向
            //直接改变rotation的值来改变朝向
            if (horizontal_Input > 0) transform.localEulerAngles = new Vector3(0, 0, 0);
            if (horizontal_Input < 0) transform.localEulerAngles = new Vector3(0, 180, 0);



            #region RunFx 跑步脚底白色效果

            if (horizontal_Input > 0 && isGround)//条件：有运动加非空中
            {
                RunFx.SetActive(true);//激活
                RunFx.transform.localScale = new Vector3(1.5f, 1, 1);//特效结尾方向和主角要一致，主角左右，特效也左右
                RunFx.transform.position = transform.position + new Vector3(-0.5f, -0.8f, 0);//激活后的位置
            }
            else
            {
                if (isGround)
                {
                    RunFx.SetActive(true);//激活
                    RunFx.transform.localScale = new Vector3(-1.5f, 1, 1);//特效结尾方向和主角要一致，主角左右，特效也左右
                    RunFx.transform.position = transform.position + new Vector3(0.5f, -0.8f, 0);//激活后的位置
                }

            }
            #endregion

        }
    }



    /// <summary>
    /// Player的跳跃
    /// </summary>
    void Jump()
    {
        if (canJump)
        {
          

            jumpFx.SetActive(true);//脚底白色特效
            jumpFx.transform.position = transform.position + new Vector3(0, -0.8f, 0);//给位置


            rb.velocity = new Vector2(rb.velocity.x, jumpForce);//给力
            canJump = false;//改回false，要不然一直有向上的力

            isJump = true;//是否在跳跃中，给动画脚本里判断是否播放跳跃动画

            CutSound.Instance.JumpSound();


  
        }

    }


    #endregion





    /// <summary>
    /// 放炸弹
    /// </summary>
    public void Attack() {


        //变相的计时器
        if (Time.time > nextAttack) {

          

            //放炸弹需要朝向主角前面
            if (transform.rotation.y> -1) 
                Instantiate(bombPrefab, transform.position + new Vector3(1.2f, 0f, 0), Quaternion.identity);
            
            else
                Instantiate(bombPrefab, transform.position + new Vector3(-1.2f, 0f, 0), Quaternion.identity);

            nextAttack = Time.time + attackRate;       //变相计时器，可以卡技能CD

            CutSound.Instance.FreeBombSound();

        }

    }



    //动画的帧事件、、要不然一直是在播放下落的特效
    public void LandFx()
    {
        //在踩到地面的时候触发落地效果
        landFx.SetActive(true);
        landFx.transform.position = transform.position + new Vector3(0, -0.8f, 0);

        CutSound.Instance.LandSound();

    }




    bool isOnePlay = true;

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("7-Hit"))//受伤时候不减血
        {
            health -= damage;
            if(!isDead)
            CutSound.Instance.HitSound();
            
            if (health < 1)
            {
                health = 0;
                isDead = true;

              
                if (isOnePlay)
                {
                    Invoke("GameOverSound", 1);
                    isOnePlay = false;
                }
            }
            

            anim.SetTrigger("Hit");

            UIManager.instance.UpdateHealth(health);//单例模式实现ui扣血
            //在加载场景的时候，这句话还需要写在LoadHealth()方法里，这里只会受伤后才跟新血量
        }
    }

     void GameOverSound() { CutSound.Instance.DieSound(); }


   


   

    /// <summary>
    /// 不需要调用。脚底小点检测范围可视化显示出
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }



}
