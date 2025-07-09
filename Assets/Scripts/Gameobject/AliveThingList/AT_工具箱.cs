using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_工具箱 : BasicAliveThing
{
    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_工具箱_Attack");
        //Sfx_Battle_Chequers_Chest_Attack
        AudioManager.install.PlaySoundEffectsByName("Sfx_Battle_Chequers_Chest_Attack");
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
            CameraShake.Instance.TriggerShake(0.3f, 0.6f);
        }
        var allY = inBattleManager.Instance.getAllObj(teamType);
        int minH = 999;
        GameObject aimG = null;
        foreach(var g in allY)
        {
            if(g != null && g != gameObject)
            {
                if (!g.GetComponent<HealthMgr>().isDead && g.GetComponent<HealthMgr>().nowHealth <= minH)
                {
                    aimG = g;
                    minH = g.GetComponent<HealthMgr>().nowHealth;
                }
            }
        }
        if(aimG != null)
        {
            aimG.GetComponent<HealthMgr>().Recover(30);
        }
        EndAct();
    }
}
