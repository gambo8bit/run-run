using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoSingleton<Map>
{
    float mapWidth;
    float floorWidth;
    FloorControl[] floors;
    public float speed = 5;
    int count = 0;

    Vector3 changePos;
    
    
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
