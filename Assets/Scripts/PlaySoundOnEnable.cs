using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    public string SoudnName;
    // Start is called before the first frame update
    void OnEnable()
    {
        AudioManager.Instance.PlaySoundEffectsByName(SoudnName);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
