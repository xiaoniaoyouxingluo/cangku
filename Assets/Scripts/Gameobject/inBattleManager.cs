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
    public List<GameObject> Line1 = new List<GameObject>() { null,null,null,null,null,null};//xxx xxx xxx aaa aaa aaa;6������,�����б���λ�ü�Ϊ������λ��
    public List<GameObject> Line2 = new List<GameObject>() { null, null, null, null, null, null };//xxx xxx xxx aaa aaa aaa;6������,�����б���λ�ü�Ϊ������λ��
    public List<EnemyUnit> �з����ֳ� = new List<EnemyUnit>() ;
    public List<GameObject> �з��������ɵ� = new List<GameObject>();
    public List<GameObject> ��ҿɷ���ש��;
    public List<GameObject> ���˿ɷ���ש��;
    public bool �Զ�����;
    public int �Զ��������� = 4;
    public int GhostNum;
    public Text GhostNumShow;
    public bool isActing;
    public Button endRoundButton;
    public ��ҿɷ����λ pp��ǰѡ��ש��;
    bool isRealStart;
    public bool ���Խ���;
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

    public void Upload������Ϣ(��ҿɷ����λ p)
    {
        if (!���Խ���)
        {
            return;
        }
        AudioManager.install.PlaySoundEffectsByName("UI_GeneralClick");
        if(pp��ǰѡ��ש�� != null && p != pp��ǰѡ��ש��)
        {
            var middle = pp��ǰѡ��ש��.�˵�����;
            if (pp��ǰѡ��ש��.Line == 1)
            {
                Line1[pp��ǰѡ��ש��.Index] = p.�˵�����;
            }else
            {
                Line2[pp��ǰѡ��ש��.Index] = p.�˵�����;
            }
            pp��ǰѡ��ש��.�˵����� = p.�˵�����;
            
            if (p.Line == 1)
            {
                Line1[p.Index] = middle;
            }
            else
            {
                Line2[p.Index] = middle;
            }
            p.�˵����� = middle;
            if(p.�˵����� != null)
            {
                p.�˵�����.transform.SetParent(p.transform);
                p.�˵�����.transform.localScale = new Vector2(1, 1);
                p.�˵�����.transform.localPosition = Vector2.zero;
                p.�˵�����.GetComponent<BasicAliveThing>().Reset();
            }
           
            if(pp��ǰѡ��ש��.�˵�����!= null)
            {
                pp��ǰѡ��ש��.�˵�����.transform.SetParent(pp��ǰѡ��ש��.transform);
                pp��ǰѡ��ש��.�˵�����.transform.localScale = new Vector2(1, 1);
                pp��ǰѡ��ש��.�˵�����.transform.localPosition = Vector2.zero;
                pp��ǰѡ��ש��.�˵�����.GetComponent<BasicAliveThing>().Reset();
               
            }
            pp��ǰѡ��ש��.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 87f / 255f);
            pp��ǰѡ��ש�� = null;
        }
        else
        {
            pp��ǰѡ��ש��= p;
        }
    }

    private void Start()
    {
        agentInfos = GameDataMgr.Instance.nowAgentList;
        if (�Զ�����)
        {
            ����Զ����ɵ���();
        }
        GhostNum = GameDataMgr.Instance.playerData.startEnergy;
    }
    private void FixedUpdate()
    {
        GhostNumShow.text = GhostNum.ToString();
        endRoundButton.interactable = !isActing;
        if(pp��ǰѡ��ש�� != null)
        {
            pp��ǰѡ��ש��.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            if (!���Խ���)
            {
                pp��ǰѡ��ש�� = null;
            }
        }
        if(getAllObj(TeamType.Team2).Count == 0 && isRealStart)
        {
            winPanel.SetActive(true);
        }
        bool û���ܴ������ = true;
        foreach(var a in GameDataMgr.Instance.nowAgentList)
        {
            
            if(a.energy <= GhostNum)
            {
                û���ܴ������ = false;
            }
        }
        if(getAllObj(TeamType.Team1).Count == 0 && isRealStart && û���ܴ������)
        {
            losePanel.SetActive(true);
        }
        healthText.text = "���hp(ʣ�ิ�����)��" +GameDataMgr.Instance.playerData.nowHealth.ToString();
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

    public void ����Զ����ɵ���()
    {
        for(int i = 0;i < �Զ���������; i++)
        {
            �з��������ɵ� = �з����ֳ�[GameDataMgr.Instance.Level % 5].Enemies;
            var e = �з��������ɵ�[Random.Range(0, �з��������ɵ�.Count)];
            var ss = GameObjectPool.Instance.CreateGameObject(e);
            var aa��ѡ�� = new List<GameObject>();
            foreach(var p in ���˿ɷ���ש��)
            {
                if(p.transform.childCount == 0)
                {
                    aa��ѡ��.Add(p);
                }
            }
            var sjjd = aa��ѡ��[Random.Range(0, aa��ѡ��.Count)] ;
            ss.transform.SetParent(sjjd.transform);
            if(sjjd.GetComponent<��ҿɷ����λ>().Line == 1)
            {
                Line1[sjjd.GetComponent<��ҿɷ����λ>().Index+2] = ss;
            }
            if (sjjd.GetComponent<��ҿɷ����λ>().Line == 2)
            {
                Line2[sjjd.GetComponent<��ҿɷ����λ>().Index+2] = ss;
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
