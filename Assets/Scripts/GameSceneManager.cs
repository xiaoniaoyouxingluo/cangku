using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// 游戏场景管理器 负责获取场上的部署位 对应部署位的灵居
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager instance;
    public static GameSceneManager Instance => instance;
    /// <summary>
    /// 我方部署位置
    /// </summary>
    public GameObject[,] Line1 = new GameObject[3, 3];
    /// <summary>
    /// 敌方部署位置
    /// </summary>
    public GameObject[,] Line2 = new GameObject[3, 3];
    private void Awake()
    {
        instance = this;
        //GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");//找到场上所有的部署位
        //玩家可放入槽位 wj;
        //for(int i = 0; i < maps.Length; i++) 
        //{
        //    wj = maps[i].GetComponent<玩家可放入槽位>();
        //    if(wj.transform.parent.name== "w玩家")
        //    {
        //        Line1[wj.pos.x, wj.pos.y] = maps[i];
        //    }
        //    else
        //    {
        //        Line2[wj.pos.x, wj.pos.y] = maps[i];
        //    }
        //}

    }
    private void Start()
    {

    }   
}
