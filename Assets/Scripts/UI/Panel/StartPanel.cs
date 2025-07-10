using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 初始面板
/// </summary>
public class StartPanel : BasePanel
{
    public AudioClip sl;
    //记录当前显示第几页教程
    private int now教程item = 1;
    protected override void Awake()
    {
        base.Awake();
        //加一个鼠标进入控件时播放音效的事件监听
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
                UImanager.Instance.创建面板<LoadingPanel>();
                //等一下动画差不多1s
                Invoke("LoadScene", 1);
                break;
            case "Button_Setting":
                UImanager.Instance.创建面板<SettingPanel>();
                break;
            case "Button_Quit":
                Application.Quit();
                break;
            case "Button_introcue":
                GetControl<Image>("Panel_教程").gameObject.SetActive(true);
                now教程item = 1;
                break;
        }
    }

    void LoadScene()
    {
        SceneMgr.Instance.LoadSceneAsyn("ChooseRole", () =>
        {
            GameDataMgr.Instance.playerData.nowHealth = 3;
            GameDataMgr.Instance.playerData.num = 4;
            GameDataMgr.Instance.playerData.startEnergy = 10;
            GameDataMgr.Instance.Level = 1;
            UImanager.Instance.删除面板<StartPanel>();
            UImanager.Instance.删除面板<LoadingPanel>();
            UImanager.Instance.创建面板<ChooseRolePanel>();
        });
    }
    public void 下一页教程()
    {
        now教程item++;
        if(GetControl<Image>("教程" + now教程item)==null)
        {
            GetControl<Image>("教程" + 1).gameObject.SetActive(true);
            GetControl<Image>("教程" + (now教程item - 1)).gameObject.SetActive(false);
            GetControl<Image>("Panel_教程").gameObject.SetActive(false);
            return;
        }
        GetControl<Image>("教程" + now教程item).gameObject.SetActive(true);
        GetControl<Image>("教程" + (now教程item - 1)).gameObject.SetActive(false);
    }
}