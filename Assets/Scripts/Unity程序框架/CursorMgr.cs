using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CursorState
{
    Default,
    ClickDown,
    Explore,//�������������Ƶ����������Χ��ʾ���ܵģ����԰�����Ϊ�Ŵ�����
    Forbidden//��ֹ״̬
}
public class CursorMgr : SingletonAutoMono<CursorMgr>
{
    public CursorState CursorState;
    [Header("Cursor Textures")]
    public Texture2D defaultCursor;
    public Texture2D pressedCursor;
    public Texture2D exploreCursor;
    public Texture2D forbiddenCursor;
    public Vector2 hotspot = Vector2.zero;

    protected override void Awake()
    {
        SetDefaultCursor();
      
        base.Awake();
    }


    void Update()
    {
        //����и���ͻ�switch��
        if (CursorState == CursorState.Forbidden)//��ֹ״̬
        {
            Cursor.SetCursor(forbiddenCursor, hotspot, CursorMode.Auto);
        }
        else if(CursorState == CursorState.Explore)//����״̬
        {
            Cursor.SetCursor(exploreCursor, hotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonDown(0))//����
        {
            CursorState = CursorState.ClickDown;
            Cursor.SetCursor(pressedCursor, hotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0))//ԭ��״̬
        {
            CursorState = CursorState.Default;
            SetDefaultCursor();
        }
    }
    public void Init()
    {
        defaultCursor = Resources.Load<Texture2D>("Images/Cursor/Cursor_Default");
        pressedCursor = Resources.Load<Texture2D>("Images/Cursor/Cursor_Click");
        exploreCursor = Resources.Load<Texture2D>("Images/Cursor/Cursor_Explore");
        forbiddenCursor = Resources.Load<Texture2D>("Images/Cursor/Cursor_Forbidden");
    }

    void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    void OnDestroy()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
