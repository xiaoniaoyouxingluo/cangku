using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制血量加减
/// </summary>
[CreateAssetMenu(fileName = "BullethpJiaJian00", menuName = "我的文件/Bullet/BullethpJiaJian")]
public class BullethpJiaJian : BaseBulletModule
{
    [Tooltip("伤害/治疗")]
    public Damage damage;
    public override void Apply(GameObject bullet, GameObject target)
    {
        DamageInfo dinfo = new DamageInfo(bullet.GetComponent<BulletState>().caster, target, damage, new DamageInfoTag[] { DamageInfoTag.directDamage });
        DamageManager.Instance.AddDamage(dinfo);
    }
}
