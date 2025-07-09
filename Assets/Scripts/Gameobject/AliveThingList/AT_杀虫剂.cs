using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_杀虫剂 : BasicAliveThing
{
    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_杀虫剂_Attack");
        AudioManager.install.PlaySoundEffectsByName("Sfx_Battle_Chequers_Spray_Attack");
        base.Act(code);
    }

    public void Attack()
    {
        var e = getThisLineFirstEnemy();
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/hit_pj_g")).transform.position = new Vector2(e.transform.position.x, e.transform.position.y);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            if(e.GetComponent<HealthMgr>().Sheild <= 0 && e.GetComponent<Boss_冰箱>() == null)
            {
                e.GetComponent<HealthMgr>().nowHealth = 0;
                e.GetComponent<HealthMgr>().HealthLess((int)999, gameObject, false);
            }
            else
            {
                e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            }
            
            CameraShake.Instance.TriggerShake(0.8f, 0.3f);
        }
        EndAct();
    }
}
