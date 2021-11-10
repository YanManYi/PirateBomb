using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour,IDamageable
{

    #region �����������

    

    private Rigidbody2D rb;//�������
    private Animator anim;//����

  
    

    //����bool����
    public  bool isHurt;//���˾�ȡ��input�����븲�ǣ����䱻ը��

    [Header("Player State")]//����״̬
    public float health;//����
    public bool isDead;//�Ƿ�����


    public float speed;//�ٶȱ���
    public float jumpForce;//��Ծ����С�ı���

    [Header("Ground Check")]//������
    public Transform groundCheck; //�ŵ����������Ķ���С��㣩
    public float checkRadius;//��ⷶΧ
    

  
    [HideInInspector]//��������
    public bool isGround;//�Ƿ�ŵײȵ�����ͼ��
    [HideInInspector]
    public bool isJump;//�������л������ж�
    private  bool canJump;//�Ƿ��¼����

    [Header("JumpFx")]
    public GameObject jumpFx;//�����ŵ���Ч
    public GameObject landFx;//�����ŵ׵���Ч
    public GameObject RunFx;//�ܶ��ŵ׵���Ч

    [Header("Attack Settings")]
    public GameObject bombPrefab;//ը������
    [HideInInspector]
    public float nextAttack = 0;//�����ʱ�����÷�
    public float attackRate;//�����ٶȾ���cd



    //�ƶ��˲���

    VariableJoystick joystick;


    #endregion

    #region �������ں���

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();//��ȡ��ֵ
        anim = GetComponent<Animator>();
    

        health = GameManager.instance.LoadHealth();//��ֵ������Ѫ��
        CutSound.Instance.BackSound();

        UIManager.instance.UpdateHealth(health);
        Time.timeScale = 1;//��Ϸ��ͣ��1����Ϸ�ָ�����


        joystick = GameObject.FindObjectOfType<VariableJoystick>();

    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject)//��ֹUI������Ծ������ֻ���û��������⣬��Ϊ�����¼�����
        {
            return;
        }

        anim.SetBool("Dead",isDead);//�Լ������˾Ͳ�����������

        if (isDead){
         
            return;//ֹͣ���
        }
       

        //�����ж��Ƿ����ڲ������˶�����Ȼ��ȡ����input�ĸ���
      //  if(anim.GetCurrentAnimatorStateInfo(1).IsName("7-Hit"))//��ը����ը��ʱ��Ҳ��ture,������ը��Ҳ����ը�����ǲ����ڱ�����

             isHurt = anim.GetCurrentAnimatorStateInfo(1).IsName("7-Hit");



        CheckInput();//�������
       

    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;//������ͣ���߱�ը�������˲�����һֱ�����֮ǰ��λ��Ŀ���ƶ�
            rb.gravityScale = 5;//����������5��Ҫ��Ȼ�����ڿ���ը��������״̬��������
            return;
        }

        PhysicsCheck();

      
        //������״̬�ſ����ƶ�����Ծ
        if (!isHurt)
        {
            Movement();//input�Ḳ�Ǳ�ըRigidbody���ٶȣ�������isHurt�������Ϳ����� Player ��������
            Jump();
        }

      
     
    }

    #endregion



    

    /// <summary>
    /// ����Ƿ���Ծ//Update��ÿһ֡
    /// </summary>
    void CheckInput()
    {

        if (Input.GetMouseButtonDown(0) && isGround&& !JoystickIsActive())
        {
            canJump = true;

          

        }


        if (Input.GetMouseButtonDown(1))
        {
            //��ը��
            Attack();

            

        }

    }
    #region �ƶ��ĺ���
    public void CheckInput_APK_JUMP()
    {

        if (isGround)
        {
            canJump = true;



        }




    }
    public void CheckInput_APK_Bomb()
    {


        //��ը��
        Attack();




    } 
    #endregion





    #region FixedUpdate()���õĺ���



    /// <summary>
    /// �ŵ��Ƿ��⵽isGround�ķ���ֵ
    /// </summary>
    void PhysicsCheck()
    {
        //�����ǵ��һ����Χ����û�ж�Ӧ�Ĳ㼶
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, 1 << 8);//λ�õ�-��Χ����-�������㼶����
        if (isGround) 
        {   
            rb.gravityScale = 1;
            isJump = false;//�����жϵ�
        }                   
        else rb.gravityScale = 5;//����������5


    }


    public bool JoystickIsActive()
    { 
        if (!joystick) return false ;
        else return true;
    }

    /// <summary>
    /// �����ƶ�
    /// </summary>
    void Movement()
    {
        // float horizontal_Input = Input.GetAxisRaw("Horizontal");//-1~1  ������С��      


        float horizontal_Input;

        if(JoystickIsActive())  horizontal_Input = joystick.Horizontal;
        else horizontal_Input = Input.GetAxisRaw("Horizontal");


        rb.velocity = new Vector2(horizontal_Input * speed, rb.velocity.y);//��һ��������

        if (horizontal_Input != 0)//���︲����ը��ʩ�����ҵ��������±�0�ˣ����Բ���
        {
            //���ƶ����������Ӳ����ˣ���Ϊֵ����1����-1�����ǹ���
            //  transform.localScale = new Vector3(horizontal_Input, 1, 1);//�ı����ҳ���
            //ֱ�Ӹı�rotation��ֵ���ı䳯��
            if (horizontal_Input > 0) transform.localEulerAngles = new Vector3(0, 0, 0);
            if (horizontal_Input < 0) transform.localEulerAngles = new Vector3(0, 180, 0);



            #region RunFx �ܲ��ŵװ�ɫЧ��

            if (horizontal_Input > 0 && isGround)//���������˶��ӷǿ���
            {
                RunFx.SetActive(true);//����
                RunFx.transform.localScale = new Vector3(1.5f, 1, 1);//��Ч��β���������Ҫһ�£��������ң���ЧҲ����
                RunFx.transform.position = transform.position + new Vector3(-0.5f, -0.8f, 0);//������λ��
            }
            else
            {
                if (isGround)
                {
                    RunFx.SetActive(true);//����
                    RunFx.transform.localScale = new Vector3(-1.5f, 1, 1);//��Ч��β���������Ҫһ�£��������ң���ЧҲ����
                    RunFx.transform.position = transform.position + new Vector3(0.5f, -0.8f, 0);//������λ��
                }

            }
            #endregion

        }
    }



    /// <summary>
    /// Player����Ծ
    /// </summary>
    void Jump()
    {
        if (canJump)
        {
          

            jumpFx.SetActive(true);//�ŵװ�ɫ��Ч
            jumpFx.transform.position = transform.position + new Vector3(0, -0.8f, 0);//��λ��


            rb.velocity = new Vector2(rb.velocity.x, jumpForce);//����
            canJump = false;//�Ļ�false��Ҫ��Ȼһֱ�����ϵ���

            isJump = true;//�Ƿ�����Ծ�У��������ű����ж��Ƿ񲥷���Ծ����

            CutSound.Instance.JumpSound();


  
        }

    }


    #endregion





    /// <summary>
    /// ��ը��
    /// </summary>
    public void Attack() {


        //����ļ�ʱ��
        if (Time.time > nextAttack) {

          

            //��ը����Ҫ��������ǰ��
            if (transform.rotation.y> -1) 
                Instantiate(bombPrefab, transform.position + new Vector3(1.2f, 0f, 0), Quaternion.identity);
            
            else
                Instantiate(bombPrefab, transform.position + new Vector3(-1.2f, 0f, 0), Quaternion.identity);

            nextAttack = Time.time + attackRate;       //�����ʱ�������Կ�����CD

            CutSound.Instance.FreeBombSound();

        }

    }



    //������֡�¼�����Ҫ��Ȼһֱ���ڲ����������Ч
    public void LandFx()
    {
        //�ڲȵ������ʱ�򴥷����Ч��
        landFx.SetActive(true);
        landFx.transform.position = transform.position + new Vector3(0, -0.8f, 0);

        CutSound.Instance.LandSound();

    }




    bool isOnePlay = true;

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("7-Hit"))//����ʱ�򲻼�Ѫ
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

            UIManager.instance.UpdateHealth(health);//����ģʽʵ��ui��Ѫ
            //�ڼ��س�����ʱ����仰����Ҫд��LoadHealth()���������ֻ�����˺�Ÿ���Ѫ��
        }
    }

     void GameOverSound() { CutSound.Instance.DieSound(); }


   


   

    /// <summary>
    /// ����Ҫ���á��ŵ�С���ⷶΧ���ӻ���ʾ��
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }



}
