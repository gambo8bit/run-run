using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoSingleton<SceneManager>
{
    public enum eSceneType
    {
        Scene_Title,
        Scene_Lobby,
        Scene_Game
    }

    bool bIsAsync = true;  //씬을 로드하는 방식이 비동기방식인가
    AsyncOperation operation = null; //Scene 로드의 작업진행정도를 확인하기 위해

}
