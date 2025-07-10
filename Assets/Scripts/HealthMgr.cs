using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMgr : MonoBehaviour
{
    public int Health;
    [HideInInspector]
    public int nowHealth;
    [HideInInspector]
    public bool isDead;
    public int Sheild;
    public bool isHurting;
    float hurtKeepTime = 1;
    float hurtTimer;

    private void OnEnable()
    {
        nowHealth = Health;
        isDead = false;
    }

    private void Update()
    {
        if(Time.time >= hurtTimer && isHurting)
        {
            isHurting = false;
        }
    }
    /// <summary>
    /// 对物体造成伤害
    /// </summary>
    /// <param name="value">伤害数值</param>
    /// <param name="sourceObj">伤害来源物体，也许会有东西能反伤</param>
    /// <param name="无视护甲">攻击时不会经过护甲直接打到本体</param>
    public void HealthLess(int value, GameObject sourceObj, bool 无视护甲 = false)
    {
        if (GetComponent<BuffManager>() == null)
        {
            gameObject.AddComponent<BuffManager>();
        }
        value = (int)(GetComponent<BuffManager>().hurtScale * (value + GetComponent<BuffManager>().hurtPlus));
        if(value < 0)
        {
            value = 0;
        }
        if (Sheild > 0 && !无视护甲)
        {
            if (Sheild >= value)
            {
                //格挡比伤害大则格挡直接减去数值，结束
                Sheild -= value;
                return;
            }
            else
            {
                //突破防御
                value -= Sheild;
                Sheild = 0;
            }
        }
        //本体受伤
        nowHealth -= value;
        isHurting = true;
        hurtTimer = Time.time + hurtKeepTime;
        if (nowHealth <= 0)
        {
            isDead = true;

        }
    }

    public void Recover(int value)
    {
        if (nowHealth + value <= Health)
        {
            nowHealth += value;
        }
        else
        {
            nowHealth = Health;

        }
        AudioManager.Instance.PlaySoundEffectsByName("Sfx_Battle_Buff_Heal");
        GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/heal_add_00000"),transform.position);
    }
}
