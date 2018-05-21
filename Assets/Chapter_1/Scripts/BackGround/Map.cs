using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoSingleton<Map>
{
    float mapWidth;
    float floorWidth;
    FloorControl[] floors;
    public float speed = 1;
    int count = 0;
    public bool bIsTitle = false;
    Vector3 changePos;
    float speedMax = 20f;
    
	// Use this for initialization
	void Start ()
    {
        floors = GetComponentsInChildren<FloorControl>();
        floorWidth = floors[0].floorWidth;
        mapWidth = floors[0].totalWidth;
        changePos = transform.position;
        changePos.x -= floorWidth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(speed < speedMax && !bIsTitle)
        speed += 1f * Time.deltaTime;

        Vector3 newPos = transform.position;
        newPos.x -= speed * Time.deltaTime;
        transform.position = newPos;


        if (count == floors.Length)
            count = 0;

        if(changePos.x >= transform.position.x)
        {
            floors[count].transform.localPosition += new Vector3(mapWidth,0,0);
            changePos.x -= floorWidth; 
            count++;
        }


	}
}
