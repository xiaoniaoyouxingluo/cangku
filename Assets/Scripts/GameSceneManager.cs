using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// 获取什么样的灵居
/// </summary>
public enum xunzhaolingjuのtiaojian
{
    Normal,//正常的
    NowHpMin,//当前血量最少
    NowHpMax,//当前血量最多
    HpMin,//生命值最低
    HpMax,//生命值最高
    AtkMin,//攻击力最低
    AtkMax//攻击力最高
}
/// <summary>
/// 范围检测
/// </summary>
public enum E_Overlap
{
    /// <summary>
    /// 一行
    /// </summary>
    line,
    /// <summary>
    /// 一列
    /// </summary>
    column,
    /// <summary>
    /// 半场全部
    /// </summary>
    all
}
/// <summary>
/// 游戏场景管理器 负责获取场上的部署位 对应部署位的灵居
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager instance;
    public static GameSceneManager Instance => instance;
    /// <summary>
    /// 我方部署位置
    /// </summary>
    public GameObject[,] Line1 = new GameObject[3, 3];
    /// <summary>
    /// 敌方部署位置
    /// </summary>
    public GameObject[,] Line2 = new GameObject[3, 3];
    public GameObject[] 攻击位 = new GameObject[3];
    public GameObject[,] 玩家可放置砖块 = new GameObject[3, 3];
    public GameObject[,] 敌人可放置砖块 = new GameObject[3, 3];
    private void Awake()
    {
        instance = this;
        GameObject[] gs = GameObject.FindGameObjectsWithTag("Map");
        Vector2Int v;
        for(int i = 0; i < gs.Length; i++)
        {
            v = gs[i].GetComponent<玩家可放入槽位>().pos;
            if (gs[i].transform.parent.name == "w玩家")
                玩家可放置砖块[v.x, v.y] = gs[i];
            else
                敌人可放置砖块[v.x, v.y] = gs[i];
        }
    }
    private void Start()
    {
        
    }
    /// <summary>
    /// 获取一个随机的部署位
    /// </summary>
    /// <param name="teamType">获取哪个阵营的</param>
    /// <param name="isNull">部署位是否是空的</param>
    /// <returns></returns>
    public GameObject GetRandomnDeployment(TeamType teamType, bool isNull = true)
    {
        if (isNull)
        {
            List<GameObject> aa可选的 = new List<GameObject>();
            if (teamType == TeamType.Team1)
            {
                for (int i = 0; i < 玩家可放置砖块.GetLength(0); i++)
                    for (int j = 0; j < 玩家可放置砖块.GetLength(1); j++)
                        if (玩家可放置砖块[i, j].transform.childCount == 0)
                            aa可选的.Add(玩家可放置砖块[i, j]);
            }
            else
            {
                for (int i = 0; i < 敌人可放置砖块.GetLength(0); i++)
                    for (int j = 0; j < 敌人可放置砖块.GetLength(1); j++)
                        if (敌人可放置砖块[i, j].transform.childCount == 0)
                            aa可选的.Add(敌人可放置砖块[i, j]);
            }
            if (aa可选的.Count == 0)
                return null;
            return aa可选的[Random.Range(0, aa可选的.Count)];
        }
        else
        {
            if (teamType == TeamType.Team1)
                return 玩家可放置砖块[Random.Range(0, 玩家可放置砖块.GetLength(0)), Random.Range(0, 玩家可放置砖块.GetLength(1))];
            else
                return 敌人可放置砖块[Random.Range(0, 敌人可放置砖块.GetLength(0)), Random.Range(0, 敌人可放置砖块.GetLength(1))];
        }
    }
    /// <summary>
    /// 获取一行中排在最前面的灵居
    /// </summary>
    /// <param name="teamType">获取哪个阵营的</param>
    /// <param name="line">行数</param>
    /// <param name="objs">此参数中的对象将被排除</param>
    /// <returns></returns>
    public GameObject GetThisLineFirstEnemy(TeamType teamType, int line)
    {
        if (line > Line1.GetLength(1) - 1 || line < 0)
            return null;
        if (teamType == TeamType.Team1)
        {
            for (int i = Line2.GetLength(1) - 1; i >= 0; i--)
            {
                if (Line1[i, line] != null && Line1[i, line].GetComponent<AgentObj>().dead == false)
                    return Line1[i, line];
            }
        }
        else
        {
            for (int i = 0; i < Line1.GetLength(1); i++)
            {
                if (Line2[i, line] != null && Line2[i, line].GetComponent<AgentObj>().dead == false)
                    return Line2[i, line];
            }
        }
        return null;
    }
    /// <summary>
    /// 获取一行中所有灵居
    /// </summary>
    /// <param name="teamType">获取哪个阵营的</param>
    /// <param name="line">行数</param>
    /// <param name="objs">此参数中的对象将被排除</param>
    /// <returns></returns>
    public List<GameObject> GetThisLineAllFirstEnemy(TeamType teamType, int line, GameObject objs = null)
    {
        if (line > Line1.GetLength(1) - 1 || line < 0)
            return null;
        List<GameObject> list = new List<GameObject>();
        if (teamType == TeamType.Team1)
        {
            for (int i = 0; i < Line1.GetLength(1); i++)
            {
                if (Line1[i, line] != null && Line1[i, line].GetComponent<AgentObj>().dead == false && Line1[i, line] != objs)
                    list.Add(Line1[i, line]);
            }
        }
        else
        {
            for (int i = Line2.GetLength(1) - 1; i >= 0; i--)
            {
                if (Line2[i, line] != null && Line2[i, line].GetComponent<AgentObj>().dead == false && Line2[i, line] != objs)
                    list.Add(Line2[i, line]);
            }
        }
        return list;
    }
    /// <summary>
    /// 获取符合条件的灵居
    /// </summary>
    /// <param name="teamType">获取哪个阵营的</param>
    /// <param name="x">条件</param>
    /// <param name="objs">此参数中的对象将被排除</param>
    /// <returns></returns>
    public GameObject GetThisEnemy(TeamType teamType, xunzhaolingjuのtiaojian x, params GameObject[] objs)
    {
        AgentObj g = null;
        GameObject[,] line = teamType == TeamType.Team1 ? Line1 : Line2;
        for (int i = 0; i < line.GetLength(0); i++)
        {
            for (int j = 0; j < line.GetLength(1); j++)
            {
                if (line[i, j] == null || (objs != null && objs.Contains(line[i, j])))
                    continue;
                if (g == null)
                {
                    g = line[i, j].GetComponent<AgentObj>();
                    continue;
                }
                switch (x)
                {
                    case xunzhaolingjuのtiaojian.NowHpMin:
                        if (g.nowproperty.nowHp > line[i, j].GetComponent<AgentObj>().nowproperty.nowHp)
                            g = line[i, j].GetComponent<AgentObj>();
                        break;
                    case xunzhaolingjuのtiaojian.NowHpMax:
                        if (g.nowproperty.nowHp < line[i, j].GetComponent<AgentObj>().nowproperty.nowHp)
                            g = line[i, j].GetComponent<AgentObj>();
                        break;
                    case xunzhaolingjuのtiaojian.HpMin:
                        if (g.nowproperty.maxHp > line[i, j].GetComponent<AgentObj>().nowproperty.maxHp)
                            g = line[i, j].GetComponent<AgentObj>();
                        break;
                    case xunzhaolingjuのtiaojian.HpMax:
                        if (g.nowproperty.maxHp < line[i, j].GetComponent<AgentObj>().nowproperty.maxHp)
                            g = line[i, j].GetComponent<AgentObj>();
                        break;
                    case xunzhaolingjuのtiaojian.AtkMin:
                        if (g.nowproperty.atk > line[i, j].GetComponent<AgentObj>().nowproperty.atk)
                            g = line[i, j].GetComponent<AgentObj>();
                        break;
                    case xunzhaolingjuのtiaojian.AtkMax:
                        if (g.nowproperty.atk < line[i, j].GetComponent<AgentObj>().nowproperty.atk)
                            g = line[i, j].GetComponent<AgentObj>();
                        break;
                }
            }
        }
        return g ? g.gameObject : null;
    }
    /// <summary>
    /// 获取场上拥有对应buff的灵居
    /// </summary>
    /// <param name="teamType">获取哪个阵营的</param>
    /// <param name="id">buff的model的id</param>
    /// <param name="caster">如果caster不是空，那么就代表只有buffObj.caster在caster里面的才符合条件</param>
    /// <returns></returns>
    public List<GameObject> GetThisEnemy(TeamType teamType, string id, List<GameObject> caster = null)
    {
        GameObject[,] line = teamType == TeamType.Team1 ? Line1 : Line2;
        List<GameObject> g = new List<GameObject>();
        for (int i = 0; i < line.GetLength(0); i++)
        {
            for (int j = 0; j < line.GetLength(1); j++)
            {
                if (line[i, j].GetComponent<AgentObj>().GetBuffById(id, caster).Count > 0)
                    g.Add(line[i, j]);
            }
        }
        return g;
    }
    /// <summary>
    /// 获取范围内的灵居
    /// </summary>
    /// <param name="teamType">获取哪个阵营的</param>
    /// <param name="center">中心点</param>
    /// <param name="overlap">范围的形状</param>
    /// <param name="caster">列表中的对象不会被返回</param>
    /// <returns></returns>
    public List<GameObject> Overlap(TeamType teamType, Vector2Int center, E_Overlap overlap, List<GameObject> caster = null)
    {
        if (center.y > Line1.GetLength(1) - 1 || center.y < 0 || center.x > Line1.GetLength(0) - 1 || center.x < 0)
            return new List<GameObject>();
        List<GameObject> g = new List<GameObject>();
        GameObject[,] line = teamType == TeamType.Team1 ? Line1 : Line2;
        switch (overlap)
        {
            case E_Overlap.line:
                if (caster != null && caster.Count > 0)
                    return GetThisLineAllFirstEnemy(teamType, center.y, caster[0]);
                else
                    return GetThisLineAllFirstEnemy(teamType, center.y);
            case E_Overlap.column:
                for (int i = 0; i < line.GetLength(1); i++)
                {
                    if (line[center.x, i] != null && (caster == null || caster.Count <= 0 || !caster.Contains(line[center.x, i])))
                        g.Add(line[center.x, i]);
                }
                break;
            case E_Overlap.all:
                for (int i = 0; i < line.GetLength(0); i++)
                {
                    for (int j = 0; j < line.GetLength(1); j++)
                    {
                        if (line[i, j] != null && (caster == null || caster.Count <= 0 || !caster.Contains(line[i, j])))
                            g.Add(line[i, j]);
                    }
                }
                break;
        }
        return g;
    }
}
