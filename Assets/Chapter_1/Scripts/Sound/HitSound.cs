using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    AudioSource hitAudioSource;
	// Use this for initialization
	void Start ()
    {
        hitAudioSource = GetComponent<AudioSource>();	
	}
    float timer = 0f;
    int count = 0;
	// Update is called once per frame
	void Update ()
    {
        
	   if( hitAudioSource.isPlaying)
        {
            timer += Time.deltaTime;

            if (count % 2 == 0)
            {
                if (timer >= SoundManager.Instance.fourNoteTime * 0.75f)
                {
                    timer = 0f;

                    hitAudioSource.Stop();
                    count++;
                }

            }
            else
            {
                if (timer >= SoundManager.Instance.fourNoteTime)
                {
                    timer = 0f;
                    //hitAudioSource.Stop();
                    count++;
                }
            }

        }
	}
}
