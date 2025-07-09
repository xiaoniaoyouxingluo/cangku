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
        //我说到时候写音效
       // AudioManager.install.PlaySoundEffects(sl);
    }

    // 按钮点击
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
        UImanager.Instance.创建面板<LoadingPanel>();

        //等一下动画差不多1s
        Invoke("LoadScene", 1);
    }

    void LoadScene()
    {
        SceneMgr.Instance.LoadSceneAsyn("ChooseRole", () => {
            GameDataMgr.Instance.playerData.nowHealth = 3;
            GameDataMgr.Instance.playerData.num = 4;
            GameDataMgr.Instance.playerData.startEnergy = 10;
            GameDataMgr.Instance.Level = 1;
            UImanager.Instance.删除面板<StartPanel>();
            UImanager.Instance.删除面板<LoadingPanel>();
        });
    }


    private void ShowSettings()
    {
        UImanager.Instance.创建面板<SettingPanel>();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}