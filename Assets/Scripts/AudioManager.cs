using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
/// <summary>
/// 音乐管理器
/// </summary>
public class AudioManager : SingletonMono<AudioManager>
{
    //关联的音频播放组件
    public AudioSource ads;
    public bool settingIsOn = false;
    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        ads = GetComponent<AudioSource>();
    }
    void Start()
    {
        ChangeValue(GameDataMgr.Instance.musicData.soundValue);
    }
    // Update is called once per frame
    void Update()
    {
        ads.volume = GameDataMgr.Instance.musicData.soundValue;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingIsOn)
            {
                Instance.settingIsOn = false;
                UImanager.Instance.删除面板<SettingPanel>();
            }
            else
            {
                settingIsOn = true;
                UImanager.Instance.创建面板<SettingPanel>();
            }
        }
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效的名字</param>
    public void PlaySoundEffectsByName(string name)
    {
        var ac = Resources.Load<AudioClip>("Sounds/" + name);
        ads.PlayOneShot(ac);
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="ac">音效切片文件</param>
    public void PlaySoundEffects(AudioClip ac)
    {
        if (ac == null)
            return;
        ads.PlayOneShot(ac);
    }
    /// <summary>
    /// 随机播放音效
    /// </summary>
    /// <param name="acs">音效列表</param>
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
    /// <summary>
    /// 调整音效大小的方法
    /// </summary>
    /// <param name="v"></param>
    public void ChangeValue(float v)
    {
        ads.volume = v;
    }
}
