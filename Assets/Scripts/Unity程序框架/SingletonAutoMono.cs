using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 自动挂载式的 继承Mono的单例模式基类
/// 推荐使用 
/// 无需手动挂载 无需动态添加 无需关心切场景带来的问题
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
                    // 在场景中查找是否已存在实例
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        // 创建新的GameObject并添加组件
                        GameObject singletonObj = new GameObject();
                        instance = singletonObj.AddComponent<T>();
                        singletonObj.name = $"{typeof(T).Name} (Singleton)";

                        // 设置为不销毁
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
    /// 基类Awake方法，确保单例唯一性
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
    /// 当应用退出时标记，防止重新创建实例
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    /// <summary>
    /// 当对象被销毁时重置实例引用
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
