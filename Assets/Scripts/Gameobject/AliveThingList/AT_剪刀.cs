using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_剪刀 : BasicAliveThing
{
    public override void Act(int code)
    {
        GetComponent<Animator>().Play("AT_剪刀_Attack");
        AudioManager.install.PlaySoundEffectsByName("Sfx_Battle_Chequers_Scissors_Attack");
        base.Act(code);
    }

    public void Attack()
    {
        var e = getThisLineFirstEnemy();
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/hit_pj"), e.transform.position);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            if (e.GetComponent<BuffManager>().hasBuff("装甲", 2))
            {
                d *= 2;
            }
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.14f, 0.4f);
        }
        EndAct();
    }
}
