using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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
    public int enemyGroupCreateNumMax = 50;

    //Group안의 Enemy의 갯수 최대
    public int enemyCreateNumMax = 10;

    //공격한 or 부딪힌 EnemyGroup 의 수
    public int enemycompleteCount = 0;
    public int enemyKillCount = 0;
    public int enemyMissCount = 0;


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
                    if (stepTimer > startTime)
                        stepNext = STEP.GAME;
                }
                break;

            case STEP.GAME:
                {
                    if (enemycompleteCount >= enemyCreateNumMax)
                        stepNext = STEP.ENEMY_VANISH_WAIT; 
                }
                break;

            case STEP.ENEMY_VANISH_WAIT:
                {
                    do
                    {

                    }while()
                }
                break;

            case STEP.LAST_RUN:
                {

                }
                break;

            case STEP.PLAYER_STOP_WAIT:
                {

                }
                break;

            case STEP.GOAL:
                {

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
                    break;
                case STEP.GOAL:
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

            //StepNext가 NONE이 아닐때만 실행한다는거 유의(즉 StepCurrent 실행을 완료해야 타이머 리셋)
            stepCurrent = stepNext;
            stepNext = STEP.NONE;

            stepTimer = 0.0f;

        }

        if (stepCurrent == STEP.GAME)
            levelControl.CheckCreate();







    }
}