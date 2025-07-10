using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// ��ʼ���
/// </summary>
public class StartPanel : BasePanel
{
    public AudioClip sl;
    //��¼��ǰ��ʾ�ڼ�ҳ�̳�
    private int now�̳�item = 1;
    protected override void Awake()
    {
        base.Awake();
        //��һ��������ؼ�ʱ������Ч���¼�����
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
                UImanager.Instance.�������<LoadingPanel>();
                //��һ�¶������1s
                Invoke("LoadScene", 1);
                break;
            case "Button_Setting":
                UImanager.Instance.�������<SettingPanel>();
                break;
            case "Button_Quit":
                Application.Quit();
                break;
            case "Button_introcue":
                GetControl<Image>("Panel_�̳�").gameObject.SetActive(true);
                now�̳�item = 1;
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
            UImanager.Instance.ɾ�����<StartPanel>();
            UImanager.Instance.ɾ�����<LoadingPanel>();
            UImanager.Instance.�������<ChooseRolePanel>();
        });
    }
    public void ��һҳ�̳�()
    {
        now�̳�item++;
        if(GetControl<Image>("�̳�" + now�̳�item)==null)
        {
            GetControl<Image>("�̳�" + 1).gameObject.SetActive(true);
            GetControl<Image>("�̳�" + (now�̳�item - 1)).gameObject.SetActive(false);
            GetControl<Image>("Panel_�̳�").gameObject.SetActive(false);
            return;
        }
        GetControl<Image>("�̳�" + now�̳�item).gameObject.SetActive(true);
        GetControl<Image>("�̳�" + (now�̳�item - 1)).gameObject.SetActive(false);
    }
}