using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Linq;
using Spine;
using UnityEditor.U2D.Path.GUIFramework;
/// <summary>
/// 家具选择面板
/// </summary>
public class ChooseRolePanel : BasePanel
{
    public Transform 主框架;
    public GameObject 槽位;
    public List<GameObject> 可用槽位 = new List<GameObject>();
    //灵居选择按钮字典
    private Dictionary<string, Button> agentButtons = new Dictionary<string, Button>();
    //鬼魂特效
    public GameObject ghostInTX;
    public string toRoom = "InBattle";

    void Start()
    {
        foreach(var e in 可用槽位)
            e.SetActive(false);
        for(int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if(可用槽位.Count > i)
            {
                可用槽位[i].SetActive(true);
            }
            else
            {
                var nc = Instantiate(槽位);
                nc.transform.SetParent(主框架);
                可用槽位.Add(nc);
                nc.SetActive(true);
            }
        }

        Button control;
        foreach (var agentInfo1 in BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic.Values)
        {
            string agentInfo = agentInfo1.name;
            control = GetControl<Button>(agentInfo);

            if (control != null)
            {
                agentButtons[agentInfo] = control;
                print("已添加" + agentInfo);
                UImanager.AddCustomEventListener(control, EventTriggerType.PointerExit, (b) =>
                {
                    GetControl<Image>("介绍1").gameObject.SetActive(false);
                });
            }
        }
        UpdateButtonStates();
        Update可用槽位();
    }

    /// <summary>
    /// 鼠标进入控件显示介绍
    /// </summary>
    /// <param name="control">鼠标进入的控件</param>
    public void showIntroduce(Button control)
    {
        AgentInfo agentInfo = BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[control.GetComponent<RoleChoose_Box>().id];//根据按钮上的组件中记录的id取出数据
        Image introduceImage = GetControl<Image>("介绍1");
        introduceImage.gameObject.SetActive(true);//显示介绍面板

        Text introduceText = GetControl<Text>("介绍");//获取介绍文本组件
        if (introduceText != null)
        {
            string hp = agentInfo.hp.ToString();
            string damage = agentInfo.atk.ToString();
            string cost = agentInfo.cost.ToString();
            introduceText.text = $"{agentInfo.name}[血量:{hp}，伤害{damage}，费用{cost}]\n{agentInfo.tipstxt}";//拼接介绍字符串
            Vector3 v = control.transform.position - Vector3.up * 60;//设置介绍面板位置
            //介绍面板在屏幕上的最小位置
            Vector2 screenMinVer = new Vector2(0 + (introduceImage.transform as RectTransform).sizeDelta.x / 2, 0 + (introduceImage.transform as RectTransform).sizeDelta.y / 2);
            //介绍面板在屏幕上的最大位置
            Vector2 screenMaxVer = new Vector2(Screen.width - (introduceImage.transform as RectTransform).sizeDelta.x / 2, Screen.height - (introduceImage.transform as RectTransform).sizeDelta.y / 2);
            introduceImage.transform.position = new Vector3(Mathf.Clamp(v.x, screenMinVer.x, screenMaxVer.x), Mathf.Clamp(v.y, screenMinVer.y, screenMaxVer.y));//防止介绍面板超出屏幕范围
        }
    }
    protected override void ClickBtn(string btnName)
    {
        switch (btnName)
        {
            case "切换":
                if (GameDataMgr.Instance.agentList.Count <= GameDataMgr.Instance.playerData.num)
                {
                    if (GameDataMgr.Instance.Level % 5 == 0)
                    {
                        toRoom = "InBattle_Boss";
                    }
                    else
                    {
                        toRoom = "InBattle";
                    }
                    //切换场景
                    AsyncOperation ao = SceneManager.LoadSceneAsync(toRoom);
                    //进行关卡初始化
                    ao.completed += (obj) =>
                    {
                        UImanager.Instance.删除面板<ChooseRolePanel>();
                        UImanager.Instance.创建面板<GamePanel>();
                        GameLevelMgr.Instance.InitInfo();
                    };
                }
                return;
        }
        Button button = GetControl<Button>(btnName);//按下的按钮
        int id = button.GetComponent<RoleChoose_Box>().id;//灵居id
        if (BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic.ContainsKey(id))//是配置表中有的id
        {
            AgentInfo agent = BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[id];
            if (!GameDataMgr.Instance.agentList.Contains(id))//不处于已拥有列表
            {
                if (GameDataMgr.Instance.agentList.Count < GameDataMgr.Instance.playerData.num)
                {
                    //AudioManager.Instance.PlaySoundEffectsByName("UI_PickUp");//播放音效
                    button.gameObject.SetActive(false);// 添加后立即隐藏按钮
                    agent.prefabName = btnName;//设置预设体名字
                    Instantiate<GameObject>(ghostInTX, button.transform.position, Quaternion.identity, this.transform);//创建鬼魂特效
                    Image introduceImage = GetControl<Image>("介绍1");
                    introduceImage.gameObject.SetActive(false);//隐藏介绍面板
                    GameDataMgr.Instance.agentList.Add(id);//把选择加入列表
                }
            }
            else
            {
                GameDataMgr.Instance.agentList.Remove(id);
                // 移除后重新显示按钮
                GetControl<Button>(btnName).gameObject.SetActive(true);
            }
            Update可用槽位();
        }
    }
    /// <summary>
    /// 更新选择面板上的物体按钮显隐
    /// </summary>
    private void UpdateButtonStates()
    {
        foreach (var kv in agentButtons)
        {
            Button button = kv.Value;
            // 如果该角色已在选择列表中，隐藏按钮
            bool isSelected = GameDataMgr.Instance.agentList.Contains(button.GetComponent<RoleChoose_Box>().id);
            button.gameObject.SetActive(!isSelected);
        }
    }
    /// <summary>
    /// 更新物品槽位
    /// </summary>
    private void Update可用槽位()
    {
        for (int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if (GameDataMgr.Instance.agentList.Count > i)
            {
                可用槽位[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/" + BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[GameDataMgr.Instance.agentList[i]].name);
                可用槽位[i].transform.GetChild(0).name = GameDataMgr.Instance.agentList[i].ToString();
            }
            else
            {
                可用槽位[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/empty");
                可用槽位[i].transform.GetChild(0).name = "空";
            }
        }
    }
    public void ReturnObj(GameObject 槽位子对象)
    {
        if (槽位子对象.name != "空")
        {
            int agentid = int.Parse(槽位子对象.name);
            AgentInfo agent = BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[agentid];
            GameDataMgr.Instance.agentList.Remove(agentid);
            // 移除后重新显示按钮
            GetControl<Button>(agent.name).gameObject.SetActive(true);
            Update可用槽位();
        }
    }

}
