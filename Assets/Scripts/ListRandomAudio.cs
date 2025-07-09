using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListRandomAudio : MonoBehaviour
{

    public List<AudioClip> clipList = new List<AudioClip>();
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().clip= clipList[Random.Range(0, clipList.Count)];
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
