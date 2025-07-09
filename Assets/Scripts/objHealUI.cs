using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class objHealUI : MonoBehaviour
{
    HealthMgr parentHM;
    public Text healthText;
    public GameObject sheildGameObj;
    public Text sheildText;
    public Image Red;
    public Image Orange;
    // Start is called before the first frame update
    void OnEnable()
    {
        parentHM = transform.parent.GetComponent<HealthMgr>();
        GetComponent<Canvas>().sortingOrder = 20;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3((parentHM.GetComponent<BasicAliveThing>().初始面向反转 ? -1 : 1)*(parentHM.GetComponent<BasicAliveThing>().teamType == TeamType.Team1 ? 1 : -1) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        healthText.text = parentHM.nowHealth.ToString();
        Red.fillAmount = (float)parentHM.nowHealth / (float)parentHM.Health;
        if (parentHM.Sheild > 0)
        {
            sheildGameObj.SetActive(true);
            sheildText.text = parentHM.Sheild.ToString();
        }
        else
        {
            sheildGameObj.SetActive(false);
        }
        if (!parentHM.isHurting)
        {
            if (Orange.fillAmount > Red.fillAmount)
            {
                Orange.fillAmount -= 0.01f;
            }
            else
            {
                Orange.fillAmount = Red.fillAmount;
            }
        }
    }
}
