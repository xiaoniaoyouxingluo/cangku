using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
/// <summary>
/// 设置面板
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
                UImanager.Instance.删除面板<SettingPanel>();
                break;
            case "Button_Back":
                UImanager.Instance.删除面板<SettingPanel>();
                SceneManager.LoadScene("Menu");
                break;
        }
    }
    protected override void SliderValueChange(string sliderName, float value)
    {
        switch (sliderName) 
        {
            case "sliderMusic":
                GameDataMgr.Instance.musicData.musicValue = value;  //调节背景音乐大小
                BKMusic.Instacne.ChangeValue(value);
                break;
            case "sliderSFX":
                GameDataMgr.Instance.musicData.soundValue = value;  //调节音效大小
                AudioManager.Instance.ChangeValue(value);
                break;
        }
    }
    // 开关值变化
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