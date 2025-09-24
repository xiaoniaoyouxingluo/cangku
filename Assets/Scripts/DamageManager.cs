using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 负责处理游戏中所有的DamageInfo
/// </summary>
public class DamageManager : MonoBehaviour
{
    private DamageManager instance;
    public DamageManager Instance => instance;
    /// <summary>
    /// 记录游戏中所有的DamageInfo
    /// </summary>
    private Queue<DamageInfo> damageQueue = new Queue<DamageInfo>();
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        while (damageQueue.Count > 0)
        {
            DealWithDamage(damageQueue.Dequeue());//从队列中取出DamageInfo进行处理
        }
    }

    /// <summary>
    /// 处理DamageInfo的流程，也就是整个游戏的伤害流程
    /// </summary>
    /// <param name="dInfo">要处理的damageInfo</param>
    private void DealWithDamage(DamageInfo dInfo)
    {
        int dVal;
        //如果目标已经挂了，就直接return了
        if (dInfo.defender == null)
            return;
        AgentObj defenderAgentObj = dInfo.defender.GetComponent<AgentObj>();
        if (defenderAgentObj == null)
            return;
        if (defenderAgentObj.dead) //判断目标有没有死亡
            return;
        AgentObj attackerAgentObj = null;
        //遍历攻击者所有的buff.onHit
        if (dInfo.attacker)
        {
            attackerAgentObj = dInfo.attacker.GetComponent<AgentObj>();
            for (int i = 0; i < attackerAgentObj.buffs.Count; i++)
            {
                attackerAgentObj.buffs[i].model.onHit?.Apply(attackerAgentObj.buffs[i], dInfo, dInfo.defender);
            }
        }
        //遍历挨打者所有的buff.beHurt
        for (int i = 0; i < defenderAgentObj.buffs.Count; i++)
        {
            defenderAgentObj.buffs[i].model.onBeHurt?.Apply(defenderAgentObj.buffs[i], dInfo, dInfo.attacker);
        }
        if (defenderAgentObj.CanBeKilledByDamageInfo(dInfo))//第一次判断此时DamageInfo能不能杀死受击者
        {
            //如果角色可能被杀死，就会走onBeforeKilled
            for (int i = 0; i < defenderAgentObj.buffs.Count; i++)
            {
                defenderAgentObj.buffs[i].model.onBeforeKilled?.Apply(defenderAgentObj.buffs[i], dInfo, dInfo.attacker);
            }
        }
        if (defenderAgentObj.CanBeKilledByDamageInfo(dInfo))//第二次判断此时DamageInfo能不能杀死受击者
        {
            //如果角色会被杀死，就会走OnKill和OnBeKilled
            if (attackerAgentObj != null)
            {
                for (int i = 0; i < attackerAgentObj.buffs.Count; i++)
                {
                    attackerAgentObj.buffs[i].model.onKill?.Apply(attackerAgentObj.buffs[i], dInfo, dInfo.defender);
                }
            }
            for (int i = 0; i < defenderAgentObj.buffs.Count; i++)
            {
                defenderAgentObj.buffs[i].model.onBeKilled?.Apply(defenderAgentObj.buffs[i], dInfo, dInfo.attacker);
            }
        }
        defenderAgentObj.Wound(dInfo);

        //伤害流程走完，添加buff
        for (int i = 0; i < dInfo.addBuffs.Count; i++)
        {
            GameObject toCha = dInfo.addBuffs[i].target;
            AgentObj toAgentObj = toCha.Equals(dInfo.attacker) ? attackerAgentObj : defenderAgentObj;

            if (toAgentObj != null && toAgentObj.dead == false)
            {
                toAgentObj.AddBuff(dInfo.addBuffs[i]);
            }
        }
    }

    ///<summary>
    ///添加一个damageInfo
    ///<param name="attacker">攻击者，可以为null</param>
    ///<param name="target">挨打对象</param>
    ///<param name="damage">基础伤害值</param>
    ///<param name="tags">伤害信息类型</param>
    ///</summary>
    public void DoDamage(GameObject attacker, GameObject target, Damage damage, DamageInfoTag[] tags)
    {
        damageQueue.Enqueue(new DamageInfo(attacker, target, damage, tags));
    }
}
