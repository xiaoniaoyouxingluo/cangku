using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolunmByPlayerPref : MonoBehaviour
{
    public string key;


    private void Update()
    {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(key,1);
    }
}
