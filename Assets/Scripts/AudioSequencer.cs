using UnityEngine;
using System.Collections;

public class AudioSequencer : MonoBehaviour
{
    public AudioClip xx_pre;  // 前置音频片段
    public AudioClip xx_main; // 主循环音频片段

    private AudioSource audioSource;

    void Start()
    {
        // 确保有AudioSource组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 开始播放序列
        StartCoroutine(PlayAudioSequence());
    }

    IEnumerator PlayAudioSequence()
    {
        // 1. 播放前置音频
        audioSource.clip = xx_pre;
        audioSource.loop = false; // 确保不循环
        audioSource.Play();

        // 2. 等待前置音频播放完成
        yield return new WaitForSeconds(xx_pre.length);

        // 3. 播放主音频并开启循环
        audioSource.clip = xx_main;
        audioSource.loop = true; // 启用循环
        audioSource.Play();
    }
}