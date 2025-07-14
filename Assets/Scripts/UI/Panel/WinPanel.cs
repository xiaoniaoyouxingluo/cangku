using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    public List<GameObject> buttons;
    public Text Title;

    void OnEnable()
    {
        AudioManager.Instance.PlaySoundEffectsByName("UI_LevelUp_Intro");
    }
    public void Upgarde1_PlusEnergy()
    {
        GameDataMgr.Instance.playerData.startEnergy += 5;
        GetComponent<Animator>().Play("WinPanel_End");
        Title.text = "继续前进！";
        foreach (var b in buttons)
        {
            b.SetActive(false);
        }
        AudioManager.Instance.PlaySoundEffectsByName("UI_LevelUp_Select");
    }

    public void Upgarde2_PlusNum()
    {
        GameDataMgr.Instance.playerData.num += 1;
        GetComponent<Animator>().Play("WinPanel_End");
        Title.text = "继续前进！";
        foreach (var b in buttons)
        {
            b.SetActive(false);
        }
        AudioManager.Instance.PlaySoundEffectsByName("UI_LevelUp_Select");

    }

    public void Upgarde3_getObj()
    {
        var list = new List<AgentInfo>();
        List<string> nam = new List<string>()
        {
            "闹钟",
            "正电池",
            "负电池"
        };
        foreach(var d in GameDataMgr.Instance.AgentDic)
        {
            if (nam.Contains(d.Key))
            {
                list.Add(d.Value);
            }
        }
        GameDataMgr.Instance.nowAgentList.Add(list[Random.Range(0,list.Count)]);
        GetComponent<Animator>().Play("WinPanel_End");
        Title.text = "继续前进！";
        foreach (var b in buttons)
        {
            b.SetActive(false);
        }
        AudioManager.Instance.PlaySoundEffectsByName("UI_LevelUp_Select");

    }
    private TKey GetRandomKey<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        // 1. 检查字典是否为空
        if (dict == null || dict.Count == 0)
        {
            throw new System.ArgumentException("字典不能为空！");
        }

        // 2. 将Key转换为List
        List<TKey> keys = new List<TKey>(dict.Keys);

        // 3. 生成随机索引 (范围: 0 到 keys.Count - 1)
        int randomIndex = Random.Range(0, keys.Count);

        // 4. 返回随机Key
        return keys[randomIndex];
    }
    public void Continue()
    {
        var list = new List<AgentInfo>();
        foreach(var l in inBattleManager.Instance.getAllObj(TeamType.Team1))
        {
            var a = new AgentInfo();
            if(l != null)
            {
                if(GameDataMgr.Instance.AgentDic.TryGetValue(l.GetComponent<BasicAliveThing>().RealName,out a))
                {
                    list.Add(a);
                    if (GameDataMgr.Instance.historyAgentList.Contains(a))
                    {
                        GameDataMgr.Instance.historyAgentList.Remove(a);
                    }
                }
            }
        }
        GameDataMgr.Instance.nowAgentList.AddRange(list);
        foreach(var s in GameDataMgr.Instance.nowAgentList)
        {
            if (GameDataMgr.Instance.historyAgentList.Contains(s))
            {
                GameDataMgr.Instance.historyAgentList.Remove(s);
            }
        }
        GameDataMgr.Instance.Level++;
        SceneManager.LoadScene("ChooseRole");
    }
}
