using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy_stage3 : MonoBehaviour
{
    // 상태 별로 에너미가 상황에 맞는 행동 실행
    // 1. 에너미의 상태
    // 2. 상태별 함수
    // 3. switch문을 통해서 상태를 체크, 상태별 함수를 실행

    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die

    }



    EnemyState enemyState;

    // 플레이어 게임 오브젝트
    GameObject player;

    // 감지 범위
    public float findDistance = 50.0f;

    // 나의 캐릭터 컨트롤러s
    CharacterController cc;

    // 이동 속도
    public float moveSpeed = 20.0f;

    // 공격 가능 범위
    public float attackDistance = 20.0f;

    // 현재 누적 시간 변수
    float currentTime = 0;

    // 공격 딜레이 시간
    public float attackDelayTime = 2.0f;

    // 공격력 변수
    public int attackPower = 10;

    // 초기 위치와 회전 저장용 변수
    Vector3 originPos;
    Quaternion originRot;

    // 이동 가능한 거리
    public float moveDistance = 20.0f;

    // 최대 몬스터체력 변수
    public int maxMonster = 20;

    // 현재 몬스터체력 변수
    int currentMonster;

    // 슬라이더 변수
    public Slider StressSlider;

    // 애니메이터 컴포넌트 변수
    Animator anim;

    // 네비게이션 메쉬 에이전트
    NavMeshAgent smith;

    bool gunDamage; //총 데미지를 받는 경우
    //총알 공격력
    public GameObject game;
    public int BulletPower;

    public bool attack; //공격여부

    public GameObject dust; //몬스터가 죽었을 때 나타나는 먼지 파티클
    public GameObject blood; //총에 맞았을 때 나타나는 피


    void Start()
    {
        // 초기 상태는 대기 상태(Idle)
        enemyState = EnemyState.Idle;

        // 플레이어 검색
        player = GameObject.Find("Player");

        // 캐릭터 컨트롤러 받아오기
        cc = GetComponent<CharacterController>();

        // 초기 위치와 회전 저장하기
        originPos = transform.position;
        originRot = transform.rotation;


        // 자식 오브젝트의 애니메이터 컴포넌트를 가져오기
        anim = GetComponentInChildren<Animator>();

        // 네브메쉬 에이전트 컴포넌트 가져오기
        smith = GetComponent<NavMeshAgent>();
        smith.speed = moveSpeed;
        smith.stoppingDistance = attackDistance;

        // 현재 체력 설정
        currentMonster = maxMonster;


    }

   
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            
            case EnemyState.Damaged:
                
                break;
            case EnemyState.Die:
                
                break;

        }
        HitEvent();
        // hp 슬라이더의 값에 체력 비율을 적용한다.
        StressSlider.value = (float)currentMonster / (float)maxMonster;

        Bullet b = game.GetComponent<Bullet>();
        BulletPower = b.damage;

        GunDamage();

    }

    void Idle()
    {
        // 만약, 플레이어와의 거리가 감지 범위 이내라면...
        if (Vector3.Distance(player.transform.position, transform.position) <= findDistance)
        {
            // 상태를 이동 상태로 변경한다.
            enemyState = EnemyState.Move;
            anim.SetTrigger("IdleToMove");
        }


    }
    void Move()
    {
        


        // 만약, 공격 범위 밖이라면...
        if (Vector3.Distance(player.transform.position, transform.position) > attackDistance)
        {
            


            // 내브메쉬 에이전트를 이용하여 타겟 방향으로 이동한다.
            smith.SetDestination(player.transform.position);
            smith.stoppingDistance = attackDistance;


        }
        // 공격 범위 안에 들어오면...
        else
        {
            // 상태를 공격 상태로 변경한다.
            enemyState = EnemyState.Attack;
           

            anim.SetTrigger("MoveToAttackDelay");


            // 공격 대기 시간을 미리 누적
            currentTime = attackDelayTime;

            // 이동을 멈추고, 타겟을 초기화한다.
            smith.isStopped = true;
            smith.ResetPath();
        }
    }
    void Attack()
    {
        // 만약, 플레이어가 공격 범위 이내라면...
        if (Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
        {
            // 만약, 현재 대기 시간이 공격 대기 시간을 넘어갔다면...
            if (currentTime >= attackDelayTime)
            {
                currentTime = 0;

                if (currentTime == 0)
                    attack = true;
                // 플레이어를 공격한다.
                
                anim.SetTrigger("StartAttack");

            }

            else
            {
                // 시간을 누적한다.
                attack = false;
                currentTime += Time.deltaTime;
            }

        }
        else
        {

            // 상태를 이동 상태로 전환한다.
            enemyState = EnemyState.Move;

            // attack = false;
            anim.SetTrigger("AttackToMove");



        }

    }

    // 플레이어에게 데미지를 주는 함수
    public void HitEvent()
    {
        if (attack == true) //에너미들이 공격 자세를 취한 경우
        {
           
            Player pm = player.GetComponent<Player>();
            pm.OnDamage(attackPower);
        }
    }


    // 복귀 시 행동 함수
    void Return()
    {
        // 만약, 원래 위치에 도달하지 않았다면, 그 방향으로 이동한다.
        if (Vector3.Distance(originPos, transform.position) > 0.1f)
        {
            

            smith.SetDestination(originPos);
            smith.stoppingDistance = 0;
        }

        // 원래 위치에 도달하면 대기 상태로 전환한다.
        else
        {
            smith.isStopped = true;
            smith.ResetPath();


            transform.position = originPos;
            transform.rotation = originRot;




            enemyState = EnemyState.Idle;
            anim.SetTrigger("MoveToIdle");

            // 체력을 최대치로 회복한다.
            currentMonster = maxMonster;
        }
    }


    // 피격 시 행동 함수
    void Damaged()
    {
        // 코루틴 함수를 실행한다.
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        // 2초간 정지한다.
        yield return new WaitForSeconds(1.0f);

        // 상태를 이동 상태로 전환한다.
        enemyState = EnemyState.Move;
       
    }

    // 사망 시 행동 함수
    void Die()
    {
        // 기존의 예약된 코루틴들을 모두 종료
        StopAllCoroutines();
        // 사망 코루틴을 시작한다.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 컨트롤러를 비활성화한다.
        cc.enabled = false;


        // 2초간 기다렸다가 몸체를 제거한다.
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);

    }

    // 데미지 처리 함수
    public void HitEnemy(int value)
    {
        // 만약, 나의 상태가 피격, 복귀, 사망 상태일 때에는 함수를 종료한다.
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Return || enemyState == EnemyState.Die)
        {
            return;
        }

        currentMonster -= value;


        // 만일, 남은 몬스터 체력이 0보다 크다면...
        if (currentMonster > 0)
        {
            // 상태를 피격 상태로 전환한다
            enemyState = EnemyState.Damaged;
           
            anim.SetTrigger("Damaged");
            Damaged();

        }
        // 그렇지 않다면,
        else
        {
            GameObject go = Instantiate(dust); //먼지 파티클(피격효과)
            go.transform.position = transform.position;

            // 상태를 사망 상태로 전환한다.
            enemyState = EnemyState.Die;
           
            anim.SetTrigger("Die");
            Die();

            Destroy(go, 4.0f);
        }

    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Weapon") //만약 몬스터와 닿은 물체가 총알이라면
        {
            gunDamage = true;
           

            GameObject go = Instantiate(blood); //피(피격효과)
            go.transform.position = collision.transform.position;
            Destroy(go, 1.0f);
        }


    }

    void GunDamage()
    {
        if (gunDamage == true)
        {
            HitEnemy(BulletPower); //몬스터 체력이 총알 공격력만큼 줄어든다
            gunDamage = false;
        }
    }
}
