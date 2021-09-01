using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //회전 속력 변수
    public float rotSpeed = 300.0f;

    //회전 각도 제한
    public float rotLimit = 60.0f;

    //회전 누적 변수
    float mx = 0;
    float my = 0;

  

    void Update()
    {
        
 
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;
        my = Mathf.Clamp(my, -rotLimit, rotLimit);

        transform.eulerAngles = new Vector3(-my, mx, 0);


    }
}
