using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_罐头 : BasicAliveThing
{
    public bool 茶杯;
    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_Tin_Attack");
        if (茶杯)
        {
            AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Chequers_Cup_Attack");
        }
        else
        {
            AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Chequers_Tin_Attack");
            //Sfx_Battle_Chequers_Tin_Attack

        }
        base.Act(code);
    }

    public void Attack()
    {
        var e = getThisLineFirstEnemy();
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/hit_pj"), e.transform.position);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.14f, 0.4f);
        }
        EndAct();
    }
}
