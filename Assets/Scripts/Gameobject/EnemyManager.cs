using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public List<GameObject> enemies = new List<GameObject>();
    int OnlyCode;


    private void Awake()
    {
        
        Instance = this;
    }

    [ContextMenu("测试开始运行")]
    public void EnemiesStartAct()
    {
        OnlyCode = 0;
        CallBack(OnlyCode);
    }

    public void CallBack(int OnlyCode)
    {
        if (enemies.Count > OnlyCode)
        {
            if (enemies[OnlyCode] == null)
            {
                CallBack(OnlyCode+1);
                return;
            }
            if (enemies[OnlyCode].GetComponent<HealthMgr>().isDead)
            {
                CallBack(OnlyCode+1);
                return;
            }
            enemies[OnlyCode].GetComponent<BasicAliveThing>().Act(OnlyCode);
        }
        else
        {
            inBattleManager.Instance.isActing = false;
        }
    }


}
