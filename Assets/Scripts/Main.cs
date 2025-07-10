using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public List<AudioClip> clips = new List<AudioClip>();
    private void Start()
    {
        //UImanager.Instance.ToString(); 
        // 显示开始界面
        UImanager.Instance.创建面板<StartPanel>();
        BKMusic.Instacne.PlayMusic(clips[Random.Range(0, clips.Count)]);
        //PlayMusic("MainTheme");
    }
}
