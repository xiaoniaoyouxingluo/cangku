using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataMgr : BaseManager<GameDataMgr>
{
    //��Ϸȫ�Ҿ��ֵ�
    public Dictionary<string, AgentInfo> AgentDic = new Dictionary<string, AgentInfo>();
    //��ǰ��Ϸ�����ӵ�еļҾ�
    public List<AgentInfo> nowAgentList = new List<AgentInfo>();
    public List<AgentInfo> historyAgentList = new List<AgentInfo>();
    //�������
    public PlayerData playerData = new PlayerData();
    //��������
    public MusicData musicData = new MusicData();
    //�ȼ�
    public int Level = 1;
    private GameDataMgr() 
    {
        //��ȡĳ��json���ţ�
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
    /// �洢��������
    /// </summary>
    public void SaveMusicData()
    {
        string srt = JsonMapper.ToJson(musicData);
        File.WriteAllText(Application.persistentDataPath + "/musicData.json", srt);
    }
    /// <summary>
    /// �洢�������
    /// </summary>
    public void SavePlayerData()
    {

    }
}
