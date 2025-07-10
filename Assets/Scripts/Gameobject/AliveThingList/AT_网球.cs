using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_网球 : BasicAliveThing
{

    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_网球_Attack");
        AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Chequers_Baseball_Attack");
        base.Act(code);
    }

    public void Attack()
    {
        Damage = 24;
        var e = getThisLineFirstEnemy();
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/hit_pj")).transform.position = new Vector2(e.transform.position.x, e.transform.position.y);
            var d = (GetComponent<BuffManager>().damageScale * (Damage + GetComponent<BuffManager>().damagePlus));
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.2f, 0.3f);
        }
        EndAct();
    }

    public override void OnDie()
    {
        Damage = 40;
        GameObject bullet = Resources.Load<GameObject>("Prefabs/Bullet/baseball");
        var nb = GameObjectPool.Instance.CreateGameObject(bullet, transform.position);
        nb.GetComponent<Bullet>().parentObj = this;
        nb.GetComponent<Bullet>().speed = 500 * (teamType == TeamType.Team1 ? 1 : -1);
        nb.transform.localScale = new Vector2(Mathf.Abs(nb.transform.localScale.x) * (teamType == TeamType.Team1 ? 1 : -1), nb.transform.localScale.y);
        nb.GetComponent<Bullet>().穿透 = false;
        nb.GetComponent<Bullet>().无视防御 = false;
        nb.GetComponent<Bullet>().ShakeC = new Vector2(0.15f, 0.05f * Damage);
        base.OnDie();
        GameObjectPool.Instance.AddObject(gameObject);
        if (inBattleManager.Instance.Line1.Contains(gameObject))
        {
            inBattleManager.Instance.Line1[inBattleManager.Instance.Line1.IndexOf(gameObject)] = null;
        }
        if (inBattleManager.Instance.Line2.Contains(gameObject))
        {
            inBattleManager.Instance.Line2[inBattleManager.Instance.Line2.IndexOf(gameObject)] = null;
        }
        if (transform.parent.GetComponent<玩家可放入槽位>().此地物体 == gameObject)
        {
            transform.parent.GetComponent<玩家可放入槽位>().此地物体 = null;
        }
        transform.parent = null;
    }
    public override void Reset()
    {
        Damage = 24;
        base.Reset();
    }

}
