using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 游戏关卡管理器
/// </summary>
public class GameLevelMgr : BaseManager<GameLevelMgr>
{
    private GameLevelMgr(){ }
    public List<GameObject> 在场上的灵居 = new List<GameObject>();
    public List<int> 临时灵居 = new List<int>();
    /// <summary>
    /// 灵居攻击中
    /// </summary>
    public bool isAtking;
    /// <summary>
    /// 剩余的能量
    /// </summary>
    public int GhostNum;
    /// <summary>
    /// 幽灵的血量
    /// </summary>
    public float youlinghp;
    /// <summary>
    /// 将要执行攻击的灵居
    /// </summary>
    List<GameObject> nowEnemies;

    public TeamType 正在行动方 = TeamType.Team1;
    /// <summary>
    /// 切换到游戏场景时进行初始化
    /// </summary>
    public void InitInfo()
    {
        GhostNum = 100;
    }
    /// <summary>
    /// 开始攻击
    /// </summary>
    public void PlayAtk()
    {
        isAtking = true;
        nowEnemies = new List<GameObject>();//我方将要执行攻击的灵居
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if(正在行动方==TeamType.Team1)
                {
                    if (GameSceneManager.Instance.Line1[i, j] != null)
                        nowEnemies.Add(GameSceneManager.Instance.Line1[i, j]);
                }
                else
                {
                    if (GameSceneManager.Instance.Line2[i, j] != null)
                        nowEnemies.Add(GameSceneManager.Instance.Line2[i, j]);
                }
        EventCenter.Instance.EventTrigger<TeamType>("回合开始时", 正在行动方);
        下一个攻击(0);
    }
    public void 下一个攻击(int index)
    {
        if(nowEnemies.Count>index)
        {
            nowEnemies[index].GetComponent<AgentObj>().CastPA(index);
        }
        else
        {
            EventCenter.Instance.EventTrigger<TeamType>("回合结束时", 正在行动方);
            if (正在行动方 == TeamType.Team1)
            {
                正在行动方 = TeamType.Team2;
                PlayAtk();
            }
            else
            {
                正在行动方 = TeamType.Team1;
                isAtking = false;
            }

        }
    }
}
