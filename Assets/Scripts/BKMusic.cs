using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instacne => instance;

    private AudioSource bkSource;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        bkSource = this.GetComponent<AudioSource>();

        //通过数据 来设置 音乐的大小和开关
        MusicData data = GameDataMgr.Instance.musicData;
        ChangeValue(data.musicValue);
    }

    //开关背景音乐的方法
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }

    /// <summary>
    /// 调整被背景音乐大小的方法
    /// </summary>
    /// <param name="v"></param>
    public void ChangeValue(float v)
    {
        bkSource.volume = v;
    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="clip">播放的音频切片</param>
    /// <param name="isloop">是否循环</param>
    public void PlayMusic(AudioClip clip,bool isloop=true)
    {
        bkSource.clip = clip;
        bkSource.loop = isloop;
        bkSource.Play();
    }
}
