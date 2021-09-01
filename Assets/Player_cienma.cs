using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_cienma : MonoBehaviour
{
    public void ReceiveSignal()
    {
        SceneManager.LoadScene(11);
    }
}
