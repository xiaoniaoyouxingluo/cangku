using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体对象类 
/// 不会有动画 不会战斗 不会移动的 静态物体的
/// </summary>
public class EntityObj : MonoBehaviour
{
    //对象属性相关
    protected EntityProperty _property;

    //父对象的Transform（模型资源依附的父物体）
    private Transform _rootTransform;
    //身体的Transform（模型资源对象的Transform）
    private Transform _bodyTransform;

    protected virtual void Awake()
    {

    }
    
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// 用于初始化对象属性的方法  在其中进行对象以及属性相关的初始化
    /// </summary>
    /// <param name="ids">变长参数 根据表的主键决定传几个int</param>
    public virtual void InitObj(int id)
    {

    }

    /// <summary>
    /// 获取属性对象的方法
    /// </summary>
    /// <typeparam name="T">属性类型</typeparam>
    /// <returns></returns>
    public virtual T GetProperty<T>() where T:EntityProperty
    {
        return _property as T;
    }

    /// <summary>
    /// 父对象的Transform（模型资源依附的父物体）
    /// </summary>
    public Transform rootTransform { get => _rootTransform; }
    /// <summary>
    /// 身体的Transform（模型资源对象的Transform）
    /// </summary>
    public Transform bodyTransform { get => _bodyTransform; }
}
