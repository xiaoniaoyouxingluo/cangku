using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 玩家可放入槽位 : MonoBehaviour
{
    public GameObject 此地物体;
    [Tooltip("行和列")]
    public Vector2Int pos;
    private void Update()
    {
        if(此地物体 != null)
        {
            此地物体.transform.localPosition = new Vector3(此地物体.transform.localPosition.x, 此地物体.transform.localPosition.y, +5);
        }
    }
    private void OnMouseDown()
    {
        
    }
}
