using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_叉子 : BasicAliveThing
{
    public override void Act(int code)
    {
        GetComponent<Animator>().Play("AT_叉子_Attack");
        AudioManager.install.PlaySoundEffectsByName("Sfx_Battle_Chequers_Fork_Attack");
        base.Act(code);
    }

    public void Attack()
    {
        var e = getThisLineFirstEnemy();
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/hit_pj_00009"), e.transform.position);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.14f, 0.4f);
        }
        EndAct();
    }
}
