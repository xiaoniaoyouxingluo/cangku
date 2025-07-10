using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeeGameDataMgr : MonoBehaviour
{
    public List<AgentInfo> nowAgentInfos = new List<AgentInfo>();
    public List<AgentInfo> historyAgentInfos = new List<AgentInfo>();
    public PlayerData pd;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        nowAgentInfos = GameDataMgr.Instance.nowAgentList;
        historyAgentInfos = GameDataMgr.Instance.historyAgentList;    
    }
    [ContextMenu("Ë¢ÐÂ")]
    public void ss()
    {
        pd = GameDataMgr.Instance.playerData;
    }
    [ContextMenu("Í¬²½")]
    public void so()
    {
        GameDataMgr.Instance.playerData= pd;
    }
}
