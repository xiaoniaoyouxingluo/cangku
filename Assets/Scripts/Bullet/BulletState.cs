using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

///<summary>
///子弹的“状态”，用来管理当前应该怎么移动、应该怎么旋转、应该怎么播放动画的。
///是一个角色的总的“调控中心”。
///</summary>
public class BulletState : MonoBehaviour
{
    ///<summary>
    ///这是一颗怎样的子弹
    ///</summary>
    public BulletModel model;

    ///<summary>
    ///要发射子弹的这个人的gameObject
    ///</summary>
    [HideInInspector]
    public GameObject caster;
    /// <summary>
    /// 子弹的移动速度
    /// </summary>
    public float speed = 500f;
    ///<summary>
    ///子弹传入的参数，逻辑用的到的临时记录
    ///</summary>
    public Dictionary<string, object> param = new Dictionary<string, object>();
    ///<summary>
    ///还能命中几次
    ///</summary>
    private int hp = 1;

    private void Start()
    {
        hp = model.hitTimes;
        //子弹刚创建时，那么就要处理刚创建的事情
        model.onCreate?.Apply(gameObject, null);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AgentObj>() != null)
        {
            if ((collision.GetComponent<AgentObj>().teamType == caster.GetComponent<AgentObj>().teamType && model.hitAlly) || (collision.GetComponent<AgentObj>().teamType != caster.GetComponent<AgentObj>().teamType && model.hitFoe))
            {
                hp--;//命中了
                GameObjectPool.Instance.CreateGameObject(model.hitTX, transform.position);
                MusicMgr.Instance.PlaySound("Sounds/" + model.hitSound);
                model.onHit?.Apply(gameObject, collision.gameObject);
                if (hp <= 0)
                    Destroy(gameObject);
            }
        }
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * Time.fixedDeltaTime, 0);
    }
    private void OnDestroy()
    {
        model.onRemoved?.Apply(gameObject, null);
    }
}
