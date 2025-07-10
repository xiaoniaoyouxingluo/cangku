// 脚本示例：ButtonHoverSound.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MenuButtonSound : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
    public string _hoverSoundName = "UI_Highlight";
    bool isOn;
    public string _clickSoundName = "UI_GeneralClick";
    private AudioSource audioSource;


    public void OnPointerEnter(PointerEventData eventData)
    {
        isOn= true;
        Invoke("playHoverSound", 0.05f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isOn= false;
        var  hoverSound = Resources.Load<AudioClip>("Sounds/" + _clickSoundName);
        AudioManager.Instance.PlaySoundEffects(hoverSound);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isOn = false;
    }

    public void playHoverSound()
    {
        if (isOn)
        {
            var hoverSound = Resources.Load<AudioClip>("Sounds/" + _hoverSoundName);
            AudioManager.Instance.PlaySoundEffects(hoverSound);
        }
    }
}