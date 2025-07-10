using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_扳手 : BasicAliveThing
{
    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_扳手_Attack");
        AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Chequers_Wrench_Attack");
        base.Act(code);
    }

    public void Attack()
    {
        var e = getThisLineFirstEnemy();
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/BlizzardPrison")).transform.position = new Vector2(e.transform.position.x, e.transform.position.y);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.2f, 0.3f);
        }
        foreach(var g in inBattleManager.Instance.getObjLineAllObj(gameObject))
        {
            if (g == null)
            {
                continue;
            }
            if (g == gameObject)
            {
                continue;
            }
            if(g.GetComponent<BasicAliveThing>().teamType == teamType)
            {
                g.GetComponent<HealthMgr>().Recover(8);
            }
        }
        EndAct();
    }
}
