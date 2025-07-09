using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager install;
    public AudioSource ads;
    public bool settingIsOn = false;
    // Use this for initialization
    void Awake()
    {
        if (AudioManager.install == null)
        {
            DontDestroyOnLoad(this);
            AudioManager.install = this;
        }
        ads = GetComponent<AudioSource>();
    }
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        ads.volume = PlayerPrefs.GetFloat("SFX_Volume", 1);
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingIsOn)
            {
                AudioManager.install.settingIsOn = false;
                UImanager.Instance.删除面板<SettingPanel>();
            }
            else
            {
                UImanager.Instance.创建面板<SettingPanel>();
            }
        }
    }
    public void PlaySoundEffectsByName(string name)
    {
        var ac = Resources.Load<AudioClip>("Sounds/" + name);
        ads.PlayOneShot(ac);
    }
    public void PlaySoundEffects(AudioClip ac)
    {
        if (ac == null)
        {
            return;
        }
        ads.PlayOneShot(ac);
    }

    public void PlaySoundRandomEffect(List<AudioClip> acs)
    {
        if (acs == null)
        {
            return;
        }
        if (acs.Count == 0)
        {
            return;
        }
        var ac = acs[Random.Range(0, acs.Count)];
        ads.PlayOneShot(ac);
    }
}
