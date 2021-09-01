using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ClearDirector_03 : MonoBehaviour
{
    private AudioSource buttonClick;

    void Start()
    {
        buttonClick = GetComponent<AudioSource>();
        buttonClick.Stop();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //화면을 누르면
        {
            buttonClick.Play();
            SceneManager.LoadScene("LOADING_STAGE01"); //다음 씬으로 이동
        }

    }
}
