using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_饮料_红色 : AT_饮料
{
    public override void OnDie()
    {
        foreach (var g in inBattleManager.Instance.getAllObj(teamType))
        {
            if (g != null && g != gameObject)
            {
                g.GetComponent<HealthMgr>().Recover(40);
            }
        }
        inBattleManager.Instance.GhostNum += 2;
        base.OnDie();
    }
}
