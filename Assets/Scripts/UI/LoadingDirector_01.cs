using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingDirector_01 : MonoBehaviour
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
            SceneManager.LoadScene("GURU_STAGE1"); //다음 씬으로 이동
           
        

    }
}
