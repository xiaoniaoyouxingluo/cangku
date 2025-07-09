using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家数据
/// </summary>
[System.Serializable]
public class PlayerData
{
    public int startEnergy = 10;
    public int nowHealth = 3;
    //当前拥有多少游戏币
    public int haveMoney = 0;
    //携带家具上限
    public int num = 4;
    //商店出售位
    public int sellNum = 10;
}
