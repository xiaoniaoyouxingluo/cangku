using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 灵居进行攻击时 所依赖的技能释放基类
/// </summary>
public abstract class BaseSkillModule : ScriptableObject
{
    /// <summary>
    /// 技能释放
    /// </summary>
    /// <param name="caster">释放技能的人</param>
    public abstract void ApplySkill(AgentObj caster);
    /// <summary>
    /// 灵居移动
    /// </summary>
    /// <param name="caster">进行移动的人</param>
    /// <param name="index">自己是第几个攻击的，主要是为了指定下一个攻击者</param>
    public abstract void ApplyMove(AgentObj caster);
    /// <summary>
    /// 攻击结束返回原点移动
    /// </summary>
    public abstract void ComeMove();
    /// <summary>
    /// 复位函数 防止数据持久化，影响后续使用
    /// </summary>
    public abstract void Res();
}
