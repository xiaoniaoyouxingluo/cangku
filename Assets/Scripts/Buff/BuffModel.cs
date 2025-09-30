using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///策划填表的内容
///</summary>
[CreateAssetMenu(fileName = "BuffData", menuName = "我的文件/BuffData")]
public class BuffModel : ScriptableObject
{
    [Tooltip("buff的id")]
    public string id;

    [Tooltip("buff的名称")]
    public new string name;
    [Tooltip("buff功能描述")]
    public string description;

    [Tooltip("buff的优先级，优先级越低的buff越后面执行")]
    public int priority;

    [Tooltip("最大层数")]
    public int maxStack;
    [Tooltip("buff持续时间结束时层数怎么减少")]
    public BuffRemoveStackUpdate buffRemoveStackUpdate;
    [Tooltip("是否在状态栏上显示")]
    public bool isShow;
    [Tooltip("buff的tag：passiveSkill=被动技能")]
    public string[] tags;


    [Tooltip("buff会给角色添加的属性，这些属性根据这个游戏设计只有2种，plus和times，所以这个数组实际上只有2维")]
    public ChaProperty[] propMod;
    [Tooltip("buff对于角色的ChaControlState的影响")]
    public ChaControlState stateMod;
    [Tooltip("回合开始时会触发的事件")]
    public BaseBuffModule onHuiheStart;
    [Tooltip("回合结束时会触发的事件")]
    public BaseBuffModule onHuiheEnd;
    ///<summary>
    ///buff在被添加、改变层数时候触发的事件
    ///<param name="buff">会传递给脚本buffObj作为参数</param>
    ///<param name="modifyStack">会传递本次改变的层数</param>
    ///</summary>
    //public BuffOnOccur onOccur;
    //public object[] onOccurParams;
    [Tooltip("buff在被添加、改变层数时候触发的事件")]
    public BaseBuffModule onOccur;

    ///<summary>
    ///在这个buffObj被移除之前要做的事情，如果运行之后buffObj又不足以被删除了就会被保留
    ///<param name="buff">会传递给脚本buffObj作为参数</param>
    ///</summary>
    //public BuffOnRemoved onRemoved;
    //public object[] onRemovedParams;
    [Tooltip("在这个buffObj被移除之前要做的事情，如果运行之后buffObj又不足以被删除了就会被保留")]
    public BaseBuffModule onRemoved;
    ///<summary>
    ///在伤害流程中，持有这个buff的人作为攻击者会发生的事情
    ///<param name="buff">会传递给脚本buffObj作为参数</param>
    ///<param name="damageInfo">这次的伤害信息</param>
    ///<param name="target">挨打的角色对象</param>
    ///</summary>
    //public BuffOnHit onHit;
    //public object[] onHitParams;
    [Tooltip("在伤害流程中，持有这个buff的人作为攻击者会发生的事情")]
    public BaseBuffModule onHit;
    ///<summary>
    ///在伤害流程中，持有这个buff的人作为挨打者会发生的事情
    ///<param name="buff">会传递给脚本buffObj作为参数</param>
    ///<param name="damageInfo">这次的伤害信息</param>
    ///<param name="attacker">打我的角色，当然可以是空的</param>
    ///</summary>
    //public BuffOnBeHurt onBeHurt;
    //public object[] onBeHurtParams;
    [Tooltip("在伤害流程中，持有这个buff的人作为挨打者会发生的事情")]
    public BaseBuffModule onBeHurt;
    ///<summary>
    ///在伤害流程中，如果击杀目标，则会触发的啥事情
    ///<param name="buff">会传递给脚本buffObj作为参数</param>
    ///<param name="damageInfo">这次的伤害信息</param>
    ///<param name="target">挨打的角色对象</param>
    ///</summary>
    //public BuffOnKill onKill;
    //public object[] onKillParams;
    [Tooltip("在伤害流程中，如果击杀目标，则会触发的啥事情")]
    public BaseBuffModule onKill;
    ///<summary>
    ///在伤害流程中，持有这个buff的人被杀死了，会触发的事情
    ///<param name="buff">会传递给脚本buffObj作为参数</param>
    ///<param name="damageInfo">这次的伤害信息</param>
    ///<param name="attacker">发起攻击造成击杀的角色对象</param>
    ///</summary>
    //public BuffOnBeKilled onBeKilled;
    //public object[] onBeKilledParams;
    [Tooltip("在伤害流程中，持有这个buff的人被杀死了，会触发的事情")]
    public BaseBuffModule onBeKilled;
    ///<summary>
    ///在伤害流程中，持有这个buff的人可能会被杀死，会触发的事情
    ///<param name="buff">会传递给脚本buffObj作为参数</param>
    ///<param name="damageInfo">这次的伤害信息</param>
    ///<param name="attacker">发起攻击造成击杀的角色对象</param>
    ///</summary>
    //public BuffOnBeforeKilled onBeforeKilled;
    //public object[] onBeforeKilledParams;
    [Tooltip("在伤害流程中，持有这个buff的人可能会被杀死，会触发的事情")]
    public BaseBuffModule onBeforeKilled;
    [Tooltip("在释放技能的时候运行的buff，执行这个buff一般可以用来替换skillModule")]
    public BaseBuffModule onCast;
    public BuffModel(string id, string name, string[] tags, int priority, int maxStack, ChaControlState stateMod, ChaProperty[] propMod = null)
    {
        this.id = id;
        this.name = name;
        this.tags = tags;
        this.priority = priority;
        this.maxStack = maxStack;
        this.stateMod = stateMod;
        this.propMod = new ChaProperty[2]{
            ChaProperty.zero,
            ChaProperty.zero
        };
        if (propMod != null)
        {
            for (int i = 0; i < Mathf.Min(2, propMod.Length); i++)
            {
                this.propMod[i] = propMod[i];
            }
        }
    }
}
