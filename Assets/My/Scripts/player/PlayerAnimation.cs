using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator animator;//动画组件
    private Rigidbody2D rb;//刚体组件变量
    private PlayerController playerController;//玩家控制器组件

    void Start()
    {

        animator = GetComponent<Animator>();//获取动画Animator组件赋值
        rb = GetComponent<Rigidbody2D>();//获取赋值
        playerController = GetComponent<PlayerController>();//控制器脚本赋值

    }


    void Update()
    {

         #region 状态机条件切换
        //施加左还是右的力，都可切换成Run动画条件修改
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);


        //向上落的时候播放Fall条件修改
        if (rb.velocity.y > 0.1f)
            animator.SetBool("Fall", true);
        else
            animator.SetBool("Fall", false);


        //向下落的时候播放Fall条件修改
        if (rb.velocity.y < -0.1f)
            animator.SetBool("Fall", true);
        else
            animator.SetBool("Fall", false);


        //跳跃和接触地面的条件修改
        animator.SetBool("Jump", playerController.isJump);
        animator.SetBool("Ground", playerController.isGround);


        #endregion

    }
}
