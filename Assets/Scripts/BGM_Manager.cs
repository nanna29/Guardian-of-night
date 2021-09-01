using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
	public AudioSource BGM;
	private void Awake() 
	{
		DontDestroyOnLoad(this.BGM);
	}
}
