using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_冰箱 : BasicAliveThing
{
    public int index;
    public List<GameObject > aa饮料s = new List<GameObject>();
    public List<GameObject> aa地皮 = new List<GameObject>();
    public List<string> acts = new List<string>() 
    {
        "Create",
        "Ice",
        "Attack"
    };
    public Text ifss;

    public override void Act(int code)
    {
        runAct(acts[index]);
        base.Act(code);
        index++;
        if(index == acts.Count)
        {
            index= 0;
        }
    }

    public void runAct(string act)
    {
        GetComponent<Animator>().Play("Boss_冰箱_"+act);
    }

    private void FixedUpdate()
    {
        if(index == 0)
        {
            Damage = 0;
            ifss.text = "他将要[召唤]";
        }else if(index == 1)
        {
            Damage = 5;
            ifss.text = "他将要全体攻击+施加负面";
        }else if(index == 2)
        {
            Damage = 80;
            ifss.text = "他将要随机选路攻击";
        }
    }

    public void Create()
    {
        //Sfx_Battle_Buff_AP
        AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Buff_AP");
        for (int i = 0;i < aa地皮.Count;i++)
        {
            var a = aa地皮[i];
            玩家可放入槽位 ac = a.GetComponent<玩家可放入槽位>();
            if( ac.此地物体 == null)
            {
                var ss = GameObjectPool.Instance.CreateGameObject(aa饮料s[i], a.transform.position);
                var sjjd = a;
                ss.transform.SetParent(sjjd.transform);
                if (sjjd.GetComponent<玩家可放入槽位>().Line == 1)
                {
                    inBattleManager.Instance.Line1[sjjd.GetComponent<玩家可放入槽位>().Index + 2] = ss;
                }
                if (sjjd.GetComponent<玩家可放入槽位>().Line == 2)
                {
                    inBattleManager.Instance.Line2[sjjd.GetComponent<玩家可放入槽位>().Index + 2] = ss;
                }
                ac.此地物体 = ss;
                ss.transform.localScale = new Vector2(1, 1);
                ss.transform.localPosition = Vector2.zero;
                ss.GetComponent<BasicAliveThing>().teamType = TeamType.Team2;
                ss.GetComponent<BasicAliveThing>().Reset();
            }
            else
            {
                ac.此地物体.GetComponent<HealthMgr>().Recover(40);
            }
        }
        EndAct();
    }

    public void Attack()
    {
        //Sfx_Battle_GeneralHit
        AudioManager.Instance.PlaySoundEffectsByName("wep_peashooter_peacannon_core_mono_ice");
        var e = getLineObj(1);
        int ran = Random.Range(0, 100);
        if(ran < 50)
        {
            e = getLineObj(2);
        }
        if (e != null)
        {
            GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/BlizzardPrison2"), e.transform.position);
            var d = (GetComponent<BuffManager>().damageScale * (80 + GetComponent<BuffManager>().damagePlus));
            e.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
            CameraShake.Instance.TriggerShake(0.25f, 0.8f);
        }
        EndAct();
    }

    public void Ice()
    {
        //Explosion_IceShroom_CoreClose
        AudioManager.Instance.PlaySoundEffectsByName("Explosion_IceShroom_CoreClose");
        foreach (var g in inBattleManager.Instance.getAllObj(TeamType.Team1))
        {
            if (g != null)
            {
                g.GetComponent<BuffManager>().AddBuff(new BuffData() { Name = "力量", times = -6, canBeStacked = true, lasting = true });
                GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/Particle System_Snow"), g.transform.position);
                var d = (GetComponent<BuffManager>().damageScale * (5 + GetComponent<BuffManager>().damagePlus));
                g.GetComponent<HealthMgr>().HealthLess((int)d, gameObject, false);
                CameraShake.Instance.TriggerShake(0.1f, 0.1f);
            }
        }
        EndAct();
    }
}
