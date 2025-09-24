using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 实体属性  主要表示 场景上出现的对象就会有的属性
/// </summary>
public class EntityProperty
{
    /// <summary>
    /// 唯一ID
    /// </summary>
    public int uID;
    /// <summary>
    /// 数据表ID
    /// </summary>
    public int tableID;
    /// <summary>
    /// 模型资源名
    /// </summary>
    public string resName;

    public EntityProperty()
    {

    }
    /// <summary>
    /// 初始化数据 
    /// </summary>
    /// <param name="id">传入表的模板ID</param>
    public virtual void SetData(int id) { }
 }
