using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_电池负 : BasicAliveThing
{
    public string BallName;
    public Transform shootPos;
    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_正电池_Attack");
        AudioManager.install.PlaySoundEffectsByName("Sfx_Battle_Chequers_BatteryS_Attack");
        base.Act(code);
    }

    public void Shoot()
    {
        GameObject bullet = Resources.Load<GameObject>("Prefabs/Bullet/" + BallName);
        var nb = GameObjectPool.Instance.CreateGameObject(bullet, shootPos.position);
        nb.GetComponent<Bullet>().parentObj = this;
        nb.GetComponent<Bullet>().speed = 500 * (teamType == TeamType.Team1 ? 1 : -1);
        nb.transform.localScale = new Vector2(Mathf.Abs(nb.transform.localScale.x) * (teamType == TeamType.Team1 ? 1 : -1), nb.transform.localScale.y);
        nb.GetComponent<Bullet>().穿透 = false;
        nb.GetComponent<Bullet>().无视防御 = false;
        nb.GetComponent<Bullet>().ShakeC = new Vector2(0.15f, 0.05f * Damage);

        Invoke("EndAct", 2);
    }
}
