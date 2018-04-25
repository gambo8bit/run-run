using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    //===========인스턴스 참조==========
    //플레이어
    public Ch1_Player player = null;

    //카메라
    public GameObject mainCamera = null;

    //GameManager
    public GameManager gameManager= null;

    //Enemy 프리팹
    public GameObject[] enemyPrefabs;

    //Group 하위의 각각의 적들의 EnemyScript들의 배열
    public Enemy[] enemyComponents;

    //======================================


    //충돌체 박스 크기(1변의 길이)
    public static float collisionSize = 2.0f;

    //그룹에 속하는 적의 수
    int enemyCount;

    //현재 적의 마리수 최댓값
    int enemyCountMax = 0;

    //플레이어가 공격실패하여 적과 충돌하였느냐?
    public bool isPlayerHit = false;


    //===========속도===================
    public static float SPEEDMIN = 2.0f; // 이동속도 최소값
    public static float SPEEDMAX = 10.0f; //이동속도 최대값
    public static float SPEEDLEAVE = 10.0f; //퇴장 속도
    public float currentMoveSpeed = SPEEDMIN; //현재 Enemy 그룹 전체의 이동속도

   public List<SignAnimation> signList;
    
    //=====================AI=================
    public enum eAIState
    {
        NONE = -1,
        NORMAL = 0, //보통
        DECELERATE, //감속
        LEAVE,      //화면에서 퇴장(플레이어 실패 후)
        MAX
    }

    public eAIState currentAIType = eAIState.NORMAL;
    
    //속도감속상태일때의 정보
    public struct Decelerate
    {
        public bool isActive; //감속 동작 중?
        public float beforeSpeed; //감속 전의 속도
        public float timer;
    }
    public Decelerate decelerate; //속도감속상태의 정보 구조체의 인스턴스

    public float bpmDelayInverse = 0f;

    public Vector3 spawnPos;

    public float hitAccuracy = 0f; //1에 근접할수록 가장 정확
    // Use this for initialization
    private void Awake()
    {
        
    }

    void Start ()
    {
        enemyPrefabs = GameManager.Instance.enemyPrefabs;
        //decelerate 초기화
        decelerate.isActive = false; //감속중 x
        decelerate.timer = 0.0f;

        bpmDelayInverse = 1f / SoundManager.Instance.fourNoteTime;

        signList = new List<SignAnimation>();
    }

    private void FixedUpdate()
    {
    }
    float timingTimer = 0f;
    float speed = 0.5f;

    IEnumerator CameraZoomEffect()
    {
        Vector3 orgPos = Camera.main.transform.position;
        Vector3 newPos = orgPos + new Vector3(0, 0, 10);
        float sixteenNoteInverse = 1f / SoundManager.Instance.sixteenNoteTime;
        float time = 0;

        //ZOOM
        while (true)
        {
            time += Time.fixedDeltaTime;
            float timing = time * sixteenNoteInverse;
            Camera.main.transform.position = Vector3.Lerp(orgPos,newPos,timing);

            if (timing >= 1)
            {
                time = 0f;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        //FADE OUT
        while (true)
        {
            time += Time.fixedDeltaTime;
            float timing = time * sixteenNoteInverse;
            Camera.main.transform.position = Vector3.Lerp(newPos, orgPos, timing);

            if (timing >= 1)
                break;
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    void Move()
    {
        ////이동 처리
        //Vector3 newPosition = this.transform.position;

        //newPosition.x -= currentMoveSpeed * Time.deltaTime;
        //this.transform.position = newPosition;
        if (currentAIType != eAIState.LEAVE)
        {
            timingTimer += Time.deltaTime * speed;
            hitAccuracy = bpmDelayInverse * timingTimer;
            Vector3 newPos = Vector3.Lerp(spawnPos, player.attackCol.transform.position,hitAccuracy);
            transform.position = newPos;
            if (hitAccuracy >= 1f)
            {
               StartCoroutine(CameraZoomEffect()) ; //zoom 이펙트
                currentAIType = eAIState.LEAVE;
                foreach(Enemy enemy in enemyComponents)
                {
                    enemy.Leave();
                }
                timingTimer = 0f;
            }
        }
        else
        {
            transform.position += new Vector3(currentMoveSpeed * Time.deltaTime,0,0);
        }
    }
    void Update ()
    {
        SpeedControl();

        Move();

        //퇴장모드일때 화면 안인지 밖인지 체크해서 밖이면 삭제처리
        if(currentAIType == eAIState.LEAVE)
        {
            bool isVisible = false;

            //현재 enemy들의 렌더링 여부 검사
            foreach(Enemy enemy in enemyComponents)
            {
                if(enemy.GetComponentInChildren<Renderer>().isVisible) //Rendering 하고 있는가
                {
                    //하나의 enemy 라도 현재 화면에 렌더링 중이라면 검사 종료
                    isVisible = true;
                    break;
                }
            }

            if (!isVisible) //enemy 모두가 렌더링 중이 아니라면
                Destroy(this.gameObject);
        }

	}

    //AI타입에 따른 적의 스피드 컨트롤
    void SpeedControl()
    {
        switch (this.currentAIType)
        {
            case eAIState.NONE:
                break;
            case eAIState.NORMAL:
                break;
            case eAIState.DECELERATE:
                {
                    //감속 로직을 돌릴 지점(플레이어와 enemy그룹간의 거리차이)
                    float decelerateStart = 8.0f;


                    if(decelerate.isActive) //감속모드 활성화상태이면
                    {
                        

                        // 1. 가속하여 도망간다.
                        // 2. 플레이어와 같은 속도를 잠시 유지
                        // 3. "역시 무리다~"라는 느낌으로 급 감속

                        //===========변수 선언================
                        float rate;

                        //페이즈마다의 페이즈 실행 시간
                        const float phase1Time = 0.7f;
                        const float phase2Time = 0.4f;
                        const float phase3Time = 2.0f;

                        //감속모드에서 쓸 최소,최대 속도값
                        const float DEC_speedMax = 30.0f;
                        float DEC_speedMin = SPEEDMIN;

                        float timerTime = decelerate.timer;

                        //============처 리==================
                        //한번은 do 이하 무조건 한번은 실행(but break만나시 루프 탈출)
                        do
                        {

                            // 페이즈 1. 가속하여 도망
                            if(timerTime < phase1Time)
                            {
                                //현재 타이머 시간이 1페이즈지정시간까지의 도달률이 어느정도인지 구함(0~1)
                                rate = Mathf.Clamp01(timerTime / phase1Time);

                                currentMoveSpeed = Mathf.Lerp(decelerate.beforeSpeed, DEC_speedMax, rate);

                                break;
                            }
                            //타이머타임이 페이즈1타임보다 커져 if문을 안 거치면 다음 페이즈에서의 시간비교를 위해 
                            //넘어간 페이즈시간길이만큼 타이머시간에 빼준다
                            timerTime -= phase1Time; 

                            //페이즈 2. 플레이어와 같은 속도가 될 때까지 감속
                            if(timerTime < phase2Time)
                            {
                                rate = Mathf.Clamp01(timerTime / phase2Time);

                                currentMoveSpeed = Mathf.Lerp(DEC_speedMax, Ch1_Player.maxSpeed, rate);

                                break;
                            }
                            timerTime -= phase2Time;

                            //페이즈 3. 속도 낮춰서 플레이어와의 거리 좁아짐
                            if(timerTime < phase3Time)
                            {
                                rate = Mathf.Clamp01(timerTime / phase3Time);

                                currentMoveSpeed = Mathf.Lerp(Ch1_Player.maxSpeed, DEC_speedMin, rate);
                            }
                            timerTime -= phase3Time;

                            
                                
                        }
                        while (false);

                        //감속모드 타이머에 시간 누적
                        decelerate.timer += Time.deltaTime;

                    }
                    else //최초 1회때의 감속모드 로직 실행할때
                    {
                       //플레이어와의 거리가 설정해둔 스타트 지점까지 되었을때 감속모드 로직 돌리기위해 거리차이값 구함
                        float distance = this.transform.position.x - this.player.transform.position.x;

                        if(distance < decelerateStart) //지정해둔 거리보다 가까워지면
                        {
                            decelerate.isActive = true; //감속모드 로직 플래그 on
                            decelerate.beforeSpeed = this.currentMoveSpeed; //감속모드 들어가기 직전의 스피드 저장
                            decelerate.timer = 0.0f; //로직을 위한 타이머 초기화
                        }
                    }

                }
                break;


            case eAIState.LEAVE:
                {
                    //플레이어에게 절때 추격당하지 않도록 플레이어 속도를 더한다.
                    currentMoveSpeed = SPEEDLEAVE ;

                }
                break;
            case eAIState.MAX:
                break;
        }
    }

    //EnemyGroup에 들어갈 각각의 Enemy들 생성
    public void CreateEnemy(int wantEnemyNum, Vector3 basePos)
    {
        enemyCount = wantEnemyNum; //그룹의 Enemy 카운트를 넘겨받은 인자로 지정
        enemyCountMax = Mathf.Max(enemyCountMax, enemyCount);

        enemyComponents = new Enemy[enemyCount];

        Vector3 enemyPosition;

        //Enemy 인스턴스 생성
        for(int i = 0; i < enemyCount; i++)
        {
            //enemyPrefab배열의 길이를 넘으면 안되므로 배열 길이와 나머지연산
            if (enemyPrefabs.Length == 0)
                Debug.Log("enemyPrefabs의 길이가 0입니다.");
            GameObject go = Instantiate(enemyPrefabs[i % enemyPrefabs.Length]) as GameObject;
            enemyComponents[i] = go.GetComponent<Enemy>();

            //적의 생성위치를 흩어지게 한다
            enemyPosition = basePos; //넘겨받은 기본 생성위치를 대입

            if(i == 0)
            {
                //첫번째 enemy는 basePos 값으로 생성
            }
            else
            {
                //난수값으로 도깨비 위치를 흩어지게 한다.

        //기본위치(basePos)의 X값은 는 Group의 콜리전박스의 제일 왼쪽 즉 z값과는 다르게 중앙에서 반반 값을 나눌 필요 x
                float splitRangeX = Enemy.collisionSize * (float)(enemyCount - 1);
                //Z값은 중앙에서 양쪽으로 반반(-,+) 나눌거기에 /2 해줌
                float splitRangeZ = Enemy.collisionSize * (float)(enemyCount - 1) / 2.0f;

                //흩어지는 범위가 Group의 콜리전 크기를 넘어가지 않게 제한 
                splitRangeX = Mathf.Min(splitRangeX, collisionSize);
                splitRangeZ = Mathf.Min(splitRangeZ, collisionSize / 2.0f);

                enemyPosition.x += Random.Range(0.0f, splitRangeX);
                enemyPosition.z += Random.Range(-splitRangeZ, splitRangeZ);
                enemyPosition.y = 0.0f;

            }
            //생성한 enemy Init
            enemyComponents[i].transform.position = enemyPosition;
            enemyComponents[i].transform.parent = this.transform;

            enemyComponents[i].player = this.player;
            enemyComponents[i].mainCamera = this.mainCamera;

            enemyComponents[i].waveAmplitude -= enemyComponents[i].transform.position.z; //좌우 wave 진폭


        }

    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
    //플레이어의 공격을 받았을 때
    public void HitByPlayer()
    {

        StartCoroutine(death());
        ////SceneControl에서 쓰러진 도깨비 수를 늘린다.

        ////원뿔모양 방향으로 도깨비가 날아가는 방향을 정한다.
        ////평가 계산값이 클수록 원뿔의 모양이 넓어져 넓은 범위에 도깨비가 흩어지게 된다.
        ////플레이어의 속도가 빠르면 원뿔의 모양이 매끄럽지 않게 된다.
        //Vector3 blowoutDir; //도깨비가 날아가는 방향(속도 벡터)
        //Vector3 blowoutUpVector; //blowoutDir의 수직 성분
        //Vector3 blowoutXZVector; //blowoutDir의 수평 성분

        //float angleY;
        //float blowoutSpeed;     //최종 날라가는 속도
        //float blowoutSpeedBase; //날라가는 속도값의 베이스 
        //float angleForwardBack; //원뿔의 전후 방향
        //float radiusBase;       //원뿔의 바닥면의 지름
    }

    void CrashOnPlayer()
    {

    }

    void BeginLeave()
    {

    }
}
