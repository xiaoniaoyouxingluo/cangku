using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0f; // 持续时间
    public float shakeAmount = 0f; // 幅度
    public float decreaseFactor = 0f; // 震动衰减(大概率不用动)
    private static CameraShake instance;
    public static CameraShake Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Camera.main.gameObject.AddComponent<CameraShake>();
            }
            return instance;
        }
    }

    private Vector3 originalPos; // 初始摄像机位置

    void Start()
    {
        originalPos = transform.localPosition; // 保存初始摄像机位置
        shakeDuration = -999f;
        instance = this;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount; // 在原始位置上加上随机震动

            shakeDuration -= Time.deltaTime * decreaseFactor; // 震动持续时间递减
        }
        else if (shakeDuration > -5)
        {
            shakeDuration = -5f;
            transform.localPosition = originalPos; // 复位摄像机位置
        }
    }

    //平常都调用这个
    public void TriggerShake(float Time, float Forse, float factor = 1.0f)
    {
        shakeDuration = Time; // 触发震动效果，设置持续时间
        shakeAmount = Forse;
        decreaseFactor = factor;
       
    }
}