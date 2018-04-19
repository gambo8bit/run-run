using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{ 
    //===========프리팹==================
    public  GameObject enemyGroupPrefab;
    public GameObject[] enemyPrefabs;
    public GameObject playerPrefab;

    //=========== 인스턴스 참조===============
    public GameObject mainCamera; //카메라

    public LevelControl levelControl; //레벨컨트롤

    public Ch1_Player player;

    //========게임진행상태=============
    public enum STEP
    {
        NONE = -1,
        START,
        GAME,
        ENEMY_VANISH_WAIT,
        LAST_RUN,
        PLAYER_STOP_WAIT,
        GOAL,
        RESULT,
        GAME_OVER,
        GOTO_TITLE,
        MAX,
    };

    public STEP stepCurrent = STEP.NONE;    //현재상태
    public STEP stepNext = STEP.NONE;       //다음으로 넘어가는 상태

    //STEP 체크용 타이머
    public float stepTimer = 0.0f;
    public float stepTimerPrev = 0.0f;

    //=================그 외 수치=====================
    //GUI(START 표시) 유지 시간
    public const float startTime = 2.0f;

    //EnemyGroup의 최대 생성 수 (도달하면 게임이 종료)
    public int enemyGroupCreateMax = 50;

    //Group안의 Enemy의 갯수 최대
    public int enemyCreateNumMax = 10;

    //EnemyGroup의 출현수
    public int enemyGroupCount = 0;



    //공격한 or 부딪힌 EnemyGroup 의 수
    public int enemycompleteCount = 0; //생성수
    public int enemyKillCount = 0; //죽인수
    public int enemyMissCount = 0; //못잡은수


    void Start()
    {
        //프리팹 로드
        playerPrefab = Resources.Load("Player") as GameObject;
        enemyGroupPrefab = Resources.Load("Enemy/EnemyGroupBox") as GameObject;
        GameObject goEnemy0 = Resources.Load("Enemy/Enemy0") as GameObject;
        GameObject goEnemy1 = Resources.Load("Enemy/Enemy1") as GameObject;
        GameObject goEnemy2 = Resources.Load("Enemy/Enemy2") as GameObject;
        enemyPrefabs = new GameObject[3];
        enemyPrefabs[0] = goEnemy0;
        enemyPrefabs[1] = goEnemy1;
        enemyPrefabs[2] = goEnemy2;

        //스크립트 인스턴스화 및 링크
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Ch1_Player>();
        player.gameManager = this;
        mainCamera = Camera.main.gameObject;

        levelControl = LevelControl.Instance;
        levelControl.gameManager = this;
        levelControl.player = player;
        levelControl.enemyGroupPrefab = enemyGroupPrefab;
        levelControl.mainCamera = mainCamera;
        levelControl.Init();

        stepNext = STEP.START;
        
	}
	
	
	void Update ()
    {
        stepTimerPrev = stepTimer;
        stepTimer += Time.deltaTime;



        switch (stepCurrent)
        {

            case STEP.START:
                {
                    //Start(GUI)표시 띄우기 위한 지정시간이 지났으면 게임진행으로 넘어감
                    if (stepTimer > startTime)
                        stepNext = STEP.GAME;
                }
                break;

            case STEP.GAME:
                {
                    //적이 지정한 값만큼 생성됬는지 체크해서 게임을 종료단계로 진행시킴
                    if (enemycompleteCount >= enemyCreateNumMax)
                        stepNext = STEP.ENEMY_VANISH_WAIT; 
                }
                break;

            case STEP.ENEMY_VANISH_WAIT:
                {
                    //EnemyGroup이 전부 사라지기전까진 다음 진행으로 넘어가지 않음
                    if (GameObject.FindGameObjectsWithTag("EnemyGroup").Length > 0)
                        break;

                    //플레이어가 어느정도 속도가 붙을때까지 다음 진행으로 넘어가지않음
                    if (player.GetSpeedRate() < 0.5f)
                        break;

                    //위에 두 조건에 걸리지 않았다면 다음 진행으로 넘어감
                    stepNext = STEP.LAST_RUN;
                }
                break;

            case STEP.LAST_RUN:
                {
                    //종료 지점까지 마지막 달리기(해당 스텝이 시작된지 2초 경과후 다음 스텝으로 이동)

                    if(stepTimer > 2.0f)
                    {
                        stepNext = STEP.PLAYER_STOP_WAIT;
                    }
                }
                break;

            case STEP.PLAYER_STOP_WAIT:
                {
                    //플레이어 정지됬다면 목표 성공 연출 시작
                    if (player.IsStopped())
                    {
                        //게임 목표 성공 연출
                        stepNext = STEP.GOAL;
                    }
                }
                break;

            case STEP.GOAL:
                {
                    //temp
                    stepNext = STEP.GOTO_TITLE;
                }
                break;

            case STEP.RESULT:
                {

                }
                break;

            case STEP.GAME_OVER:
                {

                }
                break;

            case STEP.GOTO_TITLE:
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(""); //타이틀 씬으로 ㄱ
                }
                break;

        }


        if(stepNext != STEP.NONE)
        {
            switch (stepNext)
            {
                case STEP.START:
                    {
                        //GUI START표시
                    }
                    break;
                case STEP.GAME:
                    break;
                case STEP.ENEMY_VANISH_WAIT:
                    break;
                case STEP.LAST_RUN:
                    break;
                case STEP.PLAYER_STOP_WAIT:
                    {
                        //플레이어를 정지시킨다.
                        player.StopRequest();

                        //DumpEnemy GameObject 생성(Effect)
                    }
                    break;
                case STEP.GOAL:
                    {
                        //목표 지점 연출 시작

                        //Enemy가 화면 위에서 날아오듯 생성하는 Generator 생성
                    }
                    break;
                case STEP.RESULT:
                    break;
                case STEP.GAME_OVER:
                    break;
                case STEP.GOTO_TITLE:
                    break;
                case STEP.MAX:
                    break;
            }

            //StepNext가 NONE이 아닐때만 실행한다는거 유의(즉 StepCurrent 실행을 완료하여 stepNext를 넘겨받았을때만 타이머 리셋)
            stepCurrent = stepNext;
            stepNext = STEP.NONE;

            stepTimer = 0.0f;

        }

        //현재 진행상태가 게임상태면 계속하여 Enemy생성해도 되는지 체크 
        if (stepCurrent == STEP.GAME)
            levelControl.CheckCreate();







    }
}