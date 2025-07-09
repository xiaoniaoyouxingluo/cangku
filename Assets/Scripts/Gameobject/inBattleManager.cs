using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyUnit
{
    public List<GameObject> Enemies;
}
public class inBattleManager : MonoBehaviour
{
    private List<AgentInfo> agentInfos;
    public static inBattleManager Instance;
    public List<GameObject> Line1 = new List<GameObject>() { null,null,null,null,null,null};//xxx xxx xxx aaa aaa aaa;6个对象,其在列表中位置即为场景中位置
    public List<GameObject> Line2 = new List<GameObject>() { null, null, null, null, null, null };//xxx xxx xxx aaa aaa aaa;6个对象,其在列表中位置即为场景中位置
    public List<EnemyUnit> 敌方出怪池 = new List<EnemyUnit>() ;
    public List<GameObject> 敌方可能生成的 = new List<GameObject>();
    public List<GameObject> 玩家可放置砖块;
    public List<GameObject> 敌人可放置砖块;
    public bool 自动生成;
    public int 自动生成数量 = 4;
    public int GhostNum;
    public Text GhostNumShow;
    public bool isActing;
    public Button endRoundButton;
    public 玩家可放入槽位 pp当前选择砖块;
    bool isRealStart;
    public bool 可以交换;
    public GameObject winPanel;
    public GameObject losePanel;
    public Text healthText;
    public bool isEnd;
    private void Awake()
    {
        GameObjectPool.Instance.ClearQueue();
        Instance = this; 
        CameraShake.Instance.ToString();
    }

    public void Upload交换信息(玩家可放入槽位 p)
    {
        if (!可以交换)
        {
            return;
        }
        AudioManager.install.PlaySoundEffectsByName("UI_GeneralClick");
        if(pp当前选择砖块 != null && p != pp当前选择砖块)
        {
            var middle = pp当前选择砖块.此地物体;
            if (pp当前选择砖块.Line == 1)
            {
                Line1[pp当前选择砖块.Index] = p.此地物体;
            }else
            {
                Line2[pp当前选择砖块.Index] = p.此地物体;
            }
            pp当前选择砖块.此地物体 = p.此地物体;
            
            if (p.Line == 1)
            {
                Line1[p.Index] = middle;
            }
            else
            {
                Line2[p.Index] = middle;
            }
            p.此地物体 = middle;
            if(p.此地物体 != null)
            {
                p.此地物体.transform.SetParent(p.transform);
                p.此地物体.transform.localScale = new Vector2(1, 1);
                p.此地物体.transform.localPosition = Vector2.zero;
                p.此地物体.GetComponent<BasicAliveThing>().Reset();
            }
           
            if(pp当前选择砖块.此地物体!= null)
            {
                pp当前选择砖块.此地物体.transform.SetParent(pp当前选择砖块.transform);
                pp当前选择砖块.此地物体.transform.localScale = new Vector2(1, 1);
                pp当前选择砖块.此地物体.transform.localPosition = Vector2.zero;
                pp当前选择砖块.此地物体.GetComponent<BasicAliveThing>().Reset();
               
            }
            pp当前选择砖块.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 87f / 255f);
            pp当前选择砖块 = null;
        }
        else
        {
            pp当前选择砖块= p;
        }
    }

    private void Start()
    {
        agentInfos = GameDataMgr.Instance.nowAgentList;
        if (自动生成)
        {
            随机自动生成敌人();
        }
        GhostNum = GameDataMgr.Instance.playerData.startEnergy;
    }
    private void FixedUpdate()
    {
        GhostNumShow.text = GhostNum.ToString();
        endRoundButton.interactable = !isActing;
        if(pp当前选择砖块 != null)
        {
            pp当前选择砖块.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            if (!可以交换)
            {
                pp当前选择砖块 = null;
            }
        }
        if(getAllObj(TeamType.Team2).Count == 0 && isRealStart)
        {
            winPanel.SetActive(true);
        }
        bool 没有能打出的牌 = true;
        foreach(var a in GameDataMgr.Instance.nowAgentList)
        {
            
            if(a.energy <= GhostNum)
            {
                没有能打出的牌 = false;
            }
        }
        if(getAllObj(TeamType.Team1).Count == 0 && isRealStart && 没有能打出的牌)
        {
            losePanel.SetActive(true);
        }
        healthText.text = "玩家hp(剩余复活次数)：" +GameDataMgr.Instance.playerData.nowHealth.ToString();
    }

    public List<GameObject> getAllObj()
    {
        List<GameObject> allList = new List<GameObject>();
        allList.AddRange(Line1);
        allList.AddRange(Line2);
        return allList;
    }

    public List<GameObject> getAllObj(TeamType teamType)
    {
        List<GameObject> allList = new List<GameObject>();
        foreach(var e in Line1)
        {
            if(e != null)
            {
                if(e.GetComponent<BasicAliveThing>().teamType == teamType)
                {
                    allList.Add(e);
                }
            }
        }
        foreach (var e in Line2)
        {
            if (e != null)
            {
                if (e.GetComponent<BasicAliveThing>().teamType == teamType)
                {
                    allList.Add(e);
                }
            }
        }
        return allList;
    }


    public List<GameObject> getObjLineAllObj(GameObject obj)
    {
        if (Line1.Contains(obj))
        {
            return Line1;
        }
        if(Line2.Contains(obj))
        {
            return Line2;
        }
        return null;
    }

    public void 随机自动生成敌人()
    {
        for(int i = 0;i < 自动生成数量; i++)
        {
            敌方可能生成的 = 敌方出怪池[GameDataMgr.Instance.Level % 5].Enemies;
            var e = 敌方可能生成的[Random.Range(0, 敌方可能生成的.Count)];
            var ss = GameObjectPool.Instance.CreateGameObject(e);
            var aa可选的 = new List<GameObject>();
            foreach(var p in 敌人可放置砖块)
            {
                if(p.transform.childCount == 0)
                {
                    aa可选的.Add(p);
                }
            }
            var sjjd = aa可选的[Random.Range(0, aa可选的.Count)] ;
            ss.transform.SetParent(sjjd.transform);
            if(sjjd.GetComponent<玩家可放入槽位>().Line == 1)
            {
                Line1[sjjd.GetComponent<玩家可放入槽位>().Index+2] = ss;
            }
            if (sjjd.GetComponent<玩家可放入槽位>().Line == 2)
            {
                Line2[sjjd.GetComponent<玩家可放入槽位>().Index+2] = ss;
            }
            ss.transform.localScale = new Vector2(1, 1);
            ss.transform.localPosition = Vector2.zero;
            ss.GetComponent<BasicAliveThing>().teamType = TeamType.Team2;
            ss.GetComponent<BasicAliveThing>().Reset();
        }
    }

    public void EndRound()
    {
        isActing = true;
        List<GameObject> nowEnemies = new List<GameObject>();
        nowEnemies.AddRange(getAllObj(TeamType.Team1));
        foreach(var t in getAllObj(TeamType.Team2))
        {
            if (!nowEnemies.Contains(t))
            {
                nowEnemies.Add(t);
            }
        }
        EnemyManager.Instance.enemies = nowEnemies;
        EnemyManager.Instance.EnemiesStartAct();
        isRealStart = true;
    }

}
