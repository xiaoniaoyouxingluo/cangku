using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// 场景切换管理器 主要用于切换场景
/// </summary>
public class SceneMgr : BaseManager<SceneMgr>
{
    private SceneMgr()
    {

    }
    /// <summary>
    /// 同步切换场景的方法
    /// </summary>
    /// <param name="name">切换场景的名字</param>
    /// <param name="callBack">切换后执行的方法</param>
    public void LoadScene(string name,UnityAction callBack=null)
    {
        SceneManager.LoadScene(name);
        callBack?.Invoke();
    }
    /// <summary>
    /// 异步切换场景的方法
    /// </summary>
    /// <param name="name">切换场景的名字</param>
    /// <param name="callBack">切换后执行的方法</param>
    public void LoadSceneAsyn(string name,UnityAction callBack=null)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsyn(name, callBack));
    }
    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction callBack)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        while(!ao.isDone)
        {
            EventCenter.Instance.EventTrigger<float>("SceneLoadchange", ao.progress);
            yield return 0;
        }
        EventCenter.Instance.EventTrigger<float>("SceneLoadchange", 1);
        callBack?.Invoke();
    }
}
