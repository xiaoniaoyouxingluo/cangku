using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SettingPanel : BasePanel
{
    public Slider sliderMusic;
    public Slider sliderSFX;
    public Toggle fullScreen;

    protected override void Awake()
    {
        base.Awake();

        UImanager.AddCustomEventListener(GetControl<Button>("Button_Close"),
            EventTriggerType.PointerClick, _ => ClosePanel());
        UImanager.AddCustomEventListener(GetControl<Button>("Button_Back"),
          EventTriggerType.PointerClick, _ => SceneManager.LoadScene("Menu"));
    }

    public void OnEnable()
    {
        AudioManager.install.settingIsOn = true;
        sliderMusic.value = PlayerPrefs.GetFloat("Music_Volume", 1);
        sliderSFX.value = PlayerPrefs.GetFloat("SFX_Volume", 1);
        fullScreen.isOn = Screen.fullScreen;
       
    }

    private void FixedUpdate()
    {
        PlayerPrefs.SetFloat("Music_Volume", sliderMusic.value);
        PlayerPrefs.SetFloat("SFX_Volume", sliderSFX.value);
    }




    // 开关值变化
    protected override void ToggleValueChange(string toggleName, bool value)
    {
        if (toggleName == "toggleFullscreen")
            Screen.fullScreen = value;
    }

    private void ClosePanel()
    {
        AudioManager.install.settingIsOn = false;
        UImanager.Instance.删除面板<SettingPanel>();
    }
    public override void HideMe(UnityAction callBack)
    {
        AudioManager.install.settingIsOn = false;
        base.HideMe(callBack);
    }
}