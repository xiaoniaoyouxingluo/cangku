using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataMgr : BaseManager<GameDataMgr>
{
    public Dictionary<string, AgentInfo> AgentDic = new Dictionary<string, AgentInfo>();
    public List<AgentInfo> nowAgentList = new List<AgentInfo>();
    public List<AgentInfo> historyAgentList = new List<AgentInfo>();
    public PlayerData playerData = new PlayerData();
    public int Level = 1;
    private GameDataMgr() 
    {
        //读取某个json来着；
        string str = File.ReadAllText(Application.streamingAssetsPath + "/AgentDic.json");
        AgentDic = JsonMapper.ToObject<Dictionary<string, AgentInfo>>(str);
        if (File.Exists(Application.persistentDataPath + "/playerData.json"))
        {
            str = File.ReadAllText(Application.persistentDataPath + "/playerData.json");
            playerData = JsonMapper.ToObject<PlayerData>(str);
        }
    }
}
