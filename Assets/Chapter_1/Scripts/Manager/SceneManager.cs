using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoSingleton<SceneManager>
{
    public enum eSceneType
    {
        Scene_None,
        Scene_Title,
        Scene_Lobby,
        Scene_Game
    }

    bool bIsAsync = true;  //씬을 로드하는 방식이 비동기방식인가
    AsyncOperation operation = null; //Scene 로드의 작업진행정도를 확인하기 위해

    eSceneType currentState = eSceneType.Scene_Title;
    eSceneType NextState = eSceneType.Scene_None;

    float loadDelayTime = 0f;

    //다음 씬을 로드하기위한 처리 진행
    public void LoadScene(eSceneType type, bool isAsync = true)
    {
        if (currentState == type) //로드하려는 Scene타입이 현재로드된 Scene타입이면 진행 X
            return;

        NextState = type;
        bIsAsync = isAsync;
    }

    //NextState가 변경되어 None이 아닌 상태라면 NextState type의 Scene 로드 처리

    private void Update()
    {
        if(NextState != eSceneType.Scene_None && currentState != NextState)
        {
            DisableCurrentScene(currentState);
        }
    }

    private void DisableCurrentScene(eSceneType currentType)
    {
        switch (currentType)
        {
            case eSceneType.Scene_None:
                break;
            case eSceneType.Scene_Title:
                break;
            case eSceneType.Scene_Lobby:
                break;
            case eSceneType.Scene_Game:
                break;
        }
    }
}