using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver_Quit : MonoBehaviour
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
        Application.Quit();



    }
}
