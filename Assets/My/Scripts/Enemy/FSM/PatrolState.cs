using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// patrol state���룺Ѳ�� ״̬
/// </summary>
public  class PatrolState : EnemyBaseState
{
    //ʵ�����������󷽷�

    public  override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;//idle��0��run�ǲ�����ɺ���idle

        enemy.SwitchPoint();//�л�һ��Ŀ���
    }

    //��Ҫʵʱ��⣬����update��
    public  override void OnUpdate(Enemy enemy)
    {
        //��������ڲ���idle״̬��ʱ��
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("1-Idle"))
        {        
            enemy.animState = 1;//idle��0��run�ǲ�����ɺ���idle

            enemy.MoveToTarget();
        }

        //����Ŀ�������½���һ��Ѳ��״̬�����²���idle
        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f) {

            //  enemy.TransitionToState(enemy.patrolState);//ͨ��Enemy�����½���������ĺ������л�Ŀ���
            enemy.TransitionToState(this);//ͨ��Enemy�����½���������ĺ������л�Ŀ���

        }

        if (enemy.attackList.Count>0) {

            //��ʼ��״̬������һ����״̬������Ϊ�β�
            enemy.TransitionToState(enemy.attackState);
        }

      

    }
}
