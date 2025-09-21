using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_jingjuType
{
    putong,
    xiyu,
    zhishi,
    chuju
}
/// <summary>
/// 玩家对象属性类
/// </summary>
public class AgentProperty : FightProperty
{
    /// <summary>
    /// 护盾值
    /// </summary>
    public float shield;
    /// <summary>
    /// 技能词条描述
    /// </summary>
    public string tipstxt;
    /// <summary>
    /// 灵居类型
    /// </summary>
    public E_jingjuType jingjuType;
    /// <summary>
    /// 商店出售价格
    /// </summary>
    public int sellingpPrice;
    /// <summary>
    /// 商店回收价格
    /// </summary>
    public int recyclingPrice;
    /// <summary>
    /// 稀有度
    /// </summary>
    public int rarity;
    /// <summary>
    /// 部署费用
    /// </summary>
    public int energy;
    /// <summary>
    /// 主动技能ID
    /// </summary>
    public List<int> skillIDs = new List<int>();
    /// <summary>
    /// 被动技能ID
    /// </summary>
    public List<int> passiveSkillIDs = new List<int>();

    public AgentProperty():base()
    {

    }

    /// <summary>
    /// 初始化属性信息的方法
    /// </summary>
    /// <param name="ID">ID</param>
    public override void SetData(int id)
    {
        //得到角色配置表当中对应的数据 用于初始化属性
        AgentInfo agentInfo = BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[id];
        //初始化属性
        this.uID = id;
        this.level = agentInfo.level;

        this.name = agentInfo.name;
        this.resName = agentInfo.prefabName;

        this.curHP = this.maxHP = agentInfo.hp;

        this.atk = agentInfo.atk;
        this.def = agentInfo.defense;
        shield = agentInfo.shield;
        tipstxt = agentInfo.tipstxt;
        jingjuType = (E_jingjuType)agentInfo.type;
        sellingpPrice = agentInfo.sellingpPrice;
        recyclingPrice = agentInfo.recyclingPrice;
        rarity = agentInfo.rarity;
        energy = agentInfo.cost;
        missRate = agentInfo.missRate;
    }
}
