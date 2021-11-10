using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {

        enemy.animState = 2;//进入第二种动画run跑向攻击目标
        enemy.targetPoint = enemy.attackList[0];//具体攻击的目标。下面根据距离切换判断

    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.hasBomb) return;//大块头专用
    
        if (enemy.attackList.Count == 0)//重新加载一次攻击状态，如果第一个目标消失|| enemy.targetPoint == null
        {
            enemy.TransitionToState(enemy.patrolState);//范围内没有攻击目标就切换会巡逻状态
          
        }

        //本来是大于一个对象的时候，就遍历一下，对比距离，但炸弹会消失一下，
        if (enemy.attackList.Count > 1)
        {


            for (int i = 0; i < enemy.attackList.Count; i++)
            {

                if (Vector2.Distance(enemy.transform.position, enemy.attackList[i].position)//接下来的目标如果距离有小于一开始找到的第一个目标，则需要切换攻击目标
                    < Vector2.Distance(enemy.transform.position, enemy.targetPoint.position))
                {
                    enemy.targetPoint = enemy.attackList[i];//切换距离更近的目标



                }


            }


        }
        else 
       
        if (enemy.attackList.Count == 1)// 但炸弹会消失一下，
            enemy.TransitionToState(this);
        //    enemy.targetPoint = enemy.attackList[0];//具体攻击的目标


        if (enemy.targetPoint.CompareTag("Player"))
        {
            if (Vector2.Distance(enemy.transform.position, enemy.targetPoint.position) > enemy.attackRange)
            {
                //!enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("skillBigGuy")是大块头专属的，因为动作技能是两个步骤，都不可以移动
                if (!enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("skill")&& !enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("skillBigGuy"))//防止不是在吹灭的瞬间让其移动
                    enemy.MoveToTarget();
            }

            if (enemy.targetPoint.gameObject.GetComponent<PlayerController>().health != 0)
                enemy.AttackAction();//攻击行动

            else
            {
                enemy.gameObject.transform.GetChild(0).gameObject.SetActive(false);//主角死亡后让攻击对象的检测失效，就不会一直跟着尸体打
            }
        }
      
        


        if (enemy.targetPoint.CompareTag("Bomb"))
        {
            //if (Vector2.Distance(enemy.targetPoint.position, enemy.transform.position) < 0.1f)
            //    enemy.speed = 0;
            if (Vector2.Distance(enemy.transform.position, enemy.targetPoint.position) > enemy.skillRange)
                enemy.MoveToTarget();

            enemy.SkillAction();//技能行动

        }
       
           

        


    }
}
