using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    static bool bShutdown = false;
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                if(bShutdown == false)
                {
                    T instance = GameObject.FindObjectOfType<T>() as T;
                    if(instance == null)
                    {
                        //현재 T가 인스턴스화 되어있지않다면 T를 가진 게임오브젝트를 생성 & T를 참조
                        instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    }
                    InstanceInit(instance); //방금 할당된 T의 인스턴스로 Init()

                    //Debug
                    Debug.Assert(_instance != null, typeof(T).ToString() + "싱글턴 생성 실패");
                }
            }

            return _instance;
        }
    }

    //생성된 인스턴스 저장
    private static void InstanceInit(Object instance)
    {
        _instance = instance as T;
        _instance.Init();
    }

    //Init(씬이 바뀌어도 삭제되지않고 오브젝트 유지) 기본 싱글톤 설정
    public void Init()
    {
        DontDestroyOnLoad(_instance);
    }

    //해당 인스턴스 파괴될때
    public void OnDestroy()
    {
        _instance = null;
    }

    //유니티 종료시
    private void OnApplicationQuit()
    {
        _instance = null;
        bShutdown = true;
    }
}
