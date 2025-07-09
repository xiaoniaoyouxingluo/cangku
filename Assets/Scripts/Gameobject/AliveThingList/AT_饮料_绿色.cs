using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_饮料_绿色 : AT_饮料
{
    public override void OnDie()
    {
        foreach (var g in inBattleManager.Instance.getAllObj(teamType))
        {
            if (g != null && g != gameObject)
            {
                g.GetComponent<BuffManager>().AddBuff(new BuffData() { Name = "力量", times = 5, canBeStacked = true, lasting = true });
                GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/growup_00002"), g.transform.position);
            }
        }
        inBattleManager.Instance.GhostNum += 2;

        base.OnDie();
    }
}
