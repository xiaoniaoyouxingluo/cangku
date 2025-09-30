using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 发射一个子弹
/// </summary>
[CreateAssetMenu(fileName = "BuffFireBullet00", menuName = "我的文件/BuffModule/BuffFireBullet")]
public class BuffFireBullet : BaseBuffModule
{
    [Tooltip("发射的子弹预设体")]
    public GameObject bullet;
    public override void Apply(BuffObj buff, DamageInfo damageInfo = null, GameObject targetOrattacker = null)
    {
        CameraShake.Instance.TriggerShake(0.15f, 0.05f * 40);
        GameObject g = GameObject.Instantiate<GameObject>(bullet, buff.carrier.transform.position, Quaternion.identity);
        g.GetComponent<BulletState>().caster = buff.carrier;
        g.GetComponent<BulletState>().speed *= (int)buff.carrier.GetComponent<AgentObj>().teamType;
        g.transform.localScale = new Vector2(Mathf.Abs(g.transform.localScale.x) * (int)buff.carrier.GetComponent<AgentObj>().teamType, g.transform.localScale.y);
    }
}
