using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 玩家可放入槽位 : MonoBehaviour
{
    public GameObject 此地物体;
    public int Line;
    public int Index;


    private void Update()
    {
        if(此地物体 != null)
        {
            此地物体.transform.localPosition = new Vector3(此地物体.transform.localPosition.x, 此地物体.transform.localPosition.y, +5);
        }
    }
    private void OnMouseDown()
    {
        if(!inBattleManager.Instance.isActing)
        {
            inBattleManager.Instance.Upload交换信息(this);
        }
    }
}
