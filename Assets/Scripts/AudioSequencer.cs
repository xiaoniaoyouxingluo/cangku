using UnityEngine;
using System.Collections;

public class AudioSequencer : MonoBehaviour
{
    public AudioClip xx_pre;  // ǰ����ƵƬ��
    public AudioClip xx_main; // ��ѭ����ƵƬ��

    private AudioSource audioSource;

    void Start()
    {
        // ȷ����AudioSource���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ��ʼ��������
        StartCoroutine(PlayAudioSequence());
    }

    IEnumerator PlayAudioSequence()
    {
        // 1. ����ǰ����Ƶ
        audioSource.clip = xx_pre;
        audioSource.loop = false; // ȷ����ѭ��
        audioSource.Play();

        // 2. �ȴ�ǰ����Ƶ�������
        yield return new WaitForSeconds(xx_pre.length);

        // 3. ��������Ƶ������ѭ��
        audioSource.clip = xx_main;
        audioSource.loop = true; // ����ѭ��
        audioSource.Play();
    }
}