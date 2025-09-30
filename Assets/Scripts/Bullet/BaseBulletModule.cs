using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 子弹触发事件时 依赖的基类
/// </summary>
public abstract class BaseBulletModule : ScriptableObject
{
    /// <summary>
    /// 子弹触发事件
    /// </summary>
    /// <param name="bullet">子弹本体</param>
    /// <param name="target">被击中的角色</param>
    public abstract void Apply(GameObject bullet, GameObject target);
}
