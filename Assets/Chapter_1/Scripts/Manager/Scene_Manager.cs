using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoSingleton<Scene_Manager>
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

    eSceneType currentScene = eSceneType.Scene_Title;
    eSceneType NextScene = eSceneType.Scene_None;


    float loadTimer = 0f;
    const float MIN_loadingTime = 2f; //로딩최소시간
    const float MAX_loadingBar = 0.7f;//Scene로드완료시 로딩바 차있는정도(0 ~ 1)

    //다음 씬을 로드하기위한 처리 진행
    public void LoadScene(eSceneType type, bool isAsync = true)
    {
        if (currentScene == type) //로드하려는 Scene타입이 현재로드된 Scene타입이면 진행 X
            return;

        NextScene = type;
        bIsAsync = isAsync;
    }


    //NextState가 변경되어 None이 아닌 상태라면 NextState(type)인 Scene 로드 처리
    private void Update()
    {
        if (operation != null)
        {

            loadTimer += Time.deltaTime; //최소 로딩시간을 위해 로드되는 시점부터 타이머시작

            float minusLoadingBar = 1f - MAX_loadingBar;
            UIManager.Instance.ShowLoadingUI(operation.progress - minusLoadingBar); //로딩바 불러오기(로드완료되었을때 로딩바는 70%만 참)

            //SceneManager.LoadSceneAsync()보다 먼저 처리 안해주면 Scene 무한로드 됨(로드 ->로드후 다시로드 ->로드 이렇게 반복하는듯)
            if (operation.isDone == true) //Scene 로드 완료시
            {

                if (loadTimer >= MIN_loadingTime) //로딩이 너무빨리 되서 최소 delay시간을 정해 제한
                {

                    currentScene = NextScene;
                    CompleteLoad(currentScene);

                    NextScene = eSceneType.Scene_None;
                    operation = null;
                    UIManager.Instance.HideUI(UIManager.eUIType.UI_Loading, true);
                }
                else
                {
                    //로드가 완료되었지만 최소로딩시간에 도달하지않았다면 그에맞춰서 로딩바 최신화
                    UIManager.Instance.ShowLoadingUI(MAX_loadingBar + ((minusLoadingBar / MIN_loadingTime) * loadTimer));
                }

            }

        }



        if (NextScene != eSceneType.Scene_None && currentScene != NextScene)
        {

            DisableCurrentScene(currentScene); //Scene이 바뀌기전 현재Scene에서의 처리(Scene이 바껴도 유지되는 오브젝트들의 처리)

            if (bIsAsync) //비동기식 로드
            {
                loadTimer = 0f;
                UIManager.Instance.ShowLoadingUI(0f); //로딩바 로드

                operation = SceneManager.LoadSceneAsync(NextScene.ToString()); //Scene 비동기 로드
            }
            else //동기식 로드
            {
                SceneManager.LoadScene(NextScene.ToString());
                currentScene = NextScene;
                CompleteLoad(currentScene);
                NextScene = eSceneType.Scene_None;
            }


        }

    }


    void CompleteLoad(eSceneType type)
    {
        //로드완료후 추가적으로 해야할 프로세스
        switch (type)
        {
            case eSceneType.Scene_None:
                break;
            case eSceneType.Scene_Title:
                break;
            case eSceneType.Scene_Lobby:
                UIManager.Instance.ShowUI(UIManager.eUIType.UI_Lobby); //Lobby UI 활성화
                break;
            case eSceneType.Scene_Game:
                GameManager.Instance.GameInit();
                break;
        }
    }



    private void DisableCurrentScene(eSceneType currentType)
    {
        //DontDestroy같이 Scene이 바껴도 유지되는 오브젝트들의 처리
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
