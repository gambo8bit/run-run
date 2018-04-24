using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public float bpm = 0f;
    public float fourNoteTime = 0f;
    public float eightNoteTime = 0f;
    public float sixteenNoteTime = 0f;
    public AudioClip backgroundMusic;
    public AudioSource musics;
	void Start ()
    {
        //Bpm -> 해당 bpm의 4분음표,8분음표,16분음표의 딜레이 타임 계산
        if (bpm > 0)
            ChangeBpmToDelayTime(bpm);
        else
            Debug.Log("Bpm이 입력되지 않았습니다.");

        musics = GetComponent<AudioSource>();
        if (backgroundMusic == null)
            Debug.Log("음악을 찾지못했습니다.");
        musics.clip = backgroundMusic;
        musics.Play();
	}
	
	
	void Update ()
    {
		
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
