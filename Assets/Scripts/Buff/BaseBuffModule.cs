using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// buff触发事件时 依赖的基类
/// </summary>
public abstract class BaseBuffModule : ScriptableObject
{
    /// <summary>
    /// buff在被添加、改变层数触发时，会传递本次改变的层数，只有在这个时候这个变量才有用
    /// </summary>
    [HideInInspector]
    public int modifyStack;
    /// <summary>
    /// buff触发事件
    /// </summary>
    /// <param name="buff">触发事件的buff</param>
    /// <param name="damageInfo">只有在伤害流程中会传入需要处理的DamageInfo，其余情况为null</param>
    /// <param name="targetOrattacker">如果是在被打的时候，这里传入的是攻击者，如果是在攻击的时候，这里传入的是受击者，其余情况为null</param>
    public abstract void Apply(BuffObj buff, DamageInfo damageInfo = null, GameObject targetOrattacker = null);
}
