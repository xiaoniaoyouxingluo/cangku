using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Transform 主框架;
    public GameObject 槽位;
    public List<GameObject> 可用槽位 = new List<GameObject>();
    GameObject 选中物品;
    卡槽数据 data选中;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if (可用槽位.Count > i)
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
        Update部署栏();
    }
    protected override void Update()
    {
        base.Update();
        inBattleManager.Instance.可以交换 = 选中物品 == null;
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
}
