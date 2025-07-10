using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataMgr : BaseManager<GameDataMgr>
{
    //游戏全家具字典
    public Dictionary<string, AgentInfo> AgentDic = new Dictionary<string, AgentInfo>();
    //当前游戏玩家所拥有的家具
    public List<AgentInfo> nowAgentList = new List<AgentInfo>();
    public List<AgentInfo> historyAgentList = new List<AgentInfo>();
    //玩家数据
    public PlayerData playerData = new PlayerData();
    //音乐数据
    public MusicData musicData = new MusicData();
    //等级
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
        if (File.Exists(Application.persistentDataPath + "/musicData.json"))
        {
            str = File.ReadAllText(Application.persistentDataPath + "/musicData.json");
            musicData = JsonMapper.ToObject<MusicData>(str);
        }
    }
    /// <summary>
    /// 存储音乐数据
    /// </summary>
    public void SaveMusicData()
    {
        string srt = JsonMapper.ToJson(musicData);
        File.WriteAllText(Application.persistentDataPath + "/musicData.json", srt);
    }
    /// <summary>
    /// 存储玩家数据
    /// </summary>
    public void SavePlayerData()
    {

    }
}
