

/// <summary>
/// ���˵Ļ���״̬ ���̳�ģ��
/// </summary>
public  abstract class EnemyBaseState 
{
    //EnterState���룺��ʼ״̬
    public abstract void EnterState(Enemy enemy);


    /// <summary>
    /// ��Ҫ�������е�״̬ 
    /// </summary>
    /// <param name="enemy"></param>
    public abstract void OnUpdate(Enemy enemy);

}
