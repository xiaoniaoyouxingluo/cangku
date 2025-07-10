using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutThingPanel : MonoBehaviour
{
    public Transform �����;
    public GameObject ��λ;
    public List<GameObject> ���ò�λ = new List<GameObject>();
    GameObject ѡ����Ʒ;
    �������� dataѡ��;


    private void OnEnable()
    {
        foreach (var e in ���ò�λ)
        {
            e.SetActive(false);
        }
        for (int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if (���ò�λ.Count > i)
            {
                ���ò�λ[i].SetActive(true);
            }
            else
            {
                var nc = Instantiate(��λ);
                nc.transform.SetParent(�����);
                ���ò�λ.Add(nc);
                nc.SetActive(true);
            }
        }
    }
    void Update()
    {
        if (inBattleManager.Instance.isEnd)
        {
            return;
        }
        for (int i = 0; i < GameDataMgr.Instance.playerData.num; i++)
        {
            if (���ò�λ.Count > i)
            {
                if (���ò�λ[i] != null)
                {
                    if (GameDataMgr.Instance.nowAgentList.Count > i)
                    {
                        print(GameDataMgr.Instance.nowAgentList[i].name);
                        ���ò�λ[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/" + GameDataMgr.Instance.nowAgentList[i].name);
                        ���ò�λ[i].transform.GetChild(1).gameObject.SetActive(true);
                       
                        if (���ò�λ[i].GetComponent<��������>() == null)
                        {
                            ���ò�λ[i].AddComponent<��������>();
                        }
                        
                        ���ò�λ[i].GetComponent<��������>().Name = GameDataMgr.Instance.nowAgentList[i].name;
                        ���ò�λ[i].GetComponent<��������>().AgentInfo = GameDataMgr.Instance.nowAgentList[i];
                        //Prefabs\Enemies
                        ���ò�λ[i].GetComponent<��������>().���� = Resources.Load<GameObject>("Prefabs/Enemies/" + GameDataMgr.Instance.nowAgentList[i].name);
                        ���ò�λ[i].GetComponent<��������>().Cost = ���ò�λ[i].GetComponent<��������>().����.GetComponent<BasicAliveThing>().cost;
                        GameDataMgr.Instance.nowAgentList[i].energy = ���ò�λ[i].GetComponent<��������>().����.GetComponent<BasicAliveThing>().cost;
                        ���ò�λ[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = ���ò�λ[i].GetComponent<��������>().����.GetComponent<BasicAliveThing>().cost.ToString();
                    }
                    else
                    {
                        ���ò�λ[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI/EnemyImg/empty");
                        ���ò�λ[i].transform.GetChild(1).gameObject.SetActive(false);
                        ���ò�λ[i].GetComponent<��������>().Cost = -1;
                        ���ò�λ[i].GetComponent<��������>().Name = "��";
                        ���ò�λ[i].GetComponent<��������>().AgentInfo = null;
                        //Prefabs\Enemies
                        ���ò�λ[i].GetComponent<��������>().���� = null;
                    }




                }
            }

        }
        inBattleManager.Instance.���Խ��� = ѡ����Ʒ == null;
        if (ѡ����Ʒ != null)
        {
            
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, 1 << 8);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<��ҿɷ����λ>()?.�˵����� == null)
            {
                ѡ����Ʒ.transform.position = hit.collider.transform.position;
                if (Input.GetMouseButtonDown(0))
                {
                  
                    hit.collider.gameObject.GetComponent<��ҿɷ����λ>().�˵����� = ѡ����Ʒ;
                    if (hit.collider.gameObject.GetComponent<��ҿɷ����λ>().Line == 1)
                    {
                        inBattleManager.Instance.Line1[hit.collider.gameObject.GetComponent<��ҿɷ����λ>().Index] = ѡ����Ʒ;
                    }
                    if (hit.collider.gameObject.GetComponent<��ҿɷ����λ>().Line == 2)
                    {
                        inBattleManager.Instance.Line2[hit.collider.gameObject.GetComponent<��ҿɷ����λ>().Index] = ѡ����Ʒ;
                    }
                    ѡ����Ʒ.transform.SetParent(hit.collider.gameObject.transform);
                    ѡ����Ʒ.transform.localScale = new Vector2(1, 1);
                    ѡ����Ʒ.transform.localPosition = Vector2.zero;
                    ѡ����Ʒ.GetComponent<BasicAliveThing>().teamType = TeamType.Team1;
                    ѡ����Ʒ.GetComponent<BasicAliveThing>().Reset();
                    GameDataMgr.Instance.nowAgentList.Remove(dataѡ��.AgentInfo);
                    inBattleManager.Instance.GhostNum -= dataѡ��.Cost;
                    ѡ����Ʒ = null;
                    dataѡ�� = null;
                    AudioManager.Instance.PlaySoundEffectsByName("UI_MoveChequers_Set");
                }
            }
            else
            {
                ѡ����Ʒ.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
            }
                
            if (Input.GetMouseButtonDown(1))
            {
                GameObjectPool.Instance.AddObject(ѡ����Ʒ);
                ѡ����Ʒ = null;
                dataѡ�� = null;
            }
        }
    }

    public void ClickBtn(�������� data)
    {
        if(data.Name != "��" && data.���� != null && ѡ����Ʒ == null && data.Cost <= inBattleManager.Instance.GhostNum && !inBattleManager.Instance.isActing)
        {
            AudioManager.Instance.PlaySoundEffectsByName("UI_MoveChequers_Pick");
            ѡ����Ʒ = GameObjectPool.Instance.CreateGameObject(data.����, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            dataѡ�� = data;
        }
    }
}
