using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    //회전 속력 변수
    public float rotSpeed = 300.0f;

    //회전 누적 변수
    float mx = 0;


    void Update()
    {
        //1. 사용자의 마우스 입력을 받는다
        float mouse_X = Input.GetAxis("Mouse X");


        //2. 입력받은 값을 이용해서 회전 방향을 결정한다
        mx += mouse_X * rotSpeed * Time.deltaTime;

        //3. 결정된 회전 방향을 물체의 회전 속성에 대입한다
        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
