using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorControl : MonoBehaviour
{
    public float floorWidth;
    public static int modelNum = 3;
   public GameObject Player;
    void Start ()
    {
       
        //스케일 1 일때 좌표크기로 10 이니깐 스케일 크기 * 10 하면 맵 한섹션의 크기
        floorWidth = this.transform.lossyScale.x * 10;	
	}
	
	
	void Update ()
    {
        //맵의 전체 너비( 1 루프 크기)
        float totalWidth = floorWidth * modelNum;

        //현재 위치
        Vector3 thisFloorPos = this.transform.position;

        //캐릭터 위치
        Vector3 playerPos = Player.transform.position;

        //플레이어와 현재 맵의 x 좌표 차이
        float distX = playerPos.x - thisFloorPos.x;

        //플레이어와 현재 맵의 x 좌표차이가 맵 전체의 루프 단위로 봤을때 몇 루프인지 계산
        int loopNum = Mathf.RoundToInt(distX / totalWidth);

        if(playerPos.x > thisFloorPos.x + totalWidth * 0.5f)
        {
            this.transform.position = thisFloorPos + new Vector3(totalWidth * loopNum ,0,0);
        }
	}
}
