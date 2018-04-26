
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ch1_Player : MonoBehaviour
{
    //=========Components================
    //애니메이터
    public Animator playerAnimator = null;
    //공격 판정용 Collider
   public AttackCollider attackCol = null;
    //게임매니저 참조
    public GameManager gameManager = null;
    
    
    //==========Value====================
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
    public float attackDisableTimer = 0.0f;

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
        attackCol = GameObject.FindGameObjectWithTag("AttackCollider").GetComponent<AttackCollider>();
        attackCol.owner = this;
        nextAI = ePlayerAIState.RUN;
	}
	

    public float GetSpeedRate()
    {
       float speedRate = Mathf.InverseLerp(0f, maxSpeed, GetComponent<Rigidbody>().velocity.magnitude);
        return speedRate;
    }
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetMouseButtonDown(0))
        {
            attackCol.isAttackEnabled = true;
        }


        //배경의 이동속도와 플레이어 달리기 모션을 동기화
        playerAnimator.speed = Map.Instance.speed * 0.1f;

        if (playerAnimator.speed > 0.1f)
            playerAnimator.SetInteger("State", 1);

        //AI

        //다음 AI로 넘어가야 되는지 체크
        if (nextAI == ePlayerAIState.NONE)
        {
            switch (playerAI)
            {
                
                case ePlayerAIState.RUN:
                    {
                        if(!bIsRunning) //뛰고있는 상태가 아니면
                        {
                            nextAI = ePlayerAIState.STOP; //다음 AI 멈추기 처리
                        }
                    }
                    break;
                case ePlayerAIState.HIT:
                    break;
                
            }
        }

            //다음 AI로 넘어가기전 추가처리 해야되는 AI 추가처리 
            if (nextAI != ePlayerAIState.NONE)
        {
            switch (nextAI)
            {

                case ePlayerAIState.STOP:
                    {
                        //멈추는 애니메이션 처리

                    }
                    break;
                case ePlayerAIState.HIT:
                    {

                    }
                    break;
            }

            playerAI = nextAI;
            nextAI = ePlayerAIState.NONE;
        }




            //      //ProtoType
            //      playerAnimator.speed = moveSpeed * 5.0f;

            //      if (Input.GetKey(KeyCode.LeftShift))
            //      {
            //          moveSpeed += addSpeed;
            //      }

            //      if(Input.GetKey(KeyCode.LeftControl))
            //      {
            //          moveSpeed += -addSpeed;
            //      }
            //if(Input.GetKey(KeyCode.D))
            //      {
            //          this.transform.position += Vector3.right * moveSpeed;
            //      }

            //      if(Input.GetKey(KeyCode.A))
            //      {
            //          transform.position += -Vector3.right * moveSpeed;
            //      }

            //      if (Input.GetKeyDown(KeyCode.W))
            //          transform.position += Vector3.right * 10000;
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
    
    public void ResetAttackDisableTimer()
    {
        attackDisableTimer = 0.0f;
    }

        //bool IsAttackInput()
        //{

        //}
}
