using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_面包机 : BasicAliveThing
{
    public override void Act(int code)
    {
        GetComponent<Animator>().Play("AT_面包机_Attack");
        AudioManager.install.PlaySoundEffectsByName("Sfx_Battle_Chequers_BreadMaker_Attack");
        base.Act(code);
    }

    public void Attack()
    {
        var e = getLineObj(1);
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/hit_pj"), e.transform.position);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.14f, 0.4f);
        }
        e = getLineObj(2);
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
