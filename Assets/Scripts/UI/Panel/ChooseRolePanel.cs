using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Linq;
using Spine;

public class ChooseRolePanel : BasePanel
{
    public Transform 主框架;
    public GameObject 槽位;
    public List<GameObject> 可用槽位 = new List<GameObject>();
    private Dictionary<string, Button> agentButtons = new Dictionary<string, Button>();
    public List<GameObject> ghostInTX = new List<GameObject>();
    public string toRoom = "InBattle";

    private void OnEnable()
    {
        foreach(var e in 可用槽位)
        {
            e.SetActive(false);
        }
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
    }

    public GameObject getGostInTX()
    {
        foreach(var e in ghostInTX)
        {
            if (!e.active)
            {
                return e;
            }
        }
        return null;
    }


    public void showIntroduce(Button control)
    {
        var agentInfo = control.name;
        var introduceImage = GetControl<Image>("介绍1");
        if (introduceImage != null)
        {
            introduceImage.gameObject.SetActive(true);
        }

        var roleBox = control.GetComponent<RoleChoose_Box>();
        if (roleBox == null || roleBox.AimObj == null)
        {
            Debug.LogError($"RoleChoose_Box or AimObj missing on {control.name}");
            return;
        }

        var healthMgr = roleBox.AimObj.GetComponent<HealthMgr>();
        var basicAlive = roleBox.AimObj.GetComponent<BasicAliveThing>();

        if (healthMgr == null || basicAlive == null)
        {
            Debug.LogError($"Required components missing on {roleBox.AimObj.name}");
            return;
        }

        var introduceText = GetControl<Text>("介绍");
        if (introduceText != null)
        {
            string hp = healthMgr.Health.ToString();
            string damage = basicAlive.Damage.ToString();
            string cost = basicAlive.cost.ToString();
            introduceText.text = $"{agentInfo}[血量:{hp}，伤害{damage}，费用{cost}]\n{GameDataMgr.Instance.AgentDic[agentInfo].tips}";

            if (introduceImage != null)
            {
                introduceImage.transform.position = control.transform.position - Vector3.up * 60;
            }
        }
    }
    void Start()
    {
        Button control;
        foreach (var agentInfo in GameDataMgr.Instance.AgentDic.Keys)
        {
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
    }
    protected override void ClickBtn(string btnName)
    {
        if (GameDataMgr.Instance.AgentDic.ContainsKey(btnName))
        {
            AgentInfo agent = GameDataMgr.Instance.AgentDic[btnName];

            if (!GameDataMgr.Instance.nowAgentList.Contains(agent))
            {
                if (GameDataMgr.Instance.nowAgentList.Count < GameDataMgr.Instance.playerData.num)
                {


                    AudioManager.install.PlaySoundEffectsByName("UI_PickUp");
                    // 添加后立即隐藏按钮
                    if (agentButtons.TryGetValue(btnName, out Button button))
                    {
                        button.gameObject.SetActive(false);
                        agent.prefabName = button.GetComponent<RoleChoose_Box>().AimObj.name;
                        var g = getGostInTX();
                        g.transform.position = button.transform.position;
                        g.SetActive(true);
                            var introduceImage = GetControl<Image>("介绍1");
                        if (introduceImage != null)
                        {
                            introduceImage.gameObject.SetActive(false);
                        }
                    }
                    GameDataMgr.Instance.nowAgentList.Add(agent);
                }
            }
            else
            {
                GameDataMgr.Instance.nowAgentList.Remove(agent);

                // 移除后重新显示按钮
                if (agentButtons.TryGetValue(btnName, out Button button))
                {
                    button.gameObject.SetActive(true);
                }
            }
        }
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
                    UImanager.Instance.删除面板<ChooseRolePanel>();
                }
                break;
        }
    }

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

    protected override void Update()
    {
        base.Update();
        for (int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if (可用槽位.Count > i )
            {
                if (可用槽位[i] != null )
                { 
                    if(GameDataMgr.Instance.nowAgentList.Count > i)
                    {
                        print(GameDataMgr.Instance.nowAgentList[i].name);
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

        }
        UpdateButtonStates();
    }

    public void returnObj(GameObject 槽位子对象)
    {
        if (槽位子对象.name != "空")
        {
            var agentName = 槽位子对象.name;
            AgentInfo agent = GameDataMgr.Instance.AgentDic[agentName];
            GameDataMgr.Instance.nowAgentList.Remove(agent);

            // 移除后重新显示按钮
            if (agentButtons.TryGetValue(agentName, out Button button))
            {
                button.gameObject.SetActive(true);
            }
        }
    }

}
