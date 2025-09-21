using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家对象
/// </summary>
public class AgentObj : FightObj
{
    /// <summary>
    /// 角色当前的属性
    /// </summary>
    public ChaProperty nowproperty;
    ///<summary>
    ///角色来自buff的属性
    ///第一个加，第二个百分比乘
    ///</summary>
    public ChaProperty[] buffProp = new ChaProperty[2] { ChaProperty.zero, ChaProperty.zero };
    /// <summary>
    /// 属于玩家还是敌方
    /// </summary>
    public TeamType teamType;
    /// <summary>
    /// 角色是否死亡
    /// </summary>
    public bool dead;
    ///<summary>
    ///角色身上的buff
    ///</summary>
    public List<BuffObj> buffs = new List<BuffObj>();
    /// <summary>
    /// 角色从配置表中读取的数据
    /// </summary>
    public AgentProperty Property
    {
        get
        {
            return _property as AgentProperty;
        }
    }
    protected override void Update()
    {

    }
    public override void InitObj(int id)
    {
        base.InitObj(id);
        //初始化玩家属性
        _property = new AgentProperty();
        _property.SetData(id);
        nowproperty.nowHp = Property.maxHP;
        AttrRecheck();
    }
    /// <summary>
    /// 攻击动画中调用的攻击事件
    /// </summary>
    public void Attack()
    {

    }
    ///<summary>
    ///重新计算所有属性，并且获得一个最终属性
    ///</summary>
    private void AttrRecheck()
    {
        ChaProperty c = new ChaProperty(Property.maxHP, nowproperty.nowHp, Property.atk, Property.def, Property.energy, Property.missRate);
        nowproperty = (c + buffProp[0]) * buffProp[1];
    }
    /// <summary>
    /// 判断这个角色是否会被这个damageInfo所杀
    /// </summary>
    /// <param name="damageInfo">要判断的damageInfo</param>
    /// <returns>是否会被击杀</returns>
    public bool CanBeKilledByDamageInfo(DamageInfo damageInfo)
    {
        if (damageInfo.isHeal() || damageInfo.isMiss)
            return false;
        Damage damage = damageInfo.damage;
        damage.bullet -= nowproperty.def;
        if (damage.Overall() >= nowproperty.nowHp + Property.shield || damage.explosion >= nowproperty.nowHp)
            return true;
        else
            return false;
    }
    /// <summary>
    /// 灵居受伤/加血
    /// </summary>
    /// <param name="damageInfo"></param>
    public void Wound(DamageInfo damageInfo)
    {
        if (dead)
            return;
        if(damageInfo.isHeal())
        {
            nowproperty.nowHp = Mathf.Clamp(nowproperty.nowHp - damageInfo.damage.Overall(true), 0, nowproperty.maxHp);
        }
        else
        {
            if(damageInfo.isMiss)
            {

            }
            else
            {
                Damage damage = damageInfo.damage;
                damage.bullet -= nowproperty.def;//用防御力减掉物理伤害
                Property.shield -= damage.bullet;//先扣除物理伤害对护盾
                if (Property.shield < 0)
                {
                    nowproperty.nowHp -= Mathf.Abs(Property.shield);
                    Property.shield = 0;
                }
                nowproperty.nowHp -= damage.explosion;//结算真实伤害
                nowproperty.nowHp = Mathf.Clamp(nowproperty.nowHp, 0, nowproperty.maxHp);//让血量显示在正确范围内
                if (nowproperty.nowHp <= 0)
                {
                    Kill();
                }
            }
        }
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public void Kill()
    {
        dead = true;
    }
    /// <summary>
    /// 为角色添加buff，当然，删除也是走这个的
    /// </summary>
    /// <param name="buff">buff</param>
    public void AddBuff(AddBuffInfo buff)
    {
        List<GameObject> bCaster = new List<GameObject>();//负责人列表
        if (buff.caster) bCaster.Add(buff.caster);//把buff的负责人加入列表
        List<BuffObj> hasOnes = GetBuffById(buff.buffModel.id, bCaster);//寻找列表中id和创建者都相同的buff
        int modStack;//改变的层数
        bool toRemove = false;
        BuffObj toAddBuff = null;
        if (hasOnes.Count > 0)
        {
            //列表中有已经存在的相同buff
            hasOnes[0].buffParam = new Dictionary<string, object>();
            if (buff.buffParam != null)
            {
                foreach (KeyValuePair<string, object> kv in buff.buffParam) 
                {
                    hasOnes[0].buffParam[kv.Key] = kv.Value;
                };
            }
            hasOnes[0].duration = buff.durationSetTo ? buff.duration : buff.duration + hasOnes[0].duration;//剩余时间计算
            int afterAdd = hasOnes[0].stack;//之前的层数
            hasOnes[0].stack = Mathf.Clamp(hasOnes[0].stack + buff.addStack, 0, buff.buffModel.maxStack);//现在的层数
            modStack = hasOnes[0].stack - afterAdd;
            hasOnes[0].permanent = buff.permanent;
            toAddBuff = hasOnes[0];
            toRemove = hasOnes[0].stack <= 0;
        }
        else
        {
            //新建
            toAddBuff = new BuffObj(
                buff.buffModel,
                buff.caster,
                this.gameObject,
                buff.duration,
                buff.addStack,
                buff.permanent,
                buff.buffParam
            );
            buffs.Add(toAddBuff);
            buffs.Sort((a, b) =>
            {
                return a.model.priority.CompareTo(b.model.priority);//优先级排序，数字小的在前面
            });
            modStack = buff.addStack;
        }
        if (toRemove == false )
        {
            buff.buffModel.onOccur?.Invoke(toAddBuff, modStack);
        }
        AttrRecheck();
    }
    ///<summary>
    ///获取角色身上对应的buffObj
    ///<param name="id">buff的model的id</param>
    ///<param name="caster">如果caster不是空，那么就代表只有buffObj.caster在caster里面的才符合条件</param>
    ///<return>符合条件的buffObj数组</return>
    ///</summary>
    public List<BuffObj> GetBuffById(string id, List<GameObject> caster = null)
    {
        List<BuffObj> res = new List<BuffObj>();
        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].model.id == id && (caster == null || caster.Count <= 0 || caster.Contains(buffs[i].caster)))
            {
                res.Add(buffs[i]);
            }
        }
        return res;
    }
}
