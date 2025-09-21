using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataMgr : BaseManager<GameDataMgr>
{
    /// <summary>
    /// 当前游戏玩家所拥有的家具
    /// </summary>
    public List<AgentInfo> nowAgentList = new List<AgentInfo>();
    /// <summary>
    /// 当前游戏玩家所拥有的家具id
    /// </summary>
    public List<int> agentList = new List<int>();
    /// <summary>
    /// 玩家数据
    /// </summary>
    public PlayerData playerData = new PlayerData();
    /// <summary>
    /// 音乐数据
    /// </summary>
    public MusicData musicData = new MusicData();
    /// <summary>
    /// 第几关
    /// </summary>
    public int Level = 1;
    private GameDataMgr() 
    {
        //读取某个json来着；
        string str;
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
