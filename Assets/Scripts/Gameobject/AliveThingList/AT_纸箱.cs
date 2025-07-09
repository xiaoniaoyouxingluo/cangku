using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_纸箱 : BasicAliveThing
{
    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_纸箱_Attack");
        base.Act(code);
    }

    public void Attack()
    {
        var e = getThisLineFirstEnemy();
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/hit_pj")).transform.position = new Vector2(e.transform.position.x, e.transform.position.y);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.15f, 0.5f);
        }
        foreach (var g in inBattleManager.Instance.getObjLineAllObj(gameObject))
        {
            if (g == null)
            {
                continue;
            }
            if (g == gameObject)
            {
                continue;
            }
            if (g.GetComponent<BasicAliveThing>().teamType == teamType)
            {
                g.GetComponent<HealthMgr>().Sheild+=10;
                GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/AddSheildTX"), g.transform.position);
               
                //AddSheildTX
            }
        }
        EndAct();

    }
}
