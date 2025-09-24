using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Transform 主框架;
    public GameObject 槽位;
    public List<GameObject> 可用槽位 = new List<GameObject>();
    public Image 介绍1;
    public Text 介绍;
    GameObject 选中物品;
    卡槽数据 data选中;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance.AddEventListener<TeamType>("回合开始时", 回合开始时);
        EventCenter.Instance.AddEventListener<TeamType>("回合结束时", 回合结束时);
        AgentInfo ai;
        GameObject nc;
        for (int i = 0; i < GameDataMgr.Instance.agentList.Count; i++)//把可用槽位创建一定数量
        {
            nc = Instantiate(槽位);
            nc.transform.SetParent(主框架);
            可用槽位.Add(nc);
            nc.SetActive(true);
            ai = BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[GameDataMgr.Instance.agentList[i]];
            nc.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/" + ai.name);
            if (nc.GetComponent<卡槽数据>() == null)
                nc.AddComponent<卡槽数据>();

            nc.GetComponent<卡槽数据>().Name = ai.name;
            nc.GetComponent<卡槽数据>().AgentInfo = ai;
            nc.GetComponent<卡槽数据>().物体 = Resources.Load<GameObject>("Prefabs/Enemies/" + ai.name);
            nc.GetComponent<卡槽数据>().Cost = ai.cost;
            nc.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = ai.cost.ToString();
        }
    }
    protected override void Update()
    {
        base.Update();
        if (选中物品 != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, 1 << 8);//射线检测获取场上的部署位
            if (hit.collider != null && hit.collider.gameObject.GetComponent<玩家可放入槽位>()?.此地物体 == null)
            {
                选中物品.transform.position = hit.collider.transform.position;
                if (Input.GetMouseButtonDown(0) && !GameLevelMgr.Instance.isAtking)//鼠标左键部署
                {
                    hit.collider.gameObject.GetComponent<玩家可放入槽位>().此地物体 = 选中物品;
                    GameSceneManager.Instance.Line1[hit.collider.gameObject.GetComponent<玩家可放入槽位>().pos.x, hit.collider.gameObject.GetComponent<玩家可放入槽位>().pos.y] = 选中物品;
                    选中物品.transform.SetParent(hit.collider.gameObject.transform);
                    选中物品.transform.localScale = new Vector2(1, 1);
                    选中物品.transform.localPosition = Vector2.zero;
                    选中物品.GetComponent<AgentObj>().teamType = TeamType.Team1;//设置成我方阵营
                    //选中物品.GetComponent<AgentObj>().InitObj(data选中.AgentInfo.id);//初始化预设体数据
                    for (int i = 0; i < 可用槽位.Count; i++)
                        if (data选中.Name == 可用槽位[i].GetComponent<卡槽数据>().Name)//失活部署栏中的选择
                            可用槽位[i].SetActive(false);
                    GameLevelMgr.Instance.GhostNum -= data选中.Cost;//扣除剩余能量
                    选中物品 = null;
                    data选中 = null;
                    MusicMgr.Instance.PlaySound("Sounds/UI_MoveChequers_Set");//播放音效
                }
            }
            else
            {
                选中物品.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(选中物品);
                选中物品 = null;
                data选中 = null;
            }
        }
        for(int i = 0;i <可用槽位.Count;i++)
        {
            if(可用槽位[i].activeSelf)
            {
                可用槽位[i].transform.GetChild(2).gameObject.SetActive(可用槽位[i].GetComponent<卡槽数据>().再部署时间 > 0);
                可用槽位[i].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = 可用槽位[i].GetComponent<卡槽数据>().再部署时间.ToString();
            }
        }
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<TeamType>("回合开始时", 回合开始时);
        EventCenter.Instance.RemoveEventListener<TeamType>("回合结束时", 回合结束时);
    }
    private void 回合开始时(TeamType teamType)
    {
        GetControl<Button>("btnQuit").interactable = false;
    }
    private void 回合结束时(TeamType teamType)
    {
        for(int i = 0;i<可用槽位.Count;i++)
        {
            if (可用槽位[i].activeSelf && 可用槽位[i].GetComponent<卡槽数据>().再部署时间 > 0)
                可用槽位[i].GetComponent<卡槽数据>().再部署时间 -= 0.5f;
        }
        GetControl<Button>("btnQuit").interactable = true;
    }
    /// <summary>
    /// 刷新部署栏
    /// </summary>
    public void Update部署栏()
    {
        for (int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if (GameDataMgr.Instance.nowAgentList.Count > i)
            {
                可用槽位[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/" + GameDataMgr.Instance.nowAgentList[i].name);

                if (可用槽位[i].GetComponent<卡槽数据>() == null)
                    可用槽位[i].AddComponent<卡槽数据>();

                可用槽位[i].GetComponent<卡槽数据>().Name = GameDataMgr.Instance.nowAgentList[i].name;
                可用槽位[i].GetComponent<卡槽数据>().AgentInfo = GameDataMgr.Instance.nowAgentList[i];
                可用槽位[i].GetComponent<卡槽数据>().物体 = Resources.Load<GameObject>("Prefabs/Enemies/" + GameDataMgr.Instance.nowAgentList[i].name);
                可用槽位[i].GetComponent<卡槽数据>().Cost = 可用槽位[i].GetComponent<卡槽数据>().物体.GetComponent<BasicAliveThing>().cost;
                可用槽位[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = 可用槽位[i].GetComponent<卡槽数据>().物体.GetComponent<BasicAliveThing>().cost.ToString();
            }
            else
            {
                可用槽位[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/empty");
                可用槽位[i].transform.GetChild(1).gameObject.SetActive(false);
                可用槽位[i].GetComponent<卡槽数据>().Cost = -1;
                可用槽位[i].GetComponent<卡槽数据>().Name = "空";
                可用槽位[i].GetComponent<卡槽数据>().AgentInfo = null;
                可用槽位[i].GetComponent<卡槽数据>().物体 = null;
            }
        }
    }
    public void ClickBtn(卡槽数据 data)
    {
        if (data.Cost <= GameLevelMgr.Instance.GhostNum && !GameLevelMgr.Instance.isAtking && data.再部署时间 == 0)
        {
            MusicMgr.Instance.PlaySound("Sounds/UI_MoveChequers_Pick");
            选中物品 = Instantiate(data.物体);
            data选中 = data;
            选中物品.GetComponent<AgentObj>().InitObj(data选中.AgentInfo.id);//初始化预设体数据
        }
    }
    /// <summary>
    /// 鼠标进入控件显示介绍
    /// </summary>
    /// <param name="agentObj">玩家对象</param>
    public void showIntroduce(AgentObj agentObj)
    {
        UImanager.Instance.GetPanel<GamePanel>().介绍1.gameObject.SetActive(true);//显示介绍面板
        UImanager.Instance.GetPanel<GamePanel>().介绍.text = agentObj.Property.tipstxt;
        Vector3 v = Camera.main.WorldToScreenPoint(agentObj.transform.position) - Vector3.up * 100;//设置介绍面板位置
                                                                                                  //介绍面板在屏幕上的最小位置
        v.z = 0;
        Vector2 screenMinVer = new Vector2(0 + (UImanager.Instance.GetPanel<GamePanel>().介绍1.transform as RectTransform).sizeDelta.x / 2, 0 + (UImanager.Instance.GetPanel<GamePanel>().介绍1.transform as RectTransform).sizeDelta.y / 2);
        //介绍面板在屏幕上的最大位置
        Vector2 screenMaxVer = new Vector2(Screen.width - (UImanager.Instance.GetPanel<GamePanel>().介绍1.transform as RectTransform).sizeDelta.x / 2, Screen.height - (UImanager.Instance.GetPanel<GamePanel>().介绍1.transform as RectTransform).sizeDelta.y / 2);
        UImanager.Instance.GetPanel<GamePanel>().介绍1.transform.position = new Vector3(Mathf.Clamp(v.x, screenMinVer.x, screenMaxVer.x), Mathf.Clamp(v.y, screenMinVer.y, screenMaxVer.y));//防止介绍面板超出屏幕范围

    }
    /// <summary>
    /// 鼠标退出控件隐藏介绍
    /// </summary>
    public void HideIntroduce()
    {
        UImanager.Instance.GetPanel<GamePanel>().介绍1.gameObject.SetActive(false);//隐藏介绍面板
    }
    protected override void ClickBtn(string btnName)
    {
        switch(btnName) 
        {
            case "btnQuit":
                GameLevelMgr.Instance.PlayAtk();
                break;
        }
    }
}
