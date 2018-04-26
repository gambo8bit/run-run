using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public float bpm = 0f;
    public float fourNoteTime = 0f;
    public float eightNoteTime = 0f;
    public float sixteenNoteTime = 0f;
    float playTime = 0f;
    public AudioClip clipBackgroundMusic;
    public AudioClip clipTimingChecker;
    public AudioSource music;
    public AudioSource timingChecker;
    public bool bIsMusicOn = false;
	void Start ()
    {
        //Bpm -> 해당 bpm의 4분음표,8분음표,16분음표의 딜레이 타임 계산
        if (bpm > 0)
            ChangeBpmToDelayTime(bpm);
        else
            Debug.Log("Bpm이 입력되지 않았습니다.");
        
        //AudioSource 
        music = GetComponent<AudioSource>();
        timingChecker = GameObject.FindGameObjectWithTag("SoundTiming").GetComponent<AudioSource>();

        //AudioClip 넣기
        music.clip = clipBackgroundMusic;
        timingChecker.clip = clipTimingChecker;
        
	}

    int count = 1;
    void Update ()
    {
        if (music.isPlaying)
            bIsMusicOn = true;
        

        if(bIsMusicOn)
        {

            float timing = count * eightNoteTime;
            playTime = music.time;

            if (playTime >= timing)
            {
                count++;
                timingChecker.PlayOneShot(clipTimingChecker, 1f);
            }

        }

	}

    public void PlayBackgroundMusic()
    {
       
        music.PlayOneShot(clipBackgroundMusic,0.5f);
        timingChecker.PlayOneShot(clipTimingChecker, 1f);

    }
    void ChangeBpmToDelayTime(float bpm)
    {
        if (bpm != 0f)
        {
            fourNoteTime = 60 / bpm;
            eightNoteTime = fourNoteTime * 0.5f;
            sixteenNoteTime = eightNoteTime * 0.5f;
        }

    }
}
