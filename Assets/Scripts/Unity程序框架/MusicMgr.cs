using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音乐音效管理器
/// </summary>
public class MusicMgr : BaseManager<MusicMgr>
{
    /// <summary>
    /// 背景音乐播放组件
    /// </summary>
    private AudioSource bkMusic = null;

    /// <summary>
    /// 背景音乐大小
    /// </summary>
    private float bkMusicValue = 0.1f;

    /// <summary>
    /// 管理正在播放的音效
    /// </summary>
    private List<AudioSource> soundList = new List<AudioSource>();
    /// <summary>
    /// 音效音量大小
    /// </summary>
    private float soundValue = 0.1f;
    /// <summary>
    /// 背景音乐开关
    /// </summary>
    private bool musicOpen = true;
    /// <summary>
    /// 音效开关
    /// </summary>
    private bool soundOpen = true;
    /// <summary>
    /// 音效是否在播放
    /// </summary>
    private bool soundIsPlay = true;


    private MusicMgr() 
    {
        MonoMgr.Instance.AddFixedUpdateListener(Update);
        bkMusicValue = GameDataMgr.Instance.musicData.musicValue;
        musicOpen = GameDataMgr.Instance.musicData.musicOpen;
        soundValue = GameDataMgr.Instance.musicData.soundValue;
        soundOpen = GameDataMgr.Instance.musicData.soundOpen;
    }


    private void Update()
    {
        if (!soundIsPlay)
            return;

        //不停的遍历容器 检测有没有音效播放完毕 播放完了 就移除销毁它
        //为了避免边遍历边移除出问题 我们采用逆向遍历
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if(!soundList[i].isPlaying)
            {
                //音效播放完毕了 不再使用了 我们将这个音效切片置空
                soundList[i].clip = null;
                PoolMgr.Instance.PushObj(soundList[i].gameObject);
                soundList.RemoveAt(i);
            }
        }
    }


    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name">music文件夹下的路径</param>
    public void PlayBKMusic(string name)
    {
        //动态创建播放背景音乐的组件 并且 不会过场景移除 
        //保证背景音乐在过场景时也能播放
        if(bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BKMusic";
            GameObject.DontDestroyOnLoad(obj);
            bkMusic = obj.AddComponent<AudioSource>();
        }
        //根据传入的背景音乐名字 来播放背景音乐
        Resources.LoadAsync<AudioClip>("music" + name).completed += ((rq) =>
        {
            bkMusic.clip = (rq as ResourceRequest).asset as AudioClip;
            bkMusic.loop = true;
            bkMusic.volume = bkMusicValue;
            bkMusic.mute = !musicOpen;
            bkMusic.Play();
        });
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    /// <summary>
    /// 设置背景音乐大小
    /// </summary>
    /// <param name="v">0-1的值</param>
    public void ChangeBKMusicValue(float v)
    {
        bkMusicValue = v;
        if (bkMusic == null)
            return;
        bkMusic.volume = bkMusicValue;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名字</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="isSync">是否同步加载</param>
    /// <param name="callBack">加载结束后的回调</param>
    public void PlaySound(string name, bool isLoop = false, bool isSync = false, UnityAction<AudioSource> callBack = null)
    {
        //从缓存池中取出音效对象得到对应组件
        AudioSource source = PoolMgr.Instance.GetObj("Sound/soundObj").GetComponent<AudioSource>();
        //如果取出来的音效是之前正在使用的 我们先停止它
        source.Stop();
        if (isSync)
            Resources.LoadAsync<AudioClip>("music" + name).completed += ((rq) =>
            {
                source.clip = (rq as ResourceRequest).asset as AudioClip;
            });
        else
            source.clip = Resources.Load<AudioClip>("music" + name);
        source.loop = isLoop;
        source.volume = soundValue;
        source.mute = !soundOpen;
        source.Play();
        //存储容器 用于记录 方便之后判断是否停止
        //由于从缓存池中取出对象 有可能取出一个之前正在使用的（超上限时）
        //所以我们需要判断 容器中没有记录再去记录 不要重复去添加即可
        if (!soundList.Contains(source))
            soundList.Add(source);
        //传递给外部使用
        callBack?.Invoke(source);
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    /// <param name="source">音效组件对象</param>
    public void StopSound(AudioSource source)
    {
        if(soundList.Contains(source))
        {
            //停止播放
            source.Stop();
            //从容器中移除
            soundList.Remove(source);
            //不用了 清空切片 避免占用
            source.clip = null;
            //放入缓存池
            PoolMgr.Instance.PushObj(source.gameObject);
        }
    }

    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundValue(float v)
    {
        soundValue = v;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = v;
        }
    }
    /// <summary>
    /// 背景音乐开关
    /// </summary>
    /// <param name="isOpen"></param>
    public void ChangeBKMusicOpen(bool isOpen)
    {
        musicOpen = isOpen;
        if (bkMusic == null)
            return;
        bkMusic.mute = !isOpen;
    }
    /// <summary>
    /// 音效开关
    /// </summary>
    /// <param name="isOpen"></param>
    public void ChangeSoundOpen(bool isOpen)
    {
        soundOpen = isOpen;
        for(int i = 0;i < soundList.Count;i++) 
        {
            soundList[i].mute = !isOpen;
        }
    }

    /// <summary>
    /// 继续播放或者暂停所有音效
    /// </summary>
    /// <param name="isPlay">是否是继续播放 true为播放 false为暂停</param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if(isPlay)
        {
            soundIsPlay = true;
            for (int i = 0; i < soundList.Count; i++)
                soundList[i].Play();
        }
        else
        {
            soundIsPlay = false;
            for (int i = 0; i < soundList.Count; i++)
                soundList[i].Pause();
        }
    }

    /// <summary>
    /// 清空音效相关记录 过场景时在清空缓存池之前去调用它
    /// 重要的事情说三遍！！！
    /// 过场景时在清空缓存池之前去调用它
    /// 过场景时在清空缓存池之前去调用它
    /// 过场景时在清空缓存池之前去调用它
    /// </summary>
    public void ClearSound()
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].Stop();
            soundList[i].clip = null;
            PoolMgr.Instance.PushObj(soundList[i].gameObject);
        }
        soundList.Clear();
    }
}
