using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_changeScd : MonoBehaviour
{

    public List<GameObject> scd = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void hide(int index)
    {
        scd[index].SetActive(false);
    }

    public void show(int index)
    {
        scd[index].SetActive(true);
    }

    public void ·µ»ØMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void loadScene(string sn)
    {
        SceneManager.LoadScene(sn);
    }
}
