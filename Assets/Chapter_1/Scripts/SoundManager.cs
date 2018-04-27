using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public float bpm = 0f;
    public float fourNoteTime = 0f;
    public float inverseFourNoteTime = 0f;
    public TimeManager timeManager;
    float playTime = 0f;
    public AudioClip clipBackgroundMusic;
    public AudioSource music;
    public AudioClip clipTimingChecker;
    public AudioSource timingChecker;
    public AudioClip clipAttackEffect;
    public AudioSource attackEffectSound;
    public AudioClip clipHitSound;
    public AudioSource audiosourceHit;
    public bool bIsMusicOn = false;
    float playCount = 0f;
    public float syncRate = 0f;
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
        attackEffectSound = GameObject.FindGameObjectWithTag("SoundAttack").GetComponent<AudioSource>();
        audiosourceHit = GameObject.FindGameObjectWithTag("SoundHit").GetComponent<AudioSource>();

        //AudioClip 넣기
        music.clip = clipBackgroundMusic;
        timingChecker.clip = clipTimingChecker;
        attackEffectSound.clip = clipAttackEffect;
        audiosourceHit.clip = clipHitSound;
        
        //TimeManager와의 처리
        timeManager = TimeManager.Instance;
        timeManager.syncRate = syncRate;
        timeManager.Init(bpm,fourNoteTime);
        timeManager.CalculateTotalNoteCount();
	}

    int count = 1;
    float intervalTime = 0f;
    void Update ()
    {
        intervalTime += Time.deltaTime;
        
        if (music.isPlaying)
            bIsMusicOn = true;

        if(bIsMusicOn)
        {
            if(timeManager.bIsTiming && playCount < timeManager.noteCount)
            {

                //PlayAudio(timingChecker, 1f);
                playCount++;
                Debug.Log("박자 간격 걸린 시간 :" + intervalTime.ToString());
                intervalTime = 0f;
            }
            //float timing = count * fourNoteTime * 0.5f;
            //playTime = music.time;

            //if (playTime >= timing)
            //{
            //    count++;
            //}

        }

	}

    public void PlayAudio(AudioSource audioSource, float volume = 1f)
    {
        audioSource.PlayOneShot(audioSource.clip,volume);
        //music.PlayOneShot(clipBackgroundMusic,0.5f);
        //timingChecker.PlayOneShot(clipTimingChecker, 1f);

    }
    void ChangeBpmToDelayTime(float bpm)
    {
        if (bpm > 0f)
        {
            fourNoteTime = 60 / bpm;
            inverseFourNoteTime = 1 / fourNoteTime;
        }

    }
}
