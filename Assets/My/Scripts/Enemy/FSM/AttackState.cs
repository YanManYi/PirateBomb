using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {

        enemy.animState = 2;//����ڶ��ֶ���run���򹥻�Ŀ��
        enemy.targetPoint = enemy.attackList[0];//���幥����Ŀ�ꡣ������ݾ����л��ж�

    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.hasBomb) return;//���ͷר��
    
        if (enemy.attackList.Count == 0)//���¼���һ�ι���״̬�������һ��Ŀ����ʧ|| enemy.targetPoint == null
        {
            enemy.TransitionToState(enemy.patrolState);//��Χ��û�й���Ŀ����л���Ѳ��״̬
          
        }

        //�����Ǵ���һ�������ʱ�򣬾ͱ���һ�£��ԱȾ��룬��ը������ʧһ�£�
        if (enemy.attackList.Count > 1)
        {


            for (int i = 0; i < enemy.attackList.Count; i++)
            {

                if (Vector2.Distance(enemy.transform.position, enemy.attackList[i].position)//��������Ŀ�����������С��һ��ʼ�ҵ��ĵ�һ��Ŀ�꣬����Ҫ�л�����Ŀ��
                    < Vector2.Distance(enemy.transform.position, enemy.targetPoint.position))
                {
                    enemy.targetPoint = enemy.attackList[i];//�л����������Ŀ��



                }


            }


        }
        else 
       
        if (enemy.attackList.Count == 1)// ��ը������ʧһ�£�
            enemy.TransitionToState(this);
        //    enemy.targetPoint = enemy.attackList[0];//���幥����Ŀ��


        if (enemy.targetPoint.CompareTag("Player"))
        {
            if (Vector2.Distance(enemy.transform.position, enemy.targetPoint.position) > enemy.attackRange)
            {
                //!enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("skillBigGuy")�Ǵ��ͷר���ģ���Ϊ�����������������裬���������ƶ�
                if (!enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("skill")&& !enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("skillBigGuy"))//��ֹ�����ڴ����˲�������ƶ�
                    enemy.MoveToTarget();
            }

            if (enemy.targetPoint.gameObject.GetComponent<PlayerController>().health != 0)
                enemy.AttackAction();//�����ж�

            else
            {
                enemy.gameObject.transform.GetChild(0).gameObject.SetActive(false);//�����������ù�������ļ��ʧЧ���Ͳ���һֱ����ʬ���
            }
        }
      
        


        if (enemy.targetPoint.CompareTag("Bomb"))
        {
            //if (Vector2.Distance(enemy.targetPoint.position, enemy.transform.position) < 0.1f)
            //    enemy.speed = 0;
            if (Vector2.Distance(enemy.transform.position, enemy.targetPoint.position) > enemy.skillRange)
                enemy.MoveToTarget();

            enemy.SkillAction();//�����ж�

        }
       
           

        


    }
}
