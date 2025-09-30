using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 给队友加血或加盾
/// </summary>
[CreateAssetMenu(fileName = "BuffAddHporShield00", menuName = "我的文件/BuffModule/BuffAddHporShield")]
public class BuffAddHporShield : BaseBuffModule
{
    [Tooltip("目标选择方式")]
    public xunzhaolingjuのtiaojian enemyType;
    [Tooltip("增加的血量")]
    public float Hp;
    [Tooltip("加血特效")]
    public GameObject HpSE;
    [Tooltip("增加的护盾")]
    public float Shield;
    [Tooltip("加盾特效")]
    public GameObject ShieldSE;
    [Tooltip("是否包括自己")]
    public bool isthis;
    public override void Apply(BuffObj buff, DamageInfo damageInfo = null, GameObject targetOrattacker = null)
    {
        if (!damageInfo.tags.Contains(DamageInfoTag.directDamage))//只有直接伤害才可以
            return;
        GameObject g = GameSceneManager.Instance.GetThisEnemy(buff.carrier.GetComponent<AgentObj>().teamType, enemyType, isthis ? null : buff.carrier);
        if (g == null)
            return;
        if (Shield > 0)
        {
            g.GetComponent<AgentObj>().Property.shield += Shield;//加护盾
            GameObjectPool.Instance.CreateGameObject(ShieldSE, g.transform.position);//创建护盾特效
        }
        if (Hp > 0)
        {
            DamageManager.Instance.AddDamage(buff.carrier, g, new Damage(-Hp), new DamageInfoTag[] { DamageInfoTag.directHeal });
            GameObjectPool.Instance.CreateGameObject(HpSE, g.transform.position);
        }
    }
}
