using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public abstract class BasePanel : MonoBehaviour
{
    //������Ƶ��뵭���Ļ����� ���
    private CanvasGroup canvasGroup;
    //���뵭�����ٶ�
    private float alphaSpeed = 10;
    //�Ƿ�ʼ��ʾ
    private bool isShow = true;
    //���Լ������ɹ�ʱ Ҫִ�е�ί�к���
    private UnityAction hideCallBack;
    /// <summary>
    /// ���ڴ洢����Ҫ�õ���UI�ؼ�������ʷ�滻ԭ�� ����װ����
    /// </summary>
    protected Dictionary<string, UIBehaviour> controlDic = new Dictionary<string, UIBehaviour>();

    /// <summary>
    /// �ؼ�Ĭ������ ����õ��Ŀؼ����ִ������������ ��ζ�����ǲ���ͨ������ȥʹ���� ��ֻ��������ʾ���õĿؼ�
    /// </summary>
    private static List<string> defaultNameList = new List<string>() { "Image",
                                                                   "Text (TMP)",
                                                                   "RawImage",
                                                                   "Background",
                                                                   "Checkmark",
                                                                   "Label",
                                                                   "Text (Legacy)",
                                                                   "Arrow",
                                                                   "Placeholder",
                                                                   "Fill",
                                                                   "Handle",
                                                                   "Viewport",
                                                                   "Scrollbar Horizontal",
                                                                   "Scrollbar Vertical"};


    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        //Ϊ�˱��� ĳһ�������ϴ������ֿؼ������
        //����Ӧ�����Ȳ�����Ҫ�����
        FindChildrenControl<Button>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<InputField>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Dropdown>();
        //��ʹ�����Ϲ����˶����� ֻҪ�����ҵ�����Ҫ���
        //֮��Ҳ����ͨ����Ҫ����õ������������ص�����
        FindChildrenControl<Text>();
        FindChildrenControl<TextMeshPro>();
        FindChildrenControl<Image>();
        FindChildrenControl<RawImage>();
    }
    protected virtual void Update()
    {
        if (isShow && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        else if (!isShow && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
    }
    /// <summary>
    /// �����ʾʱ����õ��߼�
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// �������ʱ����õ��߼�
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        //��¼����ĵ������ɹ����ִ�еĺ���
        hideCallBack = callBack;
    }

    /// <summary>
    /// ��ȡָ�������Լ�ָ�����͵����
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="name">�������</param>
    /// <returns></returns>
    public T GetControl<T>(string name) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(name))
        {
            T control = controlDic[name] as T;
            if (control == null)
                Debug.LogError($"�����ڶ�Ӧ����{name}����Ϊ{typeof(T)}�����");
            return control;
        }
        else
        {
            print($"�����ڶ�Ӧ����{name}�����");
            return null;
        }
    }

    protected virtual void ClickBtn(string btnName)
    {

    }

    protected virtual void SliderValueChange(string sliderName, float value)
    {

    }

    protected virtual void ToggleValueChange(string toggleName, bool value)
    {

    }
    protected virtual void InputFieldValueChange(string inputFieldName, string value)
    {

    }
    protected virtual void InputFieldEndEdit(string inputFieldName, string value)
    {

    }
    protected virtual void ScrollRectValueChange(string scrollRectName, Vector2 value)
    {

    }
    protected virtual void DropdownValueChange(string dropdownName, int value)
    {

    }

    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>(true);
        for (int i = 0; i < controls.Length; i++)
        {
            //��ȡ��ǰ�ؼ�������
            string controlName = controls[i].gameObject.name;
            //ͨ�����ַ�ʽ ����Ӧ�����¼���ֵ���
            if (!controlDic.ContainsKey(controlName))
            {
                if (!defaultNameList.Contains(controlName))
                {
                    controlDic.Add(controlName, controls[i]);
                    //�жϿؼ������� �����Ƿ���¼�����
                    if (controls[i] is Button)
                    {
                        (controls[i] as Button).onClick.AddListener(() =>
                        {
                            ClickBtn(controlName);
                        });
                    }
                    else if (controls[i] is Slider)
                    {
                        (controls[i] as Slider).onValueChanged.AddListener((value) =>
                        {
                            SliderValueChange(controlName, value);
                        });
                    }
                    else if (controls[i] is Toggle)
                    {
                        (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                        {
                            ToggleValueChange(controlName, value);
                        });
                    }
                    else if (controls[i] is InputField)
                    {
                        (controls[i] as InputField).onEndEdit.AddListener((value) =>
                        {
                            InputFieldEndEdit(controlName, value);
                        });
                        (controls[i] as InputField).onValueChanged.AddListener((value) =>
                        {
                            InputFieldValueChange(controlName, value);
                        });
                    }
                    else if (controls[i] is ScrollRect)
                    {
                        (controls[i] as ScrollRect).onValueChanged.AddListener((value) =>
                        {
                            ScrollRectValueChange(controlName, value);
                        });
                    }
                    else if (controls[i] is Dropdown)
                    {
                        (controls[i] as Dropdown).onValueChanged.AddListener((value) =>
                        {
                            DropdownValueChange(controlName, value);
                        });
                    }
                }
            }
        }
    }
}