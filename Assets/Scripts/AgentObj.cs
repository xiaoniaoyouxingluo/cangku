using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家对象
/// </summary>
public class AgentObj : FightObj
{
    public override void InitObj(int id)
    {
        base.InitObj(id);
        //初始化玩家属性
        _property = new PlayerProperty();
        _property.SetData(id);
    }
}
