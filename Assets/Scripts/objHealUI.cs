using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 管理灵居显示血量，护盾控件
/// </summary>
public class objHealUI : MonoBehaviour
{
    AgentObj agentObj;
    [Tooltip("血量文本")]
    public Text healthText;
    [Tooltip("护盾背景")]
    public GameObject sheildGameObj;
    [Tooltip("护盾文本")]
    public Text sheildText;
    [Tooltip("血量背景红")]
    public Image Red;
    [Tooltip("血量背景橙")]
    public Image Orange;
    // Start is called before the first frame update
    void Start()
    {
        agentObj = transform.parent.GetComponent<AgentObj>();
        GetComponent<Canvas>().sortingOrder = 20;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = new Vector3((parentHM.GetComponent<BasicAliveThing>().初始面向反转 ? -1 : 1)*(parentHM.GetComponent<BasicAliveThing>().teamType == TeamType.Team1 ? 1 : -1) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        healthText.text = Mathf.RoundToInt(agentObj.nowproperty.nowHp).ToString();//显示当前血量，会四舍五入显示整数
        Red.fillAmount = agentObj.nowproperty.nowHp / agentObj.nowproperty.maxHp;//控制血条显示的比例
        if (agentObj.Property.shield > 0)
        {
            sheildGameObj.SetActive(true);
            sheildText.text = Mathf.RoundToInt(agentObj.Property.shield).ToString();//显示当前护盾值，会四舍五入显示整数
        }
        else
        {
            sheildGameObj.SetActive(false);
        }
        //if (!agentObj.isHurting)
        //{
        //    if (Orange.fillAmount > Red.fillAmount)
        //    {
        //        Orange.fillAmount -= 0.01f;
        //    }
        //    else
        //    {
        //        Orange.fillAmount = Red.fillAmount;
        //    }
        //}
    }
}
