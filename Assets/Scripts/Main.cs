using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        UImanager.Instance.ToString(); 
        // 显示开始界面
        UImanager.Instance.创建面板<StartPanel>();
       
        //PlayMusic("MainTheme");
    }
}
