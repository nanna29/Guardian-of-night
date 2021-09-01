using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    

    public float speed;
    public float JumpPower;
    bool moving;

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool isJump;
    bool sDown1; //숫자 1를 눌렀을때
    bool sDown2; //숫자 2를 눌렀을때
    public bool gun=false;
    public bool grenade=false;

    //플레이어 무기 관련 배열
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public int hasGrenade; //수류탄 갯수
    bool hasG;
    public GameObject grenadeObj; //수류탄 오브젝트
    public float throwPower = 30.0f; //발사할 힘
    public Transform grenadePos; //발사위치

    //스트레스 변수, 최대 스트레스
    public int Stress;
    public int maxStress;

    //슬라이더 
    public Slider stressSlider;

    //중력 변수
    public float gravity = -20.0f;
    Vector3 moveVec;
    //수직 속도 변수
    float yVelocity = 0;

    Rigidbody rigid;
    Animator anim;

    GameObject nearWeapon; //트리거된 무기들을 저장하기 위한 변수
    Weapon equipWeapon; //기존에 장착된 무기를 저장하는 변수
    int equipWeaponIndex=-1;

    //조준선
    public GameObject crossHair; //일반
    public GameObject crossHair_Sniper; //스나이퍼
    bool crossHair_G; //일반
    bool crossHair_S; //스나이퍼
    bool isZoom=false;

    private Camera camera;
    CharacterController cc; //캐릭터 컨트롤러 변수

    //이펙트 UI 오브젝트
    public GameObject hitEffect;

    //소리관련 변수
    public AudioClip audioGun;
    //public AudioClip audioGrenade;
    public AudioClip audioItem;
    public AudioClip audioWeapon;
    public AudioClip audioHurt;

    AudioSource audioSource;

    
    void Awake()
    {
        
        
        // rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        camera = Camera.main;

        cc = GetComponent<CharacterController>();

        Stress = 0;
 

        this.audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "GUN":
                audioSource.clip = audioGun;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "WEAPON":
                audioSource.clip = audioWeapon;
                break;
            case "HURT":
                audioSource.clip = audioHurt;
                break;
        }
        audioSource.Play();

    }



    void FixedUpdate() //카메라 떨림을 방지하기 위한 Update함수
    {
        
        Move();
      

    }

    void Update()
    {
        
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        
        Swap();
        GetInput();
        Interaction_W();//무기 상호작용
      
        Attack();
        Cross(); //조준선 기능
        Grenadelimit();
        
        //슬라이더의 value를 스트레스 비율로 적용한다
        stressSlider.value = (float)Stress / (float)maxStress;

       
    }


    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        sDown1 = Input.GetButtonDown("sDown1");
        sDown2 = Input.GetButtonDown("sDown2");
    }

    //플레이어 움직이게 하는 함수
    void Move()
    {
        if (hAxis == -1 || hAxis == 1 || vAxis == -1 || vAxis == 1)
        {
            moving = true;
        }
        else
            moving = false;
       

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        moveVec = Camera.main.transform.TransformDirection(moveVec);

        //캐릭터의 수직속도(중력)을 적용한다
        yVelocity += gravity * Time.deltaTime;
        moveVec.y = yVelocity;

        if (wDown)
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else
            transform.position += moveVec * speed * Time.deltaTime;

        cc.Move(moveVec * speed * Time.deltaTime);
      
        anim.SetBool("isRun", moving);
       
    }
    
    
    void Swap() //무기 교체
    {
        // 무기를 갖고 있지 않는 상태에서 무기가 활성화되는 것을 방지
        if (sDown1 && (!hasWeapons[0]||equipWeaponIndex==0))
        {
            return;
        }
        if (sDown2 && (!hasWeapons[1]||equipWeaponIndex==1))
        {
            return;
        }

        int weaponIndex = -1;
        //숫자 1을 누르면 총, 숫자2를 누르면 수류탄
        if (sDown1)
        {
            gun = true; //총 모드 활성화
            grenade = false;
            weaponIndex = 0;
            crossHair_G = true;
            crossHair_S = false;
         
        }
        

        if (sDown2)
        {
            gun = false;
            grenade = true; //수류탄 모드 활성화
            weaponIndex = 1;
            crossHair_G = false;
            crossHair_S = false;
         
        }

        //1,2를 누르면 무기가 보이게 함.
        if (sDown1 || sDown2)
        {
            if (equipWeapon != null) //빈손이 아니라면 
            {
                equipWeapon.gameObject.SetActive(false); //전에 손에 쥐고 있었던 무기를 비활성화함
            }
           
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true); // 손에 쥐고 있는 무기를 활성화하여 보이게함.
           
        }

        if (hasGrenade == 0) //수류탄이 0개이면 못던지게 함.
        {
            hasWeapons[1] = false;
        }
        

    }


    void Grenadelimit() //수류탄 개수 제한 및 수류탄 공격
    {
        if (hasGrenade == 0)
        {
            return;
        }


        if (Input.GetMouseButtonDown(0) && (grenade == true)) //마우스 좌클릭과 동시에 플레이어한테 1개 이상의 수류탄이 있는 경우
        { 

            GameObject bomb = Instantiate(grenadeObj);

            bomb.transform.position = grenadePos.position;



            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);


            hasGrenade -= 1; //수류탄을 쓸때마다 개수를 줄임
            


        }
        

    }

    void Attack() // 총 공격
    {
        if (equipWeapon == null) //무기를 갖고 있는 경우에만 실행
            return;

        if (Input.GetMouseButtonDown(0)&&(gun==true)) //마우스 좌클릭과 동시에 손에 총이 있는 경우
        {
                equipWeapon.Use();

            anim.SetTrigger("doShot");
            PlaySound("GUN");
        }
        

    }

    void Cross() //조준선
    {
        if (Input.GetMouseButton(1) && (gun == true)) //총을 소유한채로 오른쪽 마우스를 꾹 누르면 스나이퍼 모드
        {
            crossHair_G = false;
            crossHair_S = true;
            isZoom = true;

        }
        if (Input.GetMouseButtonUp(1) && (gun == true)) //오른쪽 마우스를 떼면 다시 일반 모드
        {
            crossHair_G = true;
            crossHair_S = false;
            isZoom = false;

        }

        crossHair.SetActive(crossHair_G); //무기를 소유한채로 1번을 누를 경우에만 조준선 활성화
        crossHair_Sniper.SetActive(crossHair_S); //스나이퍼 조준선 활성화

        if (!isZoom)
        {
            Camera.main.fieldOfView = 60.0f; //줌 카메라 시야각도
        }
        else
        {
            Camera.main.fieldOfView = 15.0f; //일반모드 카메라 시야각도
        }


    }


    void Interaction_W() //닿은 무기들을 배열에 저장
    {
        if (nearWeapon != null)
        {
            if (nearWeapon.tag == "Weapon"|| nearWeapon.tag == "Grenade")
            {
                Item item = nearWeapon.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true; //해당 무기가 플레이어한테 갖고 있다고 설정

                Destroy(nearWeapon);
            }
        }
        
    }

   

    //플레이어가 아이템이라고 태그된 물체와 닿으면 일어나는 일
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                //아이템 스크립트에서 지정한 타입과 value 값에 따라 stress 수치가 떨어짐
                //stress 가 maxstress를 넘어서면 0으로 고정
                case Item.Type.Ammo:
                    Stress -= item.value;
                    if (Stress < 0)
                        Stress = 0;                   
                    break;
                case Item.Type.Heart:
                    Stress -= item.value;
                    if (Stress < 0)
                        Stress = 0;
                    break;
            }
            PlaySound("ITEM");
            //아이템은 닿으면 사라짐
            Destroy(other.gameObject);
        }

        if (other.tag == "Grenade")
        {
            hasGrenade++;
            PlaySound("WEAPON");
        }
    }

    void OnTriggerStay(Collider other) //무기 콜라이더에 닿으면(=무기 오브젝트 감지)
    {
        if (other.tag == "Weapon"|| other.tag == "Grenade")
        {   
            nearWeapon = other.gameObject;
            PlaySound("WEAPON");
        }

        
        

    }

    void OnTriggerExit(Collider other) //무기 콜라이더에서 빠져나오면
    {
        if (other.tag == "Weapon" || other.tag == "Grenade")
        {
            nearWeapon = null;
        }
       
    }

    //플레이어가 공격을 받았을 때
    public void OnDamage(int value)
    {
        Stress += value;
        if (Stress>100)
        {
            Stress = 100;
            
        }

        else
        {
            PlaySound("HURT");
            StartCoroutine(HitEffect());
        }
        IEnumerator HitEffect()
        {
            //1.이펙트를 켠다(활성화)
            hitEffect.SetActive(true);

            //2.0.3초를 기다린다
            yield return new WaitForSeconds(0.3f);

            //3.이펙트를 끈다(비활성화 시킨다)
            hitEffect.SetActive(false);

        }

    }

}
