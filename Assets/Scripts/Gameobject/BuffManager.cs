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
    [Header("������ֵ")]
    public float basicDamSc = 1f;
    public float basicShdSc = 1f;
    public float basicHutSc = 1f;
    [Header("�仯��ֵ")]
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
                //Ч������ʱ��
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
                //Ч������ʱ
                else
                {
                    bool unDebuff = false;
                    if (d.Name == "����" && !unDebuff)//����
                    {
                        sheildSca *= 0.75f;
                    }
                    else if (d.Name == "����" && !unDebuff)//����
                    {
                        damageSca *= 0.75f;
                    }
                    else if (d.Name == "����" && !unDebuff)//����
                    {
                        hurtSca *= 1.5f;
                        d.Introduce = "�ܵ����˺�����50%";
                    }
                    else if (d.Name == "����")
                    {
                        damageSca *= 2;
                    }
                    else if (d.Name == "����")
                    {
                        hurtSca *= 0.5f;
                    }
                    if(d.Name == "����")
                    {
                        damagePl += d.times;
                    }
                    if (d.Name.Length > 2)
                    {
                        if (d.Name.Substring(0, 2) == "����")
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
                        if (d.Name.Substring(0, 2) == "װ��")
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
                        if (d.Name.Substring(0, 2) == "��Ѫ")
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

                    if (d.Name == "�˿�" && !unDebuff)
                    {
                        hurtPl += d.times;
                    }
                    if(d.Name == "����ȼӳ�")
                    {
                        damagePlus += d.times;
                    }
                    if (d.Name == "����" && !unDebuff)
                    {
                        hurtPl += d.times;
                    }
                    if (d.Name == "�˺�����")
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
                        if (e.GetComponent<AT_�����>() != null)
                        {
                            print(e.GetComponent<BasicAliveThing>().teamType);
                            print(GetComponent<BasicAliveThing>().teamType);
                            if (e.GetComponent<BasicAliveThing>().teamType.ToString() == GetComponent<BasicAliveThing>().teamType.ToString() && e != gameObject)
                            {
                                damagePl += 5;
                            }
                        }
                        if (e.GetComponent<AT_����>() != null)
                        {
                            if (e.GetComponent<BasicAliveThing>().teamType == GetComponent<BasicAliveThing>().teamType && e != gameObject)
                            {
                                damageSca *= 1.5f;
                            }
                        }
                        if (e.GetComponent<AT_��ظ�>() != null)
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
                        if (e.GetComponent<AT_�����>() != null)
                        {
                            if (e.GetComponent<BasicAliveThing>().teamType == GetComponent<BasicAliveThing>().teamType && e != gameObject)
                            {
                                damagePl += 5;
                            }
                        }
                        if (e.GetComponent<AT_����>() != null)
                        {
                            if (e.GetComponent<BasicAliveThing>().teamType == GetComponent<BasicAliveThing>().teamType && e != gameObject)
                            {
                                damageSca *= 1.5f;
                            }
                        }
                        if (e.GetComponent<AT_��ظ�>() != null)
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
