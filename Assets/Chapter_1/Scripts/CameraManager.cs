using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    Vector3 orgPos;
    bool bIsReadyCoroutine = true;
	// Use this for initialization
	void Start ()
    {
        orgPos = transform.position;	
	}
    float timer = 0f;
	// Update is called once per frame
	void Update ()
    {
        if (TimeManager.Instance.bIsTiming)
        {
            if (bIsReadyCoroutine)
            {
                bIsReadyCoroutine = false;
            StartCoroutine(CameraZoomInEffect());
            }
        }
        else
        {
            if (bIsReadyCoroutine)
            {
                if (transform.position != orgPos)
                {
                    timer += 2f * Time.deltaTime * SoundManager.Instance.inverseFourNoteTime;
                    transform.position = Vector3.Lerp(transform.position, orgPos, timer);

                }
                else
                {
                    timer = 0f;
                }

            }

        }


    }

    IEnumerator CameraZoomInEffect()
    {
        
        Vector3 orgPos = Camera.main.transform.position;
        Vector3 newPos = orgPos + new Vector3(0, 0, 10);
        float sixteenNoteInverse = 1f / (SoundManager.Instance.fourNoteTime * 0.25f);
        float time = 0;

        //ZOOM
        while (true)
        {
            time += Time.fixedUnscaledDeltaTime;
            float timing = time * sixteenNoteInverse * 2f;
            Camera.main.transform.position = Vector3.Lerp(orgPos, newPos, timing);

            if (timing >= 1f)
            {
                time = 0f;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        //FADE OUT
        while (true)
        {
            time += Time.fixedUnscaledDeltaTime;
            float timing = time * sixteenNoteInverse * 2f;
            Camera.main.transform.position = Vector3.Lerp(newPos, orgPos, timing);

            if (timing >= 1f)
                break;
            yield return new WaitForEndOfFrame();
        }

        bIsReadyCoroutine = true;
        yield return null;
    }
}
