using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// �����л������� ��Ҫ�����л�����
/// </summary>
public class SceneMgr : BaseManager<SceneMgr>
{
    private SceneMgr()
    {

    }
    /// <summary>
    /// ͬ���л������ķ���
    /// </summary>
    /// <param name="name">�л�����������</param>
    /// <param name="callBack">�л���ִ�еķ���</param>
    public void LoadScene(string name,UnityAction callBack=null)
    {
        SceneManager.LoadScene(name);
        callBack?.Invoke();
    }
    /// <summary>
    /// �첽�л������ķ���
    /// </summary>
    /// <param name="name">�л�����������</param>
    /// <param name="callBack">�л���ִ�еķ���</param>
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
