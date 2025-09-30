using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
    /// 每过一回合增加的费用
    /// </summary>
    public int money_add;
    /// <summary>
    /// 幽灵的血量
    /// </summary>
    public float youlinghp;
    /// <summary>
    /// 将要执行攻击的灵居
    /// </summary>
    private List<GameObject> nowEnemies;
    /// <summary>
    /// 敌方灵居生成数据
    /// </summary>
    private List<List<(int, Vector2Int)>> createAgent = new List<List<(int, Vector2Int)>>();

    public TeamType 正在行动方 = TeamType.Team1;
    public int 回合数 = 0;
    public int 攻击index = -1;
    /// <summary>
    /// 等待创建的敌人
    /// </summary>
    private List<(int, Vector2Int)> waitCreateEnemy = new List<(int, Vector2Int)>();
    /// <summary>
    /// 切换到游戏场景时进行初始化
    /// </summary>
    /// <param name="id">关卡id</param>
    public void InitInfo(string id)
    {
        GhostNum = BinaryDataMgr.Instance.GetTable<GameLevelInfoContainer>().dataDic[id].money;
        money_add = BinaryDataMgr.Instance.GetTable<GameLevelInfoContainer>().dataDic[id].money_add;
        string[] strs = TextUtil.SplitStr(BinaryDataMgr.Instance.GetTable<GameLevelInfoContainer>().dataDic[id].createAgent, 1);
        string[] strs2;
        List<(int, Vector2Int)> createAgentInfo = new List<(int, Vector2Int)>();
        (int, Vector2Int) p;
        for (int i = 0; i < strs.Length; i++)
        {
            strs2 = TextUtil.SplitStr(strs[i], 5);
            for(int j = 0; j < strs2.Length; j++)
            {
                int[] int3 = TextUtil.SplitStrToIntArr(strs2[j], 2);
                p.Item1 = int3[0];
                p.Item2 = new Vector2Int(int3[1], int3[2]);
                createAgentInfo.Add(p);
            }
            createAgent.Add(createAgentInfo);
            createAgentInfo = new List<(int, Vector2Int)>();
        }
        Add回合数();
    }
    /// <summary>
    /// 创建灵居
    /// </summary>
    /// <param name="id">灵居配置id</param>
    /// <param name="bornpos">场上部署位的位置</param>
    /// <param name="teamType">灵居阵营</param>
    /// <returns>创建是否成功</returns>
    public bool CreateAgent(int id, Vector2Int bornpos, TeamType teamType)
    {
        if (teamType == TeamType.Team1)
        {
            if (GameSceneManager.Instance.Line1[bornpos.x, bornpos.y] != null)
                return false;
        }
        else
        {
            if (GameSceneManager.Instance.Line2[bornpos.x, bornpos.y] != null)
                return false;
        }
        GameObject g = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Enemies/" + BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[id].name));//实例化预设体
        if (teamType == TeamType.Team1)
        {
            g.transform.SetParent(GameSceneManager.Instance.玩家可放置砖块[bornpos.x, bornpos.y].transform);
            GameSceneManager.Instance.Line1[bornpos.x, bornpos.y] = g;
        }
        else
        {
            g.transform.SetParent(GameSceneManager.Instance.敌人可放置砖块[bornpos.x, bornpos.y].transform);
            GameSceneManager.Instance.Line2[bornpos.x, bornpos.y] = g;
        }
        g.transform.localScale = Vector2.one;
        g.transform.localPosition = Vector2.zero;
        g.GetComponent<AgentObj>().teamType = teamType;//设置阵营
        g.GetComponent<AgentObj>().InitObj(id);//初始化对象属性  
        GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/SE/GhostInTX2"), g.transform.position, Quaternion.identity);//创建鬼魂特效
        return true;
    }
    /// <summary>
    /// 增加一个回合
    /// </summary>
    private void Add回合数()
    {
        回合数++;
        GameObject 部署位;
        while (waitCreateEnemy.Count > 0)//把等待队列中的敌人创建
        {
            if (!CreateAgent(waitCreateEnemy[0].Item1, waitCreateEnemy[0].Item2, TeamType.Team2))//创建敌人 如果创建失败，进if
            {
                部署位 = GameSceneManager.Instance.GetRandomnDeployment(TeamType.Team2);
                if (部署位 == null)//没有可以创建的部署位了 把本回合需要创建的敌人也放入队列等待下回合创建
                {
                    if (回合数 <= createAgent.Count)
                        for (int i = 0; i < createAgent[回合数 - 1].Count; i++)
                            waitCreateEnemy.Add(createAgent[回合数 - 1][i]);
                    return;
                }
                else
                {
                    CreateAgent(waitCreateEnemy[0].Item1, 部署位.GetComponent<玩家可放入槽位>().pos, TeamType.Team2);
                    waitCreateEnemy.RemoveAt(0);
                }
            }
        }
        if (回合数 <= createAgent.Count)
        {
            List<(int, Vector2Int)> zanShiWaitCreateEnemy = new List<(int, Vector2Int)>();
            for (int i = 0; i < createAgent[回合数-1].Count; i++)
            {
                if (!CreateAgent(createAgent[回合数 - 1][i].Item1, createAgent[回合数 - 1][i].Item2, TeamType.Team2))//创建敌人 如果创建失败，进if
                {
                    zanShiWaitCreateEnemy.Add(createAgent[回合数 - 1][i]);//为了不影响能够在预定位置创建的敌人，先存起来
                }
            }
            for (int i = 0; i < zanShiWaitCreateEnemy.Count; i++)//开始创建第一次创建失败的
            {
                部署位 = GameSceneManager.Instance.GetRandomnDeployment(TeamType.Team2);
                if(部署位==null)//没有可以创建的部署位了 把本回合剩下需要创建的敌人放入队列等待下回合创建
                {
                    for (int j = i; j < zanShiWaitCreateEnemy.Count; j++)
                    {
                        waitCreateEnemy.Add(zanShiWaitCreateEnemy[i]);
                        return;
                    }
                }
                else
                {
                    CreateAgent(zanShiWaitCreateEnemy[i].Item1, 部署位.GetComponent<玩家可放入槽位>().pos, TeamType.Team2);
                }
            }
        }
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
        下一个攻击();
    }
    public void 下一个攻击()
    {
        攻击index++;
        if (nowEnemies.Count > 攻击index)
        {
            nowEnemies[攻击index].GetComponent<AgentObj>().CastPA();
        }
        else
        {
            EventCenter.Instance.EventTrigger<TeamType>("回合结束时", 正在行动方);
            攻击index = -1;
            if (正在行动方 == TeamType.Team1)
            {
                正在行动方 = TeamType.Team2;
                PlayAtk();
            }
            else
            {
                正在行动方 = TeamType.Team1;
                isAtking = false;
                GhostNum += money_add;
                UImanager.Instance.GetPanel<GamePanel>().GetControl<Text>("剩余能量").text = GhostNum.ToString();
                Add回合数();
            }
        }
    }
}
