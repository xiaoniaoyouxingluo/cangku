using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 管理灵居显示攻击力控件
/// </summary>
public class aliveObjUi : MonoBehaviour
{
    [Tooltip("攻击力文本")]
    public Text DamageText;
    AgentObj agentObj;
    void OnEnable()
    {
        agentObj = transform.parent.GetComponent<AgentObj>();
        GetComponent<Canvas>().sortingOrder = 20;
    }

    private void Update()
    {
        DamageText.text = Mathf.RoundToInt(agentObj.nowproperty.atk).ToString();//四舍五入整数显示攻击力
    }
}
