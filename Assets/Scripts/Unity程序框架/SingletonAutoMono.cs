using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �Զ�����ʽ�� �̳�Mono�ĵ���ģʽ����
/// �Ƽ�ʹ�� 
/// �����ֶ����� ���趯̬��� ��������г�������������
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonAutoMono<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    private static readonly object lockObj = new object();
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            lock (lockObj)
            {
                if (instance == null)
                {
                    // �ڳ����в����Ƿ��Ѵ���ʵ��
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        // �����µ�GameObject��������
                        GameObject singletonObj = new GameObject();
                        instance = singletonObj.AddComponent<T>();
                        singletonObj.name = $"{typeof(T).Name} (Singleton)";

                        // ����Ϊ������
                        DontDestroyOnLoad(singletonObj);

                        Debug.Log($"[Singleton] Created singleton instance of {typeof(T)}");
                    }
                    else
                    {
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// ����Awake������ȷ������Ψһ��
    /// </summary>
    protected virtual void Awake()
    {
        lock (lockObj)
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Debug.LogWarning($"[Singleton] Multiple instances of {typeof(T)} found. Destroying extra instance.");
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// ��Ӧ���˳�ʱ��ǣ���ֹ���´���ʵ��
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    /// <summary>
    /// ����������ʱ����ʵ������
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
