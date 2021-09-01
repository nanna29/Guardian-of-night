using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending_restart : MonoBehaviour
{
    private AudioSource buttonClick;

    void Start()
    {
        buttonClick = GetComponent<AudioSource>();
        buttonClick.Stop();
    }
   

    public void Click()
    {


        buttonClick.Play();
        SceneManager.LoadScene("MainScene_01");



    }
}
