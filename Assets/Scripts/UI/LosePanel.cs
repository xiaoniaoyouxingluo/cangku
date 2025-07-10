using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
    public GameObject btn����;
    string aimScene;

    private void OnEnable()
    {
        AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_GameLose");
    }
    private void Update()
    {
        btn����.SetActive(GameDataMgr.Instance.playerData.nowHealth > 0);
    }
    public void ����()
    {
        GameDataMgr.Instance.playerData.nowHealth -= 1;
        GetComponent<Animator>().Play("LosePanel_Out");
        aimScene = "ChooseRole";
    }

    public void �ؿ�()
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
