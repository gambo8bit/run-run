using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    float time = 0f;
        bool bIsTimingRight = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(SoundManager.Instance.bIsMusicOn)
        {
          float playTime = SoundManager.Instance.music.time;
          


        }
	}
}
