using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BuffData
{
    public string Name;
    public string Introduce;
    public float times;
    public bool canBeStacked = true;
    public bool lasting;
}
public class BuffManager : MonoBehaviour
{
    [Header("基础数值")]
    public float basicDamSc = 1f;
    public float basicShdSc = 1f;
    public float basicHutSc = 1f;
    [Header("变化数值")]
    public float damageScale = 1;


    public float sheildScale = 1;
    public float hurtScale = 1;
    public float damagePlus;
    public float hurtPlus;
    public float sheildPlus;
    public List<BuffData> data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (data.Count >= 0)
        {
            var damageSca = basicDamSc;
            var sheildSca = basicShdSc;
            var hurtSca = basicHutSc;
            var damagePl = 0f;
            var hurtPl = 0f;
            var sheildPl = 0f;
            foreach (var d in data)
            {
                //效果结束时：
                if (d.times > 999)
                {
                    d.times = 999;
                }
                if (d.times < -999)
                {
                    d.times = -999;
                }
                if (d.times <= 0 && !d.lasting)
                {
                    
                    print(name + " lose buff :" + d.Name);
                    data.Remove(d);
                    damageScale = basicDamSc;
                    sheildScale = basicShdSc;
                    hurtScale = basicHutSc;
                    break;
                }
                //-------------------------------------------
                //效果持续时
                else
                {
                    bool unDebuff = false;
                    if (d.Name == "脆弱" && !unDebuff)//脆弱
                    {
                        sheildSca *= 0.75f;
                    }
                    else if (d.Name == "虚弱" && !unDebuff)//虚弱
                    {
                        damageSca *= 0.75f;
                    }
                    else if (d.Name == "易伤" && !unDebuff)//易伤
                    {
                        hurtSca *= 1.5f;
                        d.Introduce = "受到的伤害增加50%";
                    }
                    else if (d.Name == "军衔")
                    {
                        damageSca *= 2;
                    }
                    else if (d.Name == "飞行")
                    {
                        hurtSca *= 0.5f;
                    }
                    if(d.Name == "力量")
                    {
                        damagePl += d.times;
                    }
                    if (d.Name.Length > 2)
                    {
                        if (d.Name.Substring(0, 2) == "敏捷")
                        {
                            int value = int.Parse(d.Name.Substring(2));
                            if (value > 0)
                            {
                                sheildPl += value;
                            }
                            else if (!unDebuff)
                            {
                                sheildPl += value;
                            }
                        }
                        if (d.Name.Substring(0, 2) == "装甲")
                        {
                            int value = int.Parse(d.Name.Substring(2));
                            if (value > 0)
                            {
                                hurtPl -= value;
                            }
                            else if (!unDebuff)
                            {
                                hurtPl -= value;
                            }
                        }
                        if (d.Name.Substring(0, 2) == "出血")
                        {
                            int value = int.Parse(d.Name.Substring(2));
                            if (value > 0)
                            {
                                if (!unDebuff)
                                {
                                    hurtPl += value;
                                }
                            }
                            else
                            {
                                hurtPl += value;
                            }
                        }
                    }

                    if (d.Name == "伤口" && !unDebuff)
                    {
                        hurtPl += d.times;
                    }
                    if(d.Name == "电风扇加成")
                    {
                        damagePlus += d.times;
                    }
                    if (d.Name == "低温" && !unDebuff)
                    {
                        hurtPl += d.times;
                    }
                    if (d.Name == "伤害翻倍")
                    {
                        damageSca *= 2;
                    }
                   
                }
            }

            if (inBattleManager.Instance != null)
            {
                if (inBattleManager.Instance.Line1.Contains(gameObject))
                {
                    foreach (var e in inBattleManager.Instance.Line1)
                    {
                        if(e == null)
                        {
                            continue;
                        }
                        if (e.GetComponent<AT_电风扇>() != null)
                        {
                            print(e.GetComponent<BasicAliveThing>().teamType);
                            print(GetComponent<BasicAliveThing>().teamType);
                            if (e.GetComponent<BasicAliveThing>().teamType.ToString() == GetComponent<BasicAliveThing>().teamType.ToString() && e != gameObject)
                            {
                                damagePl += 5;
                            }
                        }
                        if (e.GetComponent<AT_闹钟>() != null)
                        {
                            if (e.GetComponent<BasicAliveThing>().teamType == GetComponent<BasicAliveThing>().teamType && e != gameObject)
                            {
                                damageSca *= 1.5f;
                            }
                        }
                        if (e.GetComponent<AT_电池负>() != null)
                        {
                            if (e.GetComponent<BasicAliveThing>().teamType == GetComponent<BasicAliveThing>().teamType && e != gameObject)
                            {
                                damagePl += 25;
                            }
                        }
                    }
                }
                else if (inBattleManager.Instance.Line2.Contains(gameObject))
                {
                    foreach (var e in inBattleManager.Instance.Line2)
                    {
                        if (e == null)
                        {
                            continue;
                        }
                        if (e.GetComponent<AT_电风扇>() != null)
                        {
                            if (e.GetComponent<BasicAliveThing>().teamType == GetComponent<BasicAliveThing>().teamType && e != gameObject)
                            {
                                damagePl += 5;
                            }
                        }
                        if (e.GetComponent<AT_闹钟>() != null)
                        {
                            if (e.GetComponent<BasicAliveThing>().teamType == GetComponent<BasicAliveThing>().teamType && e != gameObject)
                            {
                                damageSca *= 1.5f;
                            }
                        }
                        if (e.GetComponent<AT_电池负>() != null)
                        {
                            if (e.GetComponent<BasicAliveThing>().teamType == GetComponent<BasicAliveThing>().teamType && e != gameObject)
                            {
                                damagePl += 25;
                            }
                        }
                    }
                }
            }
            damageScale = damageSca;
            hurtScale = hurtSca;
            sheildScale = sheildSca;
            damagePlus = damagePl;
            sheildPlus = sheildPl;
            hurtPlus = hurtPl;
        }
        else
        {
            damageScale = basicDamSc;
            sheildScale = basicShdSc;
            hurtScale = basicHutSc;
            damagePlus = 0;
            hurtPlus = 0;
            sheildPlus = 0;
        }
    }
    public bool hasBuff(string name)
    {
        foreach (var b in data)
        {
            if (b.Name == name)
            {
                return true;
            }
        }
        return false;
    }

    public bool hasBuff(string name, int firstLength)
    {
        foreach (var b in data)
        {
            if (b.Name.Length >= firstLength)
            {
                if (b.Name == name || b.Name.Substring(0, firstLength) == name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AddBuff(BuffData bd)
    {
        if (data.Count > 0)
        {
            foreach (var d in data) 
            {
                if (d.Name == bd.Name && d.canBeStacked && bd.canBeStacked)
                {
                    d.times += bd.times;
                    return;
                }
            }
        }

        BuffData newb = new BuffData();
        newb.Name = bd.Name;
        newb.Introduce = bd.Introduce;
        newb.times = bd.times;
        newb.canBeStacked = bd.canBeStacked;
        newb.lasting = bd.lasting;

        data.Add(newb);
    }
}
