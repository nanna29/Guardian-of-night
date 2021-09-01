using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //아이템,무기 타입이랑 value값 지정할수 있게 함
    public enum Type { Heart, Ammo,Grenade,Weapon};
    public Type type;
    public int value;

    //아이템 제자리에서 회전하게 만드는 함수
    private void Update()
    {
        transform.Rotate(Vector3.up * 25 * Time.deltaTime);
    }
}
