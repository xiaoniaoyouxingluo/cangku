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
    public Transform �����;
    public GameObject ��λ;
    public List<GameObject> ���ò�λ = new List<GameObject>();
    private Dictionary<string, Button> agentButtons = new Dictionary<string, Button>();
    public List<GameObject> ghostInTX = new List<GameObject>();
    public string toRoom = "InBattle";

    private void OnEnable()
    {
        foreach(var e in ���ò�λ)
        {
            e.SetActive(false);
        }
        for(int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if(���ò�λ.Count > i)
            {
                ���ò�λ[i].SetActive(true);
            }
            else
            {
                var nc = Instantiate(��λ);
                nc.transform.SetParent(�����);
                ���ò�λ.Add(nc);
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
        var introduceImage = GetControl<Image>("����1");
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

        var introduceText = GetControl<Text>("����");
        if (introduceText != null)
        {
            string hp = healthMgr.Health.ToString();
            string damage = basicAlive.Damage.ToString();
            string cost = basicAlive.cost.ToString();
            introduceText.text = $"{agentInfo}[Ѫ��:{hp}���˺�{damage}������{cost}]\n{GameDataMgr.Instance.AgentDic[agentInfo].tips}";

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
                print("�����" + agentInfo);
                UImanager.AddCustomEventListener(control, EventTriggerType.PointerExit, (b) =>
                {
                    GetControl<Image>("����1").gameObject.SetActive(false);
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
                    // ��Ӻ��������ذ�ť
                    if (agentButtons.TryGetValue(btnName, out Button button))
                    {
                        button.gameObject.SetActive(false);
                        agent.prefabName = button.GetComponent<RoleChoose_Box>().AimObj.name;
                        var g = getGostInTX();
                        g.transform.position = button.transform.position;
                        g.SetActive(true);
                            var introduceImage = GetControl<Image>("����1");
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

                // �Ƴ���������ʾ��ť
                if (agentButtons.TryGetValue(btnName, out Button button))
                {
                    button.gameObject.SetActive(true);
                }
            }
        }
        switch (btnName)
        {
            case "�л�":
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
                    UImanager.Instance.ɾ�����<ChooseRolePanel>();
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

            // ����ý�ɫ����ѡ���б��У����ذ�ť
            bool isSelected = GameDataMgr.Instance.nowAgentList.Any(a => a.name == agentName) || GameDataMgr.Instance.historyAgentList.Any(a => a.name == agentName);
            button.gameObject.SetActive(!isSelected);
        }
    }

    protected override void Update()
    {
        base.Update();
        for (int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if (���ò�λ.Count > i )
            {
                if (���ò�λ[i] != null )
                { 
                    if(GameDataMgr.Instance.nowAgentList.Count > i)
                    {
                        print(GameDataMgr.Instance.nowAgentList[i].name);
                        ���ò�λ[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/" + GameDataMgr.Instance.nowAgentList[i].name);
                        ���ò�λ[i].transform.GetChild(0).name = GameDataMgr.Instance.nowAgentList[i].name;
                    }
                    else
                    {
                        ���ò�λ[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/empty");
                        ���ò�λ[i].transform.GetChild(0).name = "��";
                    }
                  
                   

                    
                }
            }

        }
        UpdateButtonStates();
    }

    public void returnObj(GameObject ��λ�Ӷ���)
    {
        if (��λ�Ӷ���.name != "��")
        {
            var agentName = ��λ�Ӷ���.name;
            AgentInfo agent = GameDataMgr.Instance.AgentDic[agentName];
            GameDataMgr.Instance.nowAgentList.Remove(agent);

            // �Ƴ���������ʾ��ť
            if (agentButtons.TryGetValue(agentName, out Button button))
            {
                button.gameObject.SetActive(true);
            }
        }
    }

}
