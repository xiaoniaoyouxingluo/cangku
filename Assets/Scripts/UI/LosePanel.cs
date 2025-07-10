using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
    public GameObject btn复活;
    string aimScene;

    private void OnEnable()
    {
        AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_GameLose");
    }
    private void Update()
    {
        btn复活.SetActive(GameDataMgr.Instance.playerData.nowHealth > 0);
    }
    public void 复活()
    {
        GameDataMgr.Instance.playerData.nowHealth -= 1;
        GetComponent<Animator>().Play("LosePanel_Out");
        aimScene = "ChooseRole";
    }

    public void 重开()
    {
        aimScene = "Menu";
        GetComponent<Animator>().Play("LosePanel_Out");
        GameDataMgr.Instance.nowAgentList.Clear();
        GameDataMgr.Instance.historyAgentList.Clear();
    }

    public void Out()
    {
        SceneManager.LoadScene(aimScene);
    }
}
