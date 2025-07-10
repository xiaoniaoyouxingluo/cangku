using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
/// <summary>
/// ���ֹ�����
/// </summary>
public class AudioManager : SingletonMono<AudioManager>
{
    //��������Ƶ�������
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
                UImanager.Instance.ɾ�����<SettingPanel>();
            }
            else
            {
                settingIsOn = true;
                UImanager.Instance.�������<SettingPanel>();
            }
        }
    }
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="name">��Ч������</param>
    public void PlaySoundEffectsByName(string name)
    {
        var ac = Resources.Load<AudioClip>("Sounds/" + name);
        ads.PlayOneShot(ac);
    }
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="ac">��Ч��Ƭ�ļ�</param>
    public void PlaySoundEffects(AudioClip ac)
    {
        if (ac == null)
            return;
        ads.PlayOneShot(ac);
    }
    /// <summary>
    /// ���������Ч
    /// </summary>
    /// <param name="acs">��Ч�б�</param>
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
    /// ������Ч��С�ķ���
    /// </summary>
    /// <param name="v"></param>
    public void ChangeValue(float v)
    {
        ads.volume = v;
    }
}
