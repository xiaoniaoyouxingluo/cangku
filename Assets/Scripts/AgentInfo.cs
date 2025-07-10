using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 家具数据结构类
/// </summary>
[System.Serializable]
public class AgentInfo
{
    public string name; //名字
    public string tips; //描述
    public int sellingpPrice; //出售价格
    public int recyclingPrice;  //回收
    public int weight; //商店随机权重
    public int energy;//需要花费的能量
    public string prefabName;
    //public int cost; //费用
    //public int atk; //攻击力
    //public int hp; //血量
}
