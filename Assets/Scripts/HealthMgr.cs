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
    /// ����������˺�
    /// </summary>
    /// <param name="value">�˺���ֵ</param>
    /// <param name="sourceObj">�˺���Դ���壬Ҳ����ж����ܷ���</param>
    /// <param name="���ӻ���">����ʱ���ᾭ������ֱ�Ӵ򵽱���</param>
    public void HealthLess(int value, GameObject sourceObj, bool ���ӻ��� = false)
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
        if (Sheild > 0 && !���ӻ���)
        {
            if (Sheild >= value)
            {
                //�񵲱��˺������ֱ�Ӽ�ȥ��ֵ������
                Sheild -= value;
                return;
            }
            else
            {
                //ͻ�Ʒ���
                value -= Sheild;
                Sheild = 0;
            }
        }
        //��������
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
