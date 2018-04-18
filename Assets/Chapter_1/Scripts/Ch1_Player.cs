using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ch1_Player : MonoBehaviour
{
    
    //애니메이터
    public Animator playerAnimator = null;
    //공격 판정용 Collider
    AttackCollider attackCol = null;
    //게임매니저 참조
    public GameManager gameManager = null;
    
    //이동 속도
    public float moveSpeed = 0.1f;
    //이동 속도 최대 제한값
    public const float maxSpeed = 20.0f;

    //가속도
    public float addSpeed = 0.001f;

    //속도 감속치
    public const float decreaseSpeed = 20.0f;

    //공격 판정용 타이머 (attackTimer > 0.0f 이라면 공격 중
    public float attackTimer = 0.0f;

    //공격간의 딜레이를 측정하기위한 타이머 ( attackDelayTimer > 0.0f 라면 공격할 수 없다)
    public float attackDelayTimer = 0.0f;

    //공격 판정이 지속되는 시간
    public float attackTime = 0.3f;
    //공격 딜레이의 시간 
    public float attackDelayTime = 1.0f;
    
    public enum ePlayerAIState
    {
        NONE = -1,
        RUN = 0,
        STOP,
        HIT,
        MAX
    }

    public ePlayerAIState playerAI = ePlayerAIState.NONE;
    public ePlayerAIState nextAI = ePlayerAIState.NONE;
    
    bool bIsRunning = true;
    
    // Use this for initialization
	void Start ()
    {   
       playerAnimator = GetComponentInChildren<Animator>();
	}
	

    public float GetSpeedRate()
    {
       float speedRate = Mathf.InverseLerp(0f, maxSpeed, GetComponent<Rigidbody>().velocity.magnitude);
        return speedRate;
    }
	// Update is called once per frame
	void Update ()
    {
        playerAnimator.speed = moveSpeed * 5.0f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed += addSpeed;
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed += -addSpeed;
        }
		if(Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * moveSpeed;
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.position += -Vector3.right * moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.W))
            transform.position += Vector3.right * 10000;
	}

    public void StopRequest()
    {
        bIsRunning = false; 
    }

    public bool IsStopped()
    {
        bool bIsStopped = false;

        do
        {
            if (bIsRunning) //달리고 있다면 false 인 bIsStopped 반환
                break;

            if (moveSpeed > 0.0f) //속도가 0 보다 크다면 멈춘게 아니다
                break;

            bIsStopped = true; // 위의 두 조건 다 만족하지않으면 멈췄다고 할수있다
        } while (false);

        return (bIsStopped);
    }
}
