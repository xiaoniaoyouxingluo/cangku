using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏关卡管理器
/// </summary>
public class GameLevelMgr : BaseManager<GameLevelMgr>
{
    private GameLevelMgr(){ }
    public List<GameObject> 在场上的灵居 = new List<GameObject>();
    public List<int> 临时灵居 = new List<int>();
    /// <summary>
    /// 剩余的能量
    /// </summary>
    public int GhostNum;
    /// <summary>
    /// 幽灵的血量
    /// </summary>
    public float youlinghp;
    /// <summary>
    /// 切换到游戏场景时进行初始化
    /// </summary>
    public void InitInfo()
    {
        GhostNum = 100;
    }
}
