

/// <summary>
/// 敌人的基础状态 被继承模块
/// </summary>
public  abstract class EnemyBaseState 
{
    //EnterState翻译：开始状态
    public abstract void EnterState(Enemy enemy);


    /// <summary>
    /// 需要持续运行的状态 
    /// </summary>
    /// <param name="enemy"></param>
    public abstract void OnUpdate(Enemy enemy);

}
