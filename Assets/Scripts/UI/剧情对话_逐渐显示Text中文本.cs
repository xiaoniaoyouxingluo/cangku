using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 剧情对话_逐渐显示Text中文本 : MonoBehaviour
{
    public List<string> talkList;
    public float speed = 0.05f;
    public Text text;
    public Image enti;
    bool isShowing;
    int index;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (talkList.Count > 0 && !isShowing)
        {
            text.text = "";
            enti.gameObject.SetActive(true);
            index = 0;
            isShowing = true;
            StartCoroutine(show());
        }
        else if (talkList.Count == 0)
        {
            enti.gameObject.SetActive(false);
        }
    }
    public void AddText(string text)
    {
        talkList.Add(text);
    }

    IEnumerator show()
    {
        yield return new WaitForSeconds(speed);
        if (talkList[0].Length >= index)
        {
            text.text = talkList[0].Substring(0, index);
            index++;
            StartCoroutine(show());
        }
        else
        {
            yield return new WaitForSeconds(2);
            talkList.RemoveAt(0);
            index = 0;
            isShowing = false;
        }
    }
}
