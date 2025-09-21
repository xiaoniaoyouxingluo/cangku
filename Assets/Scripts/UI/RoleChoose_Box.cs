using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 灵居预设体关联组件
/// </summary>
public class RoleChoose_Box : MonoBehaviour
{
    /// <summary>
    /// 记录控件关联的灵居预设体
    /// </summary>
    public GameObject AimObj;
    /// <summary>
    /// 控件关联的灵居id
    /// </summary>
    public int id;
    //记录控件位置
    Vector3 StartLocalPos;
    private void Start()
    {
        StartLocalPos = transform.position;
    }

}
