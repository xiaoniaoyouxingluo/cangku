using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/// <summary>
/// 一次普通攻击，可以给目标加buff
/// </summary>
[CreateAssetMenu(fileName = "SkillPA", menuName = "我的文件/SkillData/SkillPA")]
public class SkillPA : BaseSkillModule
{
    [Tooltip("攻击目标类型")]
    public xunzhaolingjuのtiaojian enemyType = xunzhaolingjuのtiaojian.Normal;
    [Tooltip("伤害过后，给角色添加的buff")]
    public List<AddBuffInfo> addBuffs = new List<AddBuffInfo>();
    private GameObject 攻击目标;
    private Vector3 endPos;//结束坐标
    private Vector3 startPos;//开始坐标
    private float speed = 2f;//移动速度
    private float time = 0;
    private bool 攻击后 = false;
    AgentObj caster;
    public override void ApplyMove(AgentObj caster)
    {
        this.caster = caster;
        int line = 0;
        if(enemyType==xunzhaolingjuのtiaojian.Normal)
        {
            line = caster.transform.parent.GetComponent<玩家可放入槽位>().pos.y;
            攻击目标 = GameSceneManager.Instance.GetThisLineFirstEnemy((TeamType)((int)(caster.teamType) * -1), line);
            if(攻击目标 == null )
            {
                List<int> ints = new List<int>() { 0, 1, 2 };
                ints.Remove(line);
                line = ints[Random.Range(0, 2)];
                攻击目标 = GameSceneManager.Instance.GetThisLineFirstEnemy((TeamType)((int)(caster.teamType) * -1), line);
                if(攻击目标==null)
                {
                    ints.Remove(line);
                    攻击目标 = GameSceneManager.Instance.GetThisLineFirstEnemy((TeamType)((int)(caster.teamType) * -1), ints[0]);
                    line = ints[0];
                    if(攻击目标==null)
                    {
                        Res();
                        GameLevelMgr.Instance.下一个攻击();//找不到可以攻击的目标，跳过攻击
                        return;
                    }
                }
            }
        }
        startPos = caster.transform.position;
        endPos = GameSceneManager.Instance.攻击位[line].transform.position;
        MonoMgr.Instance.AddUpdateListener(MoveLerp);//前往攻击点
    }
    /// <summary>
    /// 直线插补
    /// </summary>
    private void MoveLerp()
    {
        time += Time.deltaTime;
        caster.transform.position = Vector3.Lerp(startPos, endPos, time * speed);
        if (time >= 1 && !攻击后)
        {
            time = 0;
            caster.animator.Play("Attack");//播放攻击动画
            if (BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[caster.Property.uID].atk_sound != string.Empty)//播放攻击音效
                MusicMgr.Instance.PlaySound("Sounds/" + BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[caster.Property.uID].atk_sound);
            MonoMgr.Instance.RemoveUpdateListener(MoveLerp);
        }
        else if (time >= 1 && 攻击后)
        {
            time = 0;
            MonoMgr.Instance.RemoveUpdateListener(MoveLerp);
            Res();
            GameLevelMgr.Instance.下一个攻击();
        }
    }
    public override void ApplySkill(AgentObj caster)
    {
        攻击后 = true;
        CameraShake.Instance.TriggerShake(0.2f, 0.3f);
        DamageInfo dinfo = new DamageInfo(caster.gameObject, 攻击目标, new Damage(caster.nowproperty.atk), new DamageInfoTag[] { DamageInfoTag.directDamage });
        dinfo.addBuffs = addBuffs;
        DamageManager.Instance.AddDamage(dinfo);
        //播放攻击命中特效
        GameObjectPool.Instance.CreateGameObject(Resources.Load<GameObject>("Prefabs/SE/" + BinaryDataMgr.Instance.GetTable<AgentInfoContainer>().dataDic[caster.Property.uID].atk_effect), 攻击目标.transform.position);
    }

    public override void ComeMove()
    {
        time = 0;
        endPos = startPos;
        startPos = caster.transform.position;
        MonoMgr.Instance.AddUpdateListener(MoveLerp);//返回
    }

    public override void Res()
    {
        攻击目标 = null;
        攻击后 = false;
    }
}
