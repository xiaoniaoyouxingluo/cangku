using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
/// <summary>
/// �������
/// </summary>
public class SettingPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        GetControl<Slider>("sliderMusic").value = GameDataMgr.Instance.musicData.musicValue;
        GetControl<Slider>("sliderSFX").value = GameDataMgr.Instance.musicData.soundValue;
        GetControl<Toggle>("toggleFullscreen").isOn = Screen.fullScreen;
    }
    protected override void ClickBtn(string btnName)
    {
        switch (btnName) 
        {
            case "Button_Quit":
                UImanager.Instance.ɾ�����<SettingPanel>();
                break;
            case "Button_Back":
                UImanager.Instance.ɾ�����<SettingPanel>();
                SceneManager.LoadScene("Menu");
                break;
        }
    }
    protected override void SliderValueChange(string sliderName, float value)
    {
        switch (sliderName) 
        {
            case "sliderMusic":
                GameDataMgr.Instance.musicData.musicValue = value;  //���ڱ������ִ�С
                BKMusic.Instacne.ChangeValue(value);
                break;
            case "sliderSFX":
                GameDataMgr.Instance.musicData.soundValue = value;  //������Ч��С
                AudioManager.Instance.ChangeValue(value);
                break;
        }
    }
    // ����ֵ�仯
    protected override void ToggleValueChange(string toggleName, bool value)
    {
        if (toggleName == "toggleFullscreen")
            Screen.fullScreen = value;
    }
    public override void HideMe(UnityAction callBack)
    {
        GameDataMgr.Instance.SaveMusicData();
        AudioManager.Instance.settingIsOn = false;
        base.HideMe(callBack);
    }
}