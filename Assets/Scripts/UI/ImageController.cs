using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    public int hasGrenades; //수류탄 개수
    public Text text_grenade; //수류탄 개수 텍스트

    public GameObject grenade_img;
    public GameObject gun_img;

    public GameObject use_Grenade; //수류탄 녹색(소유하고 있다는 표시)
    public GameObject use_Gun; //총 녹색(소유하고 있다는 표시)

    bool usingGrenade;
    bool usingGun;

    GameObject gameObject;

    public bool[] hasWeapons;

   
    void Start()
    {
        gameObject = GameObject.Find("Player");
    }

   
    void Update()
    {
        Player player = gameObject.GetComponent<Player>(); //Player스크립트에서 요소를 갖고 옴.
        hasGrenades = player.hasGrenade;
        hasWeapons = player.hasWeapons;
        usingGun=player.gun; //총을 사용하고 있는지
        usingGrenade = player.grenade;
    
        //총 사용여부에 따른 UI
        if (hasWeapons[0])
        {
            gun_img.SetActive(true);
            use_Gun.SetActive(false);
        }
        if (usingGun == true)
        {
            use_Gun.SetActive(true);
        }

        //수류탄 사용여부에 따른 UI
        if (hasWeapons[1])
        {
            grenade_img.SetActive(true);
            use_Grenade.SetActive(false);
        }
        if (usingGrenade == true)
        {
            use_Grenade.SetActive(true);
        }

        if (!hasWeapons[1])
        {
            grenade_img.SetActive(false);
        }

        text_grenade.text = hasGrenades+"";
    }
}
