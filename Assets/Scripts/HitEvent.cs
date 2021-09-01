using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    // 에너미_스테이지1 컴포넌트 변수
    public Enemy_stage1 eStage1;

    public void OnHit()
    {
        // 플레이어에게 데미지를 주는 함수를 실행한다.

        eStage1.HitEvent();
    }
}
