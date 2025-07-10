using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_闹钟 : BasicAliveThing
{
    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_闹钟_Act");
        AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Chequers_AlarmClock_Attack");
        base.Act(code);
    }

    public void Attack()
    {
        var e = getThisLineFirstEnemy();
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/yinboTX")).transform.position = new Vector2(e.transform.position.x,e.transform.position.y);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.14f, 0.4f);
        }
        EndAct();
    }
}
