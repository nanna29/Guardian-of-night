using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    //카메라가 쫓아다닐 위치 변수
    public Transform followPosition;
    void Update()
    {
        //나의 위치를 followPosition 위치와 일치시킨다.
        transform.position = followPosition.position;
    }
}
