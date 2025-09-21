using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗对象属性
/// </summary>
public class FightProperty : BaseObjProperty
{
    /// <summary>
    /// 等级
    /// </summary>
    public int level;
    /// <summary>
    /// 当前血量
    /// </summary>
    public float curHP;
    /// <summary>
    /// 最大血量
    /// </summary>
    public float maxHP;
    /// <summary>
    /// 攻击力
    /// </summary>
    public float atk;
    /// <summary>
    /// 防御力
    /// </summary>
    public float def;
    /// <summary>
    /// 闪避几率
    /// </summary>
    public int missRate;

    /// <summary>
    /// 动作音效对应关系
    /// </summary>
    private Dictionary<string, string> _actionSound = new Dictionary<string, string>();
    /// <summary>
    /// 动作特效对应关系
    /// </summary>
    private Dictionary<string, string> _actionEff = new Dictionary<string, string>();


    public FightProperty() : base()
    {
        level = 0;
        curHP = 0;
        maxHP = 0;
        atk = 0;
        def = 0;
        missRate = 0;
    }
}
