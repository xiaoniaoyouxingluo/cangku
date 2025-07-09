using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartPanel : BasePanel
{
    public AudioClip sl;
    protected override void Awake()
    {
        base.Awake();
        AudioMgr.Instance.ToString();
        UImanager.AddCustomEventListener(GetControl<Button>("Button_Start"),
            EventTriggerType.PointerEnter, OnButtonHover);

        UImanager.AddCustomEventListener(GetControl<Button>("Button_Setting"),
            EventTriggerType.PointerEnter, OnButtonHover);

        UImanager.AddCustomEventListener(GetControl<Button>("Button_Quit"),
            EventTriggerType.PointerEnter, OnButtonHover);
    }

    private void OnButtonHover(BaseEventData data)
    {
        //��˵��ʱ��д��Ч
       // AudioManager.install.PlaySoundEffects(sl);
    }

    // ��ť���
    protected override void ClickBtn(string btnName)
    {
        //PlaySound("ButtonClick");

        switch (btnName)
        {
            case "Button_Start":
                StartGame();
                break;

            case "Button_Setting":
                ShowSettings();
                break;

            case "Button_Quit":
                QuitGame();
                break;
        }
    }

    private void StartGame()
    {
        UImanager.Instance.�������<LoadingPanel>();

        //��һ�¶������1s
        Invoke("LoadScene", 1);
    }

    void LoadScene()
    {
        SceneMgr.Instance.LoadSceneAsyn("ChooseRole", () => {
            GameDataMgr.Instance.playerData.nowHealth = 3;
            GameDataMgr.Instance.playerData.num = 4;
            GameDataMgr.Instance.playerData.startEnergy = 10;
            GameDataMgr.Instance.Level = 1;
            UImanager.Instance.ɾ�����<StartPanel>();
            UImanager.Instance.ɾ�����<LoadingPanel>();
        });
    }


    private void ShowSettings()
    {
        UImanager.Instance.�������<SettingPanel>();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}