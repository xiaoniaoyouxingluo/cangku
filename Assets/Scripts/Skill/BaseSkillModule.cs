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
    public abstract void Apply(GameObject caster);
}
