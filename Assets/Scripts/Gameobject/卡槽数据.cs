using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 卡槽数据 : MonoBehaviour
{
    [Tooltip("灵居名字")]
    public string Name;
    [Tooltip("费用")]
    public int Cost;
    [Tooltip("关联灵居预设体")]
    public GameObject 物体;
    [Tooltip("关联灵居数据")]
    public AgentInfo AgentInfo;
    [Tooltip("剩余再部署时间")]
    public float 再部署时间;
}
