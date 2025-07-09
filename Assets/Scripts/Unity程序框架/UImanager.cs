using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UImanager:BaseManager<UImanager>
{
    //场景中的 Canvas对象 用于设置为面板的父对象
    private Transform canvasTrans;
    public Transform GetcanvasTrans => canvasTrans;
    private UImanager() 
    {
        //得到场景中的Canvas对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //通过过场景不移除该对象 保证这个游戏过程中 只有一个 canvas对象
        GameObject.DontDestroyOnLoad(canvas);
    }
    //存储场景上的面板
    private Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();
    public T 创建面板<T>()where T : BasePanel
    {
        string str = typeof(T).Name;
        if (panels.ContainsKey(str))
            return panels[str] as T;
        GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UI/" + str));
        go.transform.SetParent(canvasTrans, false);
        panels.Add(str, go.GetComponent<BasePanel>());
        panels[str].ShowMe();
        return panels[str] as T;
    }
    public void 删除面板<T>(bool isFade=true)
    {
        string str = typeof(T).Name;
        if (!panels.ContainsKey(str))
            return;
        if (isFade)
        {
            panels[str].HideMe(() =>
            {
                GameObject.Destroy(panels[str].gameObject);
                panels.Remove(str);
            });
        }
        else
        {
            GameObject.Destroy(panels[str].gameObject);
            panels.Remove(str);
        }
    }
    public T GetPanel<T>()where T:BasePanel
    {
        string str = typeof(T).Name;
        if (panels.ContainsKey(str))
            return panels[str] as T;
        return null;
    }
    /// <summary>
    /// 为控件添加自定义事件
    /// </summary>
    /// <param name="control">对应的控件</param>
    /// <param name="type">事件的类型</param>
    /// <param name="callBack">响应的函数</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        //这种逻辑主要是用于保证 控件上只会挂载一个EventTrigger
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}