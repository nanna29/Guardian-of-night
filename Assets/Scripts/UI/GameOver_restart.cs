using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_restart : MonoBehaviour
{
    int sceneIndex;
    public GameObject num;

    private AudioSource buttonClick;
    

    void Start()
    {
        buttonClick = GetComponent<AudioSource>();
        buttonClick.Stop();
    }


    

    public void Click()
    {
        buttonClick.Play();
        Time.timeScale = 1.0f;

        if ((SceneIndex.Num) == 4)
        {
            SceneManager.LoadScene("GURU_STAGE1");
        }
        else if ((SceneIndex.Num) == 6)
        {
            SceneManager.LoadScene("GURU_STAGE02");
        }
        else if ((SceneIndex.Num) == 9)
        {
            SceneManager.LoadScene("GURU_STAGE03");
        }



    }
}
