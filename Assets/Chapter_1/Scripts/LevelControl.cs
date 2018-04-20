
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelControl : MonoSingleton<LevelControl>
{
    //===============프리팹========================
    public GameObject enemyGroupPrefab;

    //===================스크립트 참조======================
    public Ch1_Player player = null;

    public GameManager gameManager = null;

    public GameObject mainCamera = null;

    //도깨비 발생위치(플레이어의 x좌표가 기준선을 넘으면 플레이어의 앞쪽에 도깨비 출현 시키도록)
    public float enemySpawnLine;

    //리스폰위치의 거리(플레이어 기준 얼마나 멀리 둘것이냐)
    private float spawnMargin = 15.0f;
    public const float SPAWN_MARGIN_MAX = 50.0f;
    public const float SPWAN_MARGIN_MIN = 20.0f;


    //그룹안의 도깨비의 수
    int enemyNumInGroup = 1;

    //Combo 
    int comboCount = 0;

    //Enemy AI TYPE
    public enum eGroupType
    {
        NONE = -1,
        SLOW = 0,   //달리는 속도가 느림
        DECELERATE, //가속 -> 감속
        PASSING,    //두 개의 그룹으로 추적
        RAPID,      //짧은 간격으로
        NORMAL,     //보통
        NUM
    };

    public eGroupType groupType = eGroupType.NORMAL;
    public eGroupType groupTypeNext = eGroupType.NORMAL;

    //특별패턴
    eGroupType eventType = eGroupType.NONE;
    int eventCount = 1;
    int normalCount = 5;
    float normalEnemySpeed = EnemyGroup.SPEEDMIN * 5.0f;

    

    
    



    

    
    //초기화
    public new void  Init()
    {
        //게임 첫 시작시 적이 출현할수 있도록 플레이어 약간 뒤를 리스폰지점으로 초기화
        this.enemySpawnLine = this.player.transform.position.x - 1.0f;
    }

    bool bCanCreateEnemy = false;
    float startSpawnLine = 0f;
    float nextSpawnLine = 40f;

    public void CheckCreate()
    {
        if (!bCanCreateEnemy) 
        {

            //생성 타이밍 플래그가 거짓이면 노멀AI일때와 특별패턴일때를 구분해서 각각 조건에 맞으면 생성명령 플래그를 TRUE로 한다
            if (isNormalAI())
            {
                bCanCreateEnemy = true;
            }
            else
            {
                //NormalAI가 아닌 즉 특별패턴AI일 경우는 화면에서 EnemyGroup이 하나도 없을때까지 기다리고서 생성진행한다.
                if (GameObject.FindGameObjectsWithTag("EnemyGroup").Length == 0)
                    bCanCreateEnemy = true;
            }


            //Enemy발생할 준비가 되면, 플레이어가 지나가면 Enemy생성을 시작할 지점을 계산
            if (bCanCreateEnemy)
            {
                startSpawnLine = player.transform.position.x + nextSpawnLine;    
            }


            //플레이어가 startSpawnLine을 지나면 EnemyGroup 생성을 시작

            do
            {
                //이미 Enemy 최대제한 생성Count 만큼 생성했다면 실행 x
                if (gameManager.enemyGroupCount >= gameManager.enemyGroupCreateMax)
                    break;

                //생성가능상태가 아니면 실행 x
                if (!bCanCreateEnemy)
                    break;

                //생성시작지점 플레이어가 아직 지나가지않았다면 실행 x
                if (player.transform.position.x <= startSpawnLine)
                    break;

                groupType = groupTypeNext;

                switch (groupType)
                {
                    case eGroupType.SLOW:
                        ProcessGroupSpawn(groupType);
                        break;
                    case eGroupType.DECELERATE:
                        ProcessGroupSpawn(groupType);
                        break;
                    case eGroupType.PASSING:
                        ProcessGroupSpawn(groupType);
                        break;
                    case eGroupType.RAPID:
                        ProcessGroupSpawn(groupType);
                        break;
                    case eGroupType.NORMAL:
                        ProcessGroupSpawn(groupType,normalEnemySpeed);
                        break;
                }

                //Enemy 생성 관련 데이터 최신화
                enemyNumInGroup++; // 다음 생성때 Group안의 Enemy수들을 늘리기위해 Enemy수 데이터++
                enemyNumInGroup = Mathf.Min(enemyNumInGroup, gameManager.enemyCreateNumMax); //Enemy 수 최대 제한
                comboCount++; //콤보수 ++
                gameManager.enemyGroupCount++; //enemyGroup 생성 수
                bCanCreateEnemy = false; //생성가능 플래그 false

                SelectNextGroup(); //Next Group Type 지정

            } while (false);






        }



    }

    private void SelectNextGroup()
    {
        //보통패턴과 특별패턴 의 전환시점체크

        if (eventType != eGroupType.NONE) //이전의 루프가 특별패턴 이벤트라면 여기 조건문에 들어와 다시 보통패턴 루프돌게 초기화
        {
            eventCount--;

            if (eventCount <= 0)
            {
                eventType = eGroupType.NONE;

                normalCount = Random.Range(3, 7);
            }

        }
        else //eventType이 NONE일때
        {
            normalCount--;  // normalCount가 0이 될때 특별패턴EnemyGroup을 다음 GroupType으로 지정

            if(normalCount <= 0)
            {
                eventType = (eGroupType)Random.Range(0, 3);

                switch (eventType)
                {
                    case eGroupType.SLOW:
                        eventCount = 1;
                        break;
                    case eGroupType.DECELERATE:
                        break;
                    case eGroupType.PASSING:
                        break;
                    case eGroupType.RAPID:
                        eventCount = Random.Range(2, 4);
                        break;
                }
            }
        }

        // 다음 그룹타입을 정하는 데 필요한 데이터 입력(NONE EVENT, EVENT 패턴 2개로 나눠서 입력)
        if(eventType == eGroupType.NONE)
        {
            // 보통 패턴의 EnemyGroup Type
            float rate;

            rate = (float)comboCount / 10.0f; //콤보수가 높을수록 rate도 높아짐

            rate = Mathf.Clamp01(rate);

            normalEnemySpeed = Mathf.Lerp(EnemyGroup.SPEEDMAX, EnemyGroup.SPEEDMIN, rate); //normal Type 의 속도(콤보시 점점 느려짐)

            nextSpawnLine = Mathf.Lerp(LevelControl.SPAWN_MARGIN_MAX, LevelControl.SPWAN_MARGIN_MIN, rate); //생성되는 간격(콤보시 점점 좁아짐)

            groupTypeNext = eGroupType.NORMAL;
        }
        else
        {
            groupTypeNext = eventType;
        }

    }

    private void ProcessGroupSpawn (eGroupType groupType, float speed = 0)
    {
        switch (groupType)
        {
            case eGroupType.NONE:
                break;
            case eGroupType.SLOW:
                break;
            case eGroupType.DECELERATE:
                break;
            case eGroupType.PASSING:
                break;
            case eGroupType.RAPID:
                break;
            case eGroupType.NORMAL:
                break;
            case eGroupType.NUM:
                break;
        }
    }

        public bool isNormalAI()
    {
        bool bIsNormalType;

        do
        {
            bIsNormalType = false;

            if (groupType == eGroupType.DECELERATE)
                break;

            if (groupType == eGroupType.PASSING)
                break;

            if (groupType == eGroupType.SLOW)
                break;

            bIsNormalType = true;

        } while (false);

        return bIsNormalType;

    }

	//적 그룹 생성
    void CreateEnemyGroup(Vector3 spawnPos, float speed,EnemyGroup.eAIState type)
    {
        //enemyGroup 인스턴스 생성
        GameObject go = GameObject.Instantiate(enemyGroupPrefab) as GameObject; 

        EnemyGroup newGroup = go.GetComponent<EnemyGroup>();

        //지면에 닿는 높이
        spawnPos.y += EnemyGroup.collisionSize / 2.0f; //콜리전 사이즈만큼 띄워야 콜리전과 지면이 맞닿음
   

        newGroup.transform.position = spawnPos;
        newGroup.gameManager = this.gameManager;
        newGroup.mainCamera = this.mainCamera;
        newGroup.player = this.player;
        newGroup.currentMoveSpeed = speed;
        newGroup.currentAIType = type;

        //그룹안의 각각의 Enemy들도 생성
        Vector3 baseSpawnPos = spawnPos;

        int wantEnemyNum = enemyNumInGroup;

        //콜리전박스 맨왼쪽 끝을 Enemy생성지점으로 함
        baseSpawnPos.x -= (EnemyGroup.collisionSize * 0.5f - Enemy.collisionSize * 0.5f);
        baseSpawnPos.y = Enemy.collisionSize * 0.5f;

        //Enemy생성을 방금 생성한 EnemyGroup에 명령
        newGroup.CreateEnemy(wantEnemyNum, baseSpawnPos);

    }



	
	// Update is called once per frame
	void Update ()
    {
		
	}

}