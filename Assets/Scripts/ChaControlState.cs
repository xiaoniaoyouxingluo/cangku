using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///角色的可操作状态
///</summary>
[System.Serializable]
public struct ChaControlState
{
    [Tooltip("是否可以移动")]
    public bool canMove;

    [Tooltip("是否可以使用技能")]
    public bool canUseSkill;

    public ChaControlState(bool canMove = true, bool canUseSkill = true)
    {
        this.canMove = canMove;
        this.canUseSkill = canUseSkill;
    }
    /// <summary>
    /// 操作状态全部true
    /// </summary>
    public void Origin()
    {
        this.canMove = true;
        this.canUseSkill = true;
    }

    public static ChaControlState origin = new ChaControlState(true, true);

    public static ChaControlState operator +(ChaControlState cs1, ChaControlState cs2)
    {
        return new ChaControlState(cs1.canMove & cs2.canMove, cs1.canUseSkill & cs2.canUseSkill);
    }
}
