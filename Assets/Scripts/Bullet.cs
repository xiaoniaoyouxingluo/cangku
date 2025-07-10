using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BasicAliveThing parentObj;
    float damage;
    public float speed;
    public bool Œﬁ ”∑¿”˘;
    public bool ¥©Õ∏;
    public Vector2 ShakeC;//Time,Force
    public GameObject hitTX;
    public string hitSound;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BasicAliveThing>() != null)
        {
            if(collision.GetComponent<BasicAliveThing>().teamType != parentObj.teamType)
            {
                var d = (parentObj.GetComponent<BuffManager>().damageScale * (parentObj.Damage + parentObj.GetComponent<BuffManager>().damagePlus));
                collision.GetComponent<HealthMgr>().HealthLess((int)d, parentObj.gameObject, Œﬁ ”∑¿”˘);
                GameObjectPool.Instance.CreateGameObject(hitTX, transform.position);
                CameraShake.Instance.TriggerShake(ShakeC.x, ShakeC.y);
                AudioManager.Instance.PlaySoundEffectsByName(hitSound);
                if (!¥©Õ∏)
                {
                    GameObjectPool.Instance.AddObject(gameObject);
                }
            }
        }
    }


    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * Time.fixedDeltaTime, 0);
    }




}
