using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_电风扇 : BasicAliveThing
{
    public Transform shootPos;
    public override void Act(int code)
    {

        //之后也许可以判断羁绊之类的
        GetComponent<Animator>().Play("AT_电风扇_Attack");
        //Sfx_Battle_Chequers_Fan_Attack
        AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Chequers_Fan_Attack");
        base.Act(code);
    }

    public void Shoot()
    {
        GameObject bullet = Resources.Load<GameObject>("Prefabs/Bullet/fanbullet");
        var nb = GameObjectPool.Instance.CreateGameObject(bullet, shootPos.position);
        nb.GetComponent<Bullet>().parentObj = this;
        nb.GetComponent<Bullet>().speed = 500 * (teamType == TeamType.Team1 ? 1 : -1);
        nb.transform.localScale = new Vector2(Mathf.Abs(nb.transform.localScale.x) * (teamType == TeamType.Team1 ? 1 : -1), nb.transform.localScale.y);
        nb.GetComponent<Bullet>().穿透 = true;
        nb.GetComponent<Bullet>().无视防御 = false;
        nb.GetComponent<Bullet>().ShakeC = new Vector2(0.15f, 0.05f * Damage);
        Invoke("EndAct", 2);
    }

    public override void OnPlace()
    {
        base.OnPlace();
    }


}
