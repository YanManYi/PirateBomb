using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// patrol state翻译：巡逻 状态
/// </summary>
public  class PatrolState : EnemyBaseState
{
    //实现了两个抽象方法

    public  override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;//idle是0；run是播放完成后是idle

        enemy.SwitchPoint();//切换一下目标点
    }

    //需要实时监测，所以update里
    public  override void OnUpdate(Enemy enemy)
    {
        //如果不是在播放idle状态的时候
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("1-Idle"))
        {        
            enemy.animState = 1;//idle是0；run是播放完成后是idle

            enemy.MoveToTarget();
        }

        //到达目标点后，重新进入一下巡逻状态，重新播放idle
        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f) {

            //  enemy.TransitionToState(enemy.patrolState);//通过Enemy有重新进入了上面的函数，切换目标点
            enemy.TransitionToState(this);//通过Enemy有重新进入了上面的函数，切换目标点

        }

        if (enemy.attackList.Count>0) {

            //开始换状态，给不一样的状态对象作为形参
            enemy.TransitionToState(enemy.attackState);
        }

      

    }
}
