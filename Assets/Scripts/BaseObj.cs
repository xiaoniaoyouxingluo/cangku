using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基础对象
/// 可能会播放一些模型动画
/// </summary>
public class BaseObj : EntityObj
{
    //对象的动画组件
    private Animator _animator;

    /// <summary>
    /// 获取对象的动画组件
    /// </summary>
    public Animator animator { get => _animator;}
}
