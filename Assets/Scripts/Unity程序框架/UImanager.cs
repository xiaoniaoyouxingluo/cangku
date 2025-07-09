using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UImanager:BaseManager<UImanager>
{
    //�����е� Canvas���� ��������Ϊ���ĸ�����
    private Transform canvasTrans;
    public Transform GetcanvasTrans => canvasTrans;
    private UImanager() 
    {
        //�õ������е�Canvas����
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //ͨ�����������Ƴ��ö��� ��֤�����Ϸ������ ֻ��һ�� canvas����
        GameObject.DontDestroyOnLoad(canvas);
    }
    //�洢�����ϵ����
    private Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();
    public T �������<T>()where T : BasePanel
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
    public void ɾ�����<T>(bool isFade=true)
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
    /// Ϊ�ؼ�����Զ����¼�
    /// </summary>
    /// <param name="control">��Ӧ�Ŀؼ�</param>
    /// <param name="type">�¼�������</param>
    /// <param name="callBack">��Ӧ�ĺ���</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        //�����߼���Ҫ�����ڱ�֤ �ؼ���ֻ�����һ��EventTrigger
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}