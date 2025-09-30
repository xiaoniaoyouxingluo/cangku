using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
/// <summary>
/// 范围加血或加盾
/// </summary>
[CreateAssetMenu(fileName = "BuffOverlapAddHporShield00", menuName = "我的文件/BuffModule/BuffOverlapAddHporShield")]
public class BuffOverlapAddHporShield : BaseBuffModule
{
    [Tooltip("范围的形状")]
    public E_Overlap overlap;
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
        List<GameObject> gs = GameSceneManager.Instance.Overlap(buff.carrier.GetComponent<AgentObj>().teamType, buff.carrier.transform.parent.GetComponent<玩家可放入槽位>().pos, overlap, isthis ? null : new List<GameObject>() { buff.carrier });
        for (int i = 0; i < gs.Count; i++)
        {
            if (Shield > 0)
            {
                gs[i].GetComponent<AgentObj>().Property.shield += Shield;//加护盾
                GameObjectPool.Instance.CreateGameObject(ShieldSE, gs[i].transform.position);//创建护盾特效
            }
            if (Hp > 0)
            {
                DamageManager.Instance.AddDamage(buff.carrier, gs[i], new Damage(-Hp), new DamageInfoTag[] { DamageInfoTag.directHeal });
                GameObjectPool.Instance.CreateGameObject(HpSE, gs[i].transform.position);
            }
        }
    }
}
