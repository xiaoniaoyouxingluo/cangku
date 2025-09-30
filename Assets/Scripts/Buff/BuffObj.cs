using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///游戏中运行的、角色身上存在的buff
///</summary>
public class BuffObj
{
    ///<summary>
    ///这是个什么buff
    ///</summary>
    public BuffModel model;

    ///<summary>
    ///剩余多久，单位：回合
    ///</summary>
    public float duration;
    ///<summary>
    ///是否是一个永久的buff，永久的duration不会减少，但是timeElapsed还会增加
    ///</summary>
    public bool permanent;
    ///<summary>
    ///当前层数
    ///</summary>
    public int stack;

    ///<summary>
    ///buff的施法者是谁，可以是空的
    ///</summary>
    public GameObject caster;

    ///<summary>
    ///buff的携带者，实际上是作为参数传递给脚本用，具体是谁，可定是所在控件的this.gameObject了
    ///</summary>
    public GameObject carrier;

    ///<summary>
    ///buff已经存在了多少时间了，单位：回合
    ///</summary>
    public float timeElapsed;

    ///<summary>
    ///buff执行了多少次onTick了，如果不会执行onTick，那将永远是0
    ///</summary>
    public int ticked = 0;

    ///<summary>
    ///buff的一些参数，这些参数是逻辑使用的，比如wow中牧师的盾还能吸收多少伤害，就可以记录在buffParam里面
    ///</summary>
    public Dictionary<string, object> buffParam = new Dictionary<string, object>();

    public BuffObj(
        BuffModel model, GameObject caster, GameObject carrier, float duration, int stack, bool permanent = false,
        Dictionary<string, object> buffParam = null
    )
    {
        this.model = model;
        this.caster = caster;
        this.carrier = carrier;
        this.duration = duration;
        this.stack = stack;
        this.permanent = permanent;
        if (buffParam != null)
        {
            foreach (KeyValuePair<string, object> kv in buffParam)
            {
                this.buffParam.Add(kv.Key, kv.Value);
            }
        }
    }
}
///<summary>
///用于添加一条buff的信息
///</summary>
[System.Serializable]
public class AddBuffInfo
{
    ///<summary>
    ///buff的负责人是谁，可以是null
    ///</summary>
    [HideInInspector]
    public GameObject caster;
    [Tooltip("是否要把负责人记录下来")]
    public bool isCaster;

    ///<summary>
    ///buff要添加给谁，这个必须有
    ///</summary>
    [HideInInspector]
    public GameObject target;

    ///<summary>
    ///buff的model，这里当然可以从数据里拿，也可以是逻辑脚本现生成的
    ///</summary>
    public BuffModel buffModel;

    ///<summary>
    ///要添加的层数，负数则为减少
    ///</summary>
    public int addStack;

    ///<summary>
    ///关于时间，是改变还是设置为
    ///</summary>
    public E_duration durationSetTo;

    ///<summary>
    ///是否是一个永久的buff，即便=true，时间设置也是有意义的，因为时间如果被减少到0以下，即使是永久的也会被删除
    ///</summary>
    public bool permanent;

    ///<summary>
    ///时间值，单位：回合
    ///</summary>
    public float duration;

    ///<summary>
    ///buff的一些参数，这些参数是逻辑使用的，比如wow中牧师的盾还能吸收多少伤害，就可以记录在buffParam里面
    ///</summary>
    public Dictionary<string, object> buffParam;

    public AddBuffInfo(
        BuffModel model, GameObject caster, GameObject target,
        int stack, float duration, E_duration durationSetTo = E_duration.max,
        bool permanent = false, bool isCaster = false,
        Dictionary<string, object> buffParam = null
    )
    {
        this.buffModel = model;
        this.caster = caster;
        this.target = target;
        this.addStack = stack;
        this.duration = duration;
        this.durationSetTo = durationSetTo;
        this.buffParam = buffParam;
        this.permanent = permanent;
        this.isCaster = isCaster;
    }
    public AddBuffInfo(
        BuffModel model, GameObject caster, GameObject target,
        int stack, float duration, E_duration durationSetTo = E_duration.max,
        bool permanent = false,
        Dictionary<string, object> buffParam = null
    )
    {
        this.buffModel = model;
        this.caster = caster;
        this.target = target;
        this.addStack = stack;
        this.duration = duration;
        this.durationSetTo = durationSetTo;
        this.buffParam = buffParam;
        this.permanent = permanent;
    }
}
/// <summary>
/// 时间叠加方式
/// </summary>
public enum E_duration
{
    setTo,//设置为
    addTo,//相加
    max,//取较大的时间
    min//取较小的时间
}
/// <summary>
/// buff持续时间结束时层数怎么减少
/// </summary>
public enum BuffRemoveStackUpdate
{
    /// <summary>
    /// 层数清空
    /// </summary>
    Clear,
    /// <summary>
    /// 层数减一
    /// </summary>
    Reduce,
    /// <summary>
    /// 层数减半（向下取整）
    /// </summary>
    Half
}
public delegate void BuffOnOccur(BuffObj buff, int modifyStack);
public delegate void BuffOnRemoved(BuffObj buff);
public delegate void BuffOnHit(BuffObj buff, ref DamageInfo damageInfo, GameObject target);
public delegate void BuffOnBeHurt(BuffObj buff, ref DamageInfo damageInfo, GameObject attacker);
public delegate void BuffOnKill(BuffObj buff, DamageInfo damageInfo, GameObject target);
public delegate void BuffOnBeKilled(BuffObj buff, DamageInfo damageInfo, GameObject attacker);
public delegate void BuffOnBeforeKilled(BuffObj buff, ref DamageInfo damageInfo, GameObject attacker);
