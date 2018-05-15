using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
        float time = 0f;
        bool bIsTimingRight = false;
    float musicBpm = 0f;
    float fourNoteTime = 0f;
    AudioSource musicAudioSource = null;
   public float noteCount = 0f;
    public bool bIsTiming = false;
    public float syncRate = 0f;
    public int totalNoteCount = 0;

    private void Start()
    {
        musicAudioSource = SoundManager.Instance.music;
    }
    void Update ()
    {
		if(SoundManager.Instance.bIsMusicOn)
        {
          float playTime = musicAudioSource.time + (syncRate * fourNoteTime *0.25f);
          float currentNote= playTime / fourNoteTime; //플레이타임 / 한박자의 시간 = 현재 음악의 진행위치가 몇박자 위치인지
          noteCount = Mathf.Floor(currentNote); //버림처리

            do
            {
                if (noteCount >= 1)
                {
                    float standardTiming = noteCount * fourNoteTime;
                    float marginTime = fourNoteTime * 0.25f; //16비트타임

                    if (standardTiming - (marginTime * 0.25f) > playTime) //어느정도의 오차 인정
                    {
                        bIsTiming = false;
                        break;
                    }

                    if (standardTiming + (marginTime * 0.25f) < playTime)
                    {
                        bIsTiming = false;
                        break;
                    }
                    bIsTiming = true;
                }
            } while (false);


        }
	} 

    public void Init(float bpm, float NoteTime)
    {
        musicBpm = bpm;
        fourNoteTime = NoteTime;
    }

    public void CalculateTotalNoteCount()
    {
       totalNoteCount = Mathf.FloorToInt(SoundManager.Instance.music.clip.length / SoundManager.Instance.fourNoteTime);

    }
}
