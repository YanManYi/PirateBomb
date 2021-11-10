using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator animator;//�������
    private Rigidbody2D rb;//�����������
    private PlayerController playerController;//��ҿ��������

    void Start()
    {

        animator = GetComponent<Animator>();//��ȡ����Animator�����ֵ
        rb = GetComponent<Rigidbody2D>();//��ȡ��ֵ
        playerController = GetComponent<PlayerController>();//�������ű���ֵ

    }


    void Update()
    {

         #region ״̬�������л�
        //ʩ�������ҵ����������л���Run���������޸�
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);


        //�������ʱ�򲥷�Fall�����޸�
        if (rb.velocity.y > 0.1f)
            animator.SetBool("Fall", true);
        else
            animator.SetBool("Fall", false);


        //�������ʱ�򲥷�Fall�����޸�
        if (rb.velocity.y < -0.1f)
            animator.SetBool("Fall", true);
        else
            animator.SetBool("Fall", false);


        //��Ծ�ͽӴ�����������޸�
        animator.SetBool("Jump", playerController.isJump);
        animator.SetBool("Ground", playerController.isGround);


        #endregion

    }
}
