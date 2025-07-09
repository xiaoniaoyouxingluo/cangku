using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aliveObjUi : MonoBehaviour
{
    public Text DamageText;
    BasicAliveThing parentBAT;
    public GameObject introduce;
    void OnEnable()
    {
        parentBAT = transform.parent.GetComponent<BasicAliveThing>();
        GetComponent<Canvas>().sortingOrder = 20;
    }

    private void FixedUpdate()
    {
        DamageText.text = (parentBAT.GetComponent<BuffManager>().damageScale * (parentBAT.Damage + parentBAT.GetComponent<BuffManager>().damagePlus)).ToString();
        transform.localScale = new Vector3((parentBAT.初始面向反转?-1:1)*(parentBAT.teamType == TeamType.Team1?1:-1) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    [ContextMenu("Test")]
    public void Show()
    {
        introduce.SetActive(true);
    }

    public void Hide()
    {
        introduce.SetActive(false);
    }

}
