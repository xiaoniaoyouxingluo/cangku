using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSE : MonoBehaviour
{
    public float triggerTime = -1;
    float triggerTimer;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (triggerTime != -1)
        {
            triggerTimer = Time.time + triggerTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= triggerTimer && triggerTime != -1)
        {
            Des();
        }
    }


    public void Des()
    {
        GameObjectPool.Instance.AddObject(gameObject);
    }
}
