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

        //ͨ������ ������ ���ֵĴ�С�Ϳ���
        MusicData data = GameDataMgr.Instance.musicData;
        ChangeValue(data.musicValue);
    }

    //���ر������ֵķ���
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }

    /// <summary>
    /// �������������ִ�С�ķ���
    /// </summary>
    /// <param name="v"></param>
    public void ChangeValue(float v)
    {
        bkSource.volume = v;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="clip">���ŵ���Ƶ��Ƭ</param>
    /// <param name="isloop">�Ƿ�ѭ��</param>
    public void PlayMusic(AudioClip clip,bool isloop=true)
    {
        bkSource.clip = clip;
        bkSource.loop = isloop;
        bkSource.Play();
    }
}
