using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///游戏中任何一次伤害、治疗等逻辑，都会产生一条damageInfo，由此开始正常的伤害流程，而不是直接改写hp
///值得一提的是，在类似“攻击时产生额外一次伤害”这种效果中，额外一次伤害也应该是一个damageInfo。
///</summary>
public class DamageInfo
{
    ///<summary>
    ///造成伤害的攻击者，当然可以是null的
    ///</summary>
    public GameObject attacker;

    ///<summary>
    ///造成攻击伤害的受击者，这个必须有
    ///</summary>
    public GameObject defender;

    ///<summary>
    ///这次伤害的类型Tag，这个会被用于buff相关的逻辑，是一个极其重要的信息
    ///这里是策划根据游戏设计来定义的，比如游戏中可能存在"frozen" "fire"之类的伤害类型，还会存在"directDamage" "period" "reflect"之类的类型伤害
    ///根据这些伤害类型，逻辑处理可能会有所不同，典型的比如"reflect"，来自反伤的，那本身一个buff的作用就是受到伤害的时候反弹伤害，如果双方都有这个buff
    ///并且这个buff没有判断damageInfo.tags里面有reflect，则可能造成“短路”，最终有一下有一方就秒了。
    ///</summary>
    public DamageInfoTag[] tags;

    ///<summary>
    ///伤害值/治疗值
    ///</summary>
    public Damage damage;

    ///<summary>
    ///闪避率
    ///</summary>
    public float missRate;
    /// <summary>
    /// 闪避是否成功
    /// </summary>
    public bool isMiss;

    ///<summary>
    ///伤害过后，给角色添加的buff
    ///</summary>
    public List<AddBuffInfo> addBuffs = new List<AddBuffInfo>();

    public DamageInfo(GameObject attacker, GameObject defender, Damage damage, DamageInfoTag[] tags)
    {
        this.attacker = attacker;
        this.defender = defender;
        this.damage = damage;
        this.missRate = defender.GetComponent<AgentObj>().nowproperty.missRate;
        this.isMiss = Random.Range(0.00f, 1.00f) <= missRate;
        this.tags = new DamageInfoTag[tags.Length];
        for (int i = 0; i < tags.Length; i++)
        {
            this.tags[i] = tags[i];
        }
    }

    ///<summary>
    ///根据tag判断，这是否是一次治疗
    ///</summary>
    public bool isHeal()
    {
        for (int i = 0; i < this.tags.Length; i++)
        {
            if (tags[i] == DamageInfoTag.directHeal || tags[i] == DamageInfoTag.periodHeal)
            {
                return true;
            }
        }
        return false;
    }

    ///<summary>
    ///根据tag决定是否要播放受伤动作，当然你还可以是根据类型决定不同的受伤动作，但是我这个demo就没这么复杂了
    ///</summary>
    public bool requireDoHurt()
    {
        for (int i = 0; i < this.tags.Length; i++)
        {
            if (tags[i] == DamageInfoTag.directDamage)
            {
                return true;
            }
        }
        return false;
    }

    ///<summary>
    ///将添加buff信息添加到伤害信息中来
    ///buffOnHit\buffBeHurt\buffOnKill\buffBeKilled等伤害流程张的buff添加通常走这里
    ///<param name="info">要添加的buff的信息</param>
    ///</summary>
    public void AddBuffToCha(AddBuffInfo buffInfo)
    {
        this.addBuffs.Add(buffInfo);
    }
}

///<summary>
///游戏中伤害值的struct 正数是伤害 负数是治疗
///</summary>
[System.Serializable]
public struct Damage
{
    /// <summary>
    /// 物理伤害
    /// </summary>
    public float bullet;
    /// <summary>
    /// 真实伤害
    /// </summary>
    public float explosion;

    public Damage(float bullet, float explosion = 0)
    {
        this.bullet = bullet;
        this.explosion = explosion;
    }

    /// <summary>
    /// 统计规则，在这个游戏里伤害和治疗不能共存在一个结果里，作为抵消用
    /// </summary>
    /// <param name="asHeal">是否当做治疗来统计</param>
    /// <returns>伤害/治疗总量</returns>
    public float Overall(bool asHeal = false)
    {
        return (asHeal) ? (Mathf.Min(0, bullet) + Mathf.Min(0, explosion)) : (Mathf.Max(0, bullet) + Mathf.Max(0, explosion));
    }

    public static Damage operator +(Damage a, Damage b)
    {
        return new Damage(a.bullet + b.bullet, a.explosion + b.explosion);
    }
    public static Damage operator *(Damage a, float b)
    {
        return new Damage(a.bullet * b, a.explosion * b);
    }
}

///<summary>
///伤害类型的Tag元素，因为DamageInfo的逻辑需要的严谨性远高于其他的元素，所以伤害类型应该是枚举数组的
///这个伤害类型不应该是类似 火伤害、水伤害、毒伤害之类的，如果是这种元素伤害，那么应该是在damage做文章，即damange不是int而是一个struct或者array或者dictionary，然后DamageValue函数里面去改最终值算法
///这里的伤害类型，指的还是比如直接伤害、反弹伤害、dot伤害等等，一些在逻辑处理流程会有不同待遇的东西，比如dot伤害可能不会触发一些效果等，当然这最终还是取决于策划设计的规则。
///</summary>
public enum DamageInfoTag
{
    /// <summary>
    /// 直接伤害
    /// </summary>
    directDamage = 0,
    /// <summary>
    /// 间歇性伤害
    /// </summary>
    periodDamage = 1,
    /// <summary>
    /// 反噬伤害
    /// </summary>
    reflectDamage = 2,  
    /// <summary>
    /// 直接治疗
    /// </summary>
    directHeal = 10,
    /// <summary>
    /// 间歇性治疗
    /// </summary>
    periodHeal = 11,    
}
