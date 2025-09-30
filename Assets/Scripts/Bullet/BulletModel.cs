using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///子弹的模板
///</summary>
[System.Serializable]
public struct BulletModel
{
    public string id;
    [Tooltip("子弹可以碰触的次数，每次碰到合理目标-1，到0的时候子弹就结束了")]
    public int hitTimes;
    [Tooltip("子弹命中特效")]
    public GameObject hitTX;
    [Tooltip("子弹命中音效")]
    public string hitSound;
    [Tooltip("子弹被创建的事件")]
    public BaseBulletModule onCreate;

    [Tooltip("子弹命中目标时候发生的事情")]
    public BaseBulletModule onHit;
    [Tooltip("子弹生命周期结束时候发生的事情")]
    public BaseBulletModule onRemoved;

    [Tooltip("子弹是否会命中敌人")]
    public bool hitFoe;

    [Tooltip("子弹是否会命中队友")]
    public bool hitAlly;
}
///<summary>
///子弹被创建的事件
///</summary>
public delegate void BulletOnCreate(GameObject bullet);

///<summary>
///子弹命中目标的时候触发的事件
///<param name="bullet">发生碰撞的子弹，应该是个bulletObj，但是在unity的逻辑下，他就是个GameObject，具体数据从GameObject拿了</param>
///<param name="target">被击中的角色</param>
///<summary>
public delegate void BulletOnHit(GameObject bullet, GameObject target);

///<summary>
///子弹在生命周期消耗殆尽之后发生的事件，生命周期消耗殆尽是因为BulletState.duration<=0，或者是因为移动撞到了阻挡。
///<param name="bullet">发生碰撞的子弹，应该是个bulletObj，但是在unity的逻辑下，他就是个GameObject，具体数据从GameObject拿了</param>
///</summary>
public delegate void BulletOnRemoved(GameObject bullet);
