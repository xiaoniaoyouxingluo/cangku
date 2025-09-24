using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色当前的属性
/// </summary>
[System.Serializable]
public struct ChaProperty
{
    public static ChaProperty zero = new ChaProperty();
    ///<summary>
    ///最大生命
    ///</summary>
    public float maxHp;
    /// <summary>
    /// 当前生命
    /// </summary>
    public float nowHp;
    ///<summary>
    ///攻击力
    ///</summary>
    public float atk;
    /// <summary>
    /// 防御力
    /// </summary>
    public float def;
    /// <summary>
    /// 部署费用
    /// </summary>
    public int energy;
    /// <summary>
    /// 闪避几率
    /// </summary>
    public int missRate;
    public ChaProperty(float maxHp,float nowHp,float atk,float def,int energy,int missRate)
    {
        this.maxHp = maxHp;
        this.nowHp = nowHp;
        this.atk = atk;
        this.def = def;
        this.energy = energy;
        this.missRate = missRate;
    }
    public static ChaProperty operator +(ChaProperty a, ChaProperty b)
    {
        return new ChaProperty
            (
            a.maxHp + b.maxHp,
            a.nowHp + b.nowHp,
            a.atk + b.atk,
            a.def + b.def,
            a.energy + b.energy,
            a.missRate + b.missRate
        );
    }
    public static ChaProperty operator *(ChaProperty a, ChaProperty b)
    {
        return a + new ChaProperty(a.maxHp * b.maxHp, a.nowHp * b.nowHp, a.atk * b.atk, a.def * b.def, a.energy * b.energy, a.missRate * b.missRate);
    }
    public static ChaProperty operator *(ChaProperty a, float b)
    {
        return new ChaProperty(
            a.maxHp * b,
            a.nowHp * b,
            a.atk * b,
            a.def * b,
            (int)(a.energy * b),
            (int)(a.missRate * b)
        );
    }
    public static ChaProperty operator *(ChaProperty a, int b)
    {
        return new ChaProperty(
            a.maxHp * b,
            a.nowHp * b,
            a.atk * b,
            a.def * b,
            a.energy * b,
            a.missRate * b
        );
    }
}