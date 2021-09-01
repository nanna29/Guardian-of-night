using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect_stage3 : MonoBehaviour
{
    public GameObject explosion;
    public float explosionRadius;
    public int bombPower;

    //public GameObject boss;



    public GameObject boss;


    //충돌하면 폭발 파티클효과 형성
    //자신을 제거
    void OnCollisionEnter(Collision collision)
    {
       

        GameObject go = Instantiate(explosion);
        go.transform.position = transform.position;



        //자신의 범위에서 일정반경을 검색. 그안에 적들을 찾는다.
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);
        Collider[] enemies2 = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 9);
        Destroy(gameObject);

        //수류탄 데미지를 입힌다
        for (int i = 0; i < enemies.Length+1; i++)
        {
            if (i < enemies.Length)
            {
                Enemy_stage3 eFSM = enemies[i].transform.GetComponent<Enemy_stage3>();

                eFSM.HitEnemy(bombPower);
            }

            BOSS_stage3 eFSM2 = enemies2[i].transform.GetComponent<BOSS_stage3>();


            eFSM2.HitEnemy(bombPower);

        }


    }
}
