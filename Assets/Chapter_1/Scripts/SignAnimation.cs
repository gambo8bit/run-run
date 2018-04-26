using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignAnimation : MonoBehaviour {

    Vector3 orgPos;
    float time = 0f;
    float sixteenNote = 0f;
    EnemyGroup enemyGroup;
    Color startColor;
    Color endColor;
    Renderer[] renders;
    float animationHeight = 0.5f;
   public static int signCount = 0;
   public float myNumber = 0;

    private void Awake()
    {
        signCount++;
        myNumber = signCount;
        
        enemyGroup = GetComponentInParent<EnemyGroup>();
        enemyGroup.sign = this;
        

        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        transform.localPosition = new Vector3(1.5f,1.5f,0);
        orgPos = transform.position;
    }
    void Start ()
    {
        //색깔
        startColor = new Color();
        endColor = new Color();
        startColor = Color.black;
        startColor.a = 0f;
        endColor = Color.red;
        endColor.a = 255f;
        renders = GetComponentsInChildren<Renderer>();

        //애니메이션


        sixteenNote = 1f/SoundManager.Instance.sixteenNoteTime;
    }

	void Update ()
    {
      


       
        


        float timing = enemyGroup.hitAccuracy;

        foreach(Renderer render in renders)
        {
            render.material.color = Color.Lerp(startColor, endColor, timing);
        }


        transform.position = Vector3.Lerp(orgPos, orgPos+ new Vector3(0,animationHeight,0), timing);

        if (timing >= 1f)
        {
            
            Destroy(this.gameObject);
        }
        
	}
    private void OnDestroy()
    {
        signCount--;
    }
}
