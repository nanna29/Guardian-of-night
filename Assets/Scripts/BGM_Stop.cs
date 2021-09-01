using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Stop : MonoBehaviour
{
    AudioSource Start_BGM;

    void Start()
    {
        Start_BGM = GameObject.Find("BGM_Manager").GetComponent<AudioSource>();
        AudioSource.Destroy(Start_BGM);
    }

}
