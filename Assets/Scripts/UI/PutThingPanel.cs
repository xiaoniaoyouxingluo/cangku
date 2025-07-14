using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutThingPanel : MonoBehaviour
{
    public Transform 主框架;
    public GameObject 槽位;
    public List<GameObject> 可用槽位 = new List<GameObject>();
    GameObject 选中物品;
    卡槽数据 data选中;
    void Update()
    {
        if (inBattleManager.Instance.isEnd)
        {
            return;
        }
        
        
        if (选中物品 != null)
        {
            
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, 1 << 8);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<玩家可放入槽位>()?.此地物体 == null)
            {
                选中物品.transform.position = hit.collider.transform.position;
                if (Input.GetMouseButtonDown(0))
                {
                  
                    hit.collider.gameObject.GetComponent<玩家可放入槽位>().此地物体 = 选中物品;
                    if (hit.collider.gameObject.GetComponent<玩家可放入槽位>().Line == 1)
                    {
                        inBattleManager.Instance.Line1[hit.collider.gameObject.GetComponent<玩家可放入槽位>().Index] = 选中物品;
                    }
                    if (hit.collider.gameObject.GetComponent<玩家可放入槽位>().Line == 2)
                    {
                        inBattleManager.Instance.Line2[hit.collider.gameObject.GetComponent<玩家可放入槽位>().Index] = 选中物品;
                    }
                    选中物品.transform.SetParent(hit.collider.gameObject.transform);
                    选中物品.transform.localScale = new Vector2(1, 1);
                    选中物品.transform.localPosition = Vector2.zero;
                    选中物品.GetComponent<BasicAliveThing>().teamType = TeamType.Team1;
                    选中物品.GetComponent<BasicAliveThing>().Reset();
                    GameDataMgr.Instance.nowAgentList.Remove(data选中.AgentInfo);
                    inBattleManager.Instance.GhostNum -= data选中.Cost;
                    选中物品 = null;
                    data选中 = null;
                    AudioManager.Instance.PlaySoundEffectsByName("UI_MoveChequers_Set");
                }
            }
            else
            {
                选中物品.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
            }
                
            if (Input.GetMouseButtonDown(1))
            {
                GameObjectPool.Instance.AddObject(选中物品);
                选中物品 = null;
                data选中 = null;
            }
        }
    }

    public void ClickBtn(卡槽数据 data)
    {
        if(data.Name != "空" && data.物体 != null && 选中物品 == null && data.Cost <= inBattleManager.Instance.GhostNum && !inBattleManager.Instance.isActing)
        {
            AudioManager.Instance.PlaySoundEffectsByName("UI_MoveChequers_Pick");
            选中物品 = GameObjectPool.Instance.CreateGameObject(data.物体, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            data选中 = data;
        }
    }
}
