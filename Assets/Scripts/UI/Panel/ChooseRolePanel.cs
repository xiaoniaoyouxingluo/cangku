using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Linq;
using Spine;
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
        string agentInfo = control.name;//获取控件名字
        Image introduceImage = GetControl<Image>("介绍1");
        if (introduceImage != null)
        {
            introduceImage.gameObject.SetActive(true);//显示介绍面板
        }

        RoleChoose_Box roleBox = control.GetComponent<RoleChoose_Box>();//获取灵居预设体关联组件
        if (roleBox == null || roleBox.AimObj == null)
        {
            Debug.LogError($"RoleChoose_Box or AimObj missing on {control.name}");
            return;
        }

        HealthMgr healthMgr = roleBox.AimObj.GetComponent<HealthMgr>();//获取血量和护甲
        BasicAliveThing basicAlive = roleBox.AimObj.GetComponent<BasicAliveThing>();//获取灵居基类

        if (healthMgr == null || basicAlive == null)
        {
            Debug.LogError($"Required components missing on {roleBox.AimObj.name}");
            return;
        }

        Text introduceText = GetControl<Text>("介绍");//获取介绍文本组件
        if (introduceText != null)
        {
            string hp = healthMgr.Health.ToString();
            string damage = basicAlive.Damage.ToString();
            string cost = basicAlive.cost.ToString();
            //introduceText.text = $"{agentInfo}[血量:{hp}，伤害{damage}，费用{cost}]\n{GameDataMgr.Instance.AgentDic[agentInfo].tipstxt}";//拼接介绍字符串

            if (introduceImage != null)
            {
                introduceImage.transform.position = control.transform.position - Vector3.up * 60;//设置介绍面板位置
            }
        }
    }
    protected override void ClickBtn(string btnName)
    {
        //if (GameDataMgr.Instance.AgentDic.ContainsKey(btnName))
        //{
        //    AgentInfo agent = GameDataMgr.Instance.AgentDic[btnName];
        //    if (!GameDataMgr.Instance.nowAgentList.Contains(agent))
        //    {
        //        if (GameDataMgr.Instance.nowAgentList.Count < GameDataMgr.Instance.playerData.num)
        //        {
        //            AudioManager.Instance.PlaySoundEffectsByName("UI_PickUp");//播放音效
        //            // 添加后立即隐藏按钮
        //            Button button = GetControl<Button>(btnName);
        //            button.gameObject.SetActive(false);
        //            agent.prefabName = btnName;//设置预设体名字
        //            Instantiate<GameObject>(ghostInTX, button.transform.position, Quaternion.identity, this.transform);//创建鬼魂特效
        //            Image introduceImage = GetControl<Image>("介绍1");
        //            if (introduceImage != null)
        //                introduceImage.gameObject.SetActive(false);
        //            GameDataMgr.Instance.nowAgentList.Add(agent);
        //        }
        //    }
        //    else
        //    {
        //        GameDataMgr.Instance.nowAgentList.Remove(agent);
        //        // 移除后重新显示按钮
        //        GetControl<Button>(btnName).gameObject.SetActive(true);
        //    }
        //    Update可用槽位();
        //}
        switch (btnName)
        {
            case "切换":
                if(GameDataMgr.Instance.nowAgentList.Count <= GameDataMgr.Instance.playerData.num)
                {
                    if(GameDataMgr.Instance.Level % 5 == 0)
                    {
                        toRoom = "InBattle_Boss";
                    }
                    else
                    {
                        toRoom = "InBattle";
                    }
                    SceneManager.LoadScene(toRoom);
                    GameDataMgr.Instance.historyAgentList.AddRange(GameDataMgr.Instance.nowAgentList);
                    UImanager.Instance.创建面板<GamePanel>();
                    UImanager.Instance.删除面板<ChooseRolePanel>();
                }
                break;
        }
    }
    /// <summary>
    /// 更新选择面板上的物体按钮显隐
    /// </summary>
    private void UpdateButtonStates()
    {
        foreach (var kv in agentButtons)
        {
            string agentName = kv.Key;
            Button button = kv.Value;

            // 如果该角色已在选择列表中，隐藏按钮
            bool isSelected = GameDataMgr.Instance.nowAgentList.Any(a => a.name == agentName) || GameDataMgr.Instance.historyAgentList.Any(a => a.name == agentName);
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
            if (GameDataMgr.Instance.nowAgentList.Count > i)
            {
                可用槽位[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/" + GameDataMgr.Instance.nowAgentList[i].name);
                可用槽位[i].transform.GetChild(0).name = GameDataMgr.Instance.nowAgentList[i].name;
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
        //if (槽位子对象.name != "空")
        //{
        //    string agentName = 槽位子对象.name;
        //    AgentInfo agent = GameDataMgr.Instance.AgentDic[agentName];
        //    GameDataMgr.Instance.nowAgentList.Remove(agent);
        //    // 移除后重新显示按钮
        //    GetControl<Button>(agentName).gameObject.SetActive(true);
        //    Update可用槽位();
        //}        
    }

}
