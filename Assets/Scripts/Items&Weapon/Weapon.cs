using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   public enum Type{ Gun,Grenade};
    public Type type;
    public int damage;
    public float rate;

    //총알,탄피관련 변수
    public Transform bulletPos;
    public GameObject bullet;

    public Transform targetPosition;

    public void Use()
    {
        if (type == Type.Gun) //무기가 총인 경우 코루틴 실행
        {
            StartCoroutine("Shot"); 
        }

    }

    IEnumerator Shot()
    {
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        //총알에 속도 적용하기
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;
    }
}
