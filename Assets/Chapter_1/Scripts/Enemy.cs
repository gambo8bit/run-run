using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //플레이어
    public Ch1_Player player = null;
    //카메라
    public GameObject mainCamera = null;
    //콜리전 크기(반지름)
    public const float collisionSize = 0.5f;

    //이동시 좌우로 움직이는 주기(Z축)
    public float speedWaveCycle = 1f;
    //이동시 좌우로 움직이는 폭(Z축)
    public float waveAmplitude = 0.5f;
    
    
    public void Leave()
    {
        GetComponentInChildren<Animator>().speed *= 5f;
    }
    void Start ()
    {
		
	}
	
	
	void Update () {
		
	}
}
