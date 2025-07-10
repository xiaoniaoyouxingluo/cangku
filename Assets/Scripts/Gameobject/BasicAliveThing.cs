using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    Team1,//玩家
    Team2//敌人
}

public class BasicAliveThing : MonoBehaviour
{
    public string RealName;
    public TeamType teamType;
    int getCode;
    public int cost;
    public int Damage;
    public bool 初始面向反转;
    bool becomeDead;
    List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    List<Color> defaultColors = new List<Color>();
    public virtual void Act(int code)
    {
        getCode = code;
    }

    public void EndAct()
    {
        if(getCode == -1)
        {
            return;
        }
        if(teamType== TeamType.Team1)
        {
            EnemyManager.Instance.CallBack(getCode+1);
        }
        else
        {
            EnemyManager.Instance.CallBack(getCode+1);

        }
    }

    private void Update()
    {
        if (GetComponent<HealthMgr>().isDead && !becomeDead)
        {
            becomeDead = true;
            AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Chequers_Dead");
            OnDie();
        }
        if (becomeDead)
        {
            foreach(var r in spriteRenderers)
            {
                r.color -= new Color(0, 0, 0, 0.005f);
                if(r.color.a <= 0)
                {
                    if (gameObject.active)
                    {
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
                    GameObjectPool.Instance.AddObject(gameObject);
                }
            }
        }

    }

    public virtual void OnDie()
    {

    }
    public virtual void OnPlace()
    {

    }

    public GameObject getThisLineFirstEnemy()
    {
        if (teamType == TeamType.Team1)
        {
            if (inBattleManager.Instance.Line1.Contains(gameObject))
            {
                for (int i = 3; i < 6; i++)
                {
                    if (inBattleManager.Instance.Line1[i] != null)
                    {
                        var aim = inBattleManager.Instance.Line1[i];
                        if (!aim.GetComponent<HealthMgr>().isDead)
                        {
                            return aim;
                        }
                    }
                }
            }
            else if (inBattleManager.Instance.Line2.Contains(gameObject))
            {
                for (int i = 3; i < 6; i++)
                {
                    if (inBattleManager.Instance.Line2[i] != null)
                    {
                        var aim = inBattleManager.Instance.Line2[i];
                        if (!aim.GetComponent<HealthMgr>().isDead)
                        {
                            return aim;
                        }
                    }
                }
            }
        }
        else if (teamType == TeamType.Team2)
        {
            if (inBattleManager.Instance.Line1.Contains(gameObject))
            {
                for (int i = 2; i >= 0; i--)
                {
                    if (inBattleManager.Instance.Line1[i] != null)
                    {
                        var aim = inBattleManager.Instance.Line1[i];
                        if (!aim.GetComponent<HealthMgr>().isDead)
                        {
                            return aim;
                        }
                    }
                }
            }
            else if (inBattleManager.Instance.Line2.Contains(gameObject))
            {
                for (int i = 2; i >= 0; i--)
                {
                    if (inBattleManager.Instance.Line2[i] != null)
                    {
                        var aim = inBattleManager.Instance.Line2[i];
                        if (!aim.GetComponent<HealthMgr>().isDead)
                        {
                            return aim;
                        }
                    }
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 获取某行第一个怪物
    /// </summary>
    /// <param name="line">行数，只能是1或2</param>
    /// <returns>返回那个物体</returns>
    public GameObject getLineObj(int line)
    {
        if (teamType == TeamType.Team1)
        {
            if (line == 1)
            {
                for (int i = 3; i < 6; i++)
                {
                    if (inBattleManager.Instance.Line1[i] != null)
                    {
                        var aim = inBattleManager.Instance.Line1[i];
                        if (!aim.GetComponent<HealthMgr>().isDead)
                        {
                            return aim;
                        }
                    }
                }
            }
            else if (line == 2)
            {
                for (int i = 3; i < 6; i++)
                {
                    if (inBattleManager.Instance.Line2[i] != null)
                    {
                        var aim = inBattleManager.Instance.Line2[i];
                        if (!aim.GetComponent<HealthMgr>().isDead)
                        {
                            return aim;
                        }
                    }
                }
            }
        }
        else if (teamType == TeamType.Team2)
        {
            if (line == 1)
            {
                for (int i = 2; i >= 0; i--)
                {
                    if (inBattleManager.Instance.Line1[i] != null)
                    {
                        var aim = inBattleManager.Instance.Line1[i];
                        if (!aim.GetComponent<HealthMgr>().isDead)
                        {
                            return aim;
                        }
                    }
                }
            }
            else if (line == 2)
            {
                for (int i = 2; i >= 0; i--)
                {
                    if (inBattleManager.Instance.Line2[i] != null)
                    {
                        var aim = inBattleManager.Instance.Line2[i];
                        if (!aim.GetComponent<HealthMgr>().isDead)
                        {
                            return aim;
                        }
                    }
                }
            }
        }
        return null;
    }

    private void OnEnable()
    {
        Reset();
        becomeDead= false;
        if(spriteRenderers.Count == 0)
        {
            GetChild(transform);
        }
        else
        {
            for(int i = 0;i < spriteRenderers.Count;i++)
            {
                var sr = spriteRenderers[i];
                var c = defaultColors[i];
                sr.color = c;
            }
        }
    }

    public virtual void Reset()
    {

        if (初始面向反转)
        {
            if (teamType == TeamType.Team2)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
        }
        else
        {
            if (teamType == TeamType.Team1)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
        }
    }

    void GetChild(Transform pt)
    {
        foreach (Transform t in pt)
        {
            if(t.GetComponent<SpriteRenderer>() != null)
            {
                spriteRenderers.Add(t.GetComponent<SpriteRenderer>());
                defaultColors.Add(t.GetComponent<SpriteRenderer>().color);
            }
            GetChild(t);
        }
    }
}
