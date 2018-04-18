﻿using System.Collections;
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

    //그룹안의 도깨비의 수
    int goalEnemyNum = 1;

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

    

    

    
    



    

    
    //초기화
    public new void  Init()
    {
        //게임 첫 시작시 적이 출현할수 있도록 플레이어 약간 뒤를 리스폰지점으로 초기화
        this.enemySpawnLine = this.player.transform.position.x - 1.0f;
    }


    public void CheckCreate()
    {

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

        int wantEnemyNum = goalEnemyNum;

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
