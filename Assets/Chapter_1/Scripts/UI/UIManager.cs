using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public enum eUIType
    {
        UI_Title,
        UI_Lobby,
        UI_Stage,
    }

    //UI카메라
    Camera UICamera = null;
    
    
    //UI 프리팹을 담을 딕셔너리
    Dictionary<eUIType, GameObject> DicUI = new Dictionary<eUIType, GameObject>();

    //자주 사용할 UI용(즉 DontDestroy 상태로 한번 생성후 삭제하지않고 반복사용할 UI)
    Dictionary<eUIType, GameObject> DicSubUI = new Dictionary<eUIType, GameObject>();
    GameObject SubUIRoot = null; //DontDestroy UI 의 Root

    //시작시 UI카메라 참조
    private void Awake()
    {
        UICamera = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI"));
    }

     
    




    GameObject GetUI(eUIType uIType, bool isDontDestroy = false)
    {
        //1.Dictionary에 있는지 확인해보고 있으면 반환
        if(isDontDestroy == false)  
        {
            if (DicUI.ContainsKey(uIType) == true)
                return DicUI[uIType];
        }
        else     //Dont Destroy UI
        {
            if (DicSubUI.ContainsKey(uIType))
                return DicSubUI[uIType];
        }


        //2.Dictionary에 없어서 여기로 넘어왔다면 UI프리팹 인스턴스화 & Dic에 저장
        GameObject makeUI = null;
        GameObject prefabUI = Resources.Load("Prefabs/UI/" + uIType.ToString()) as GameObject; //복사할 UI 프리팹

        if(prefabUI != null) //prefabUI를 잘 불러왔다면
        {
            
            //DontDestroy 인지 아닌지에 따라 조금 다르게 처리

            if (isDontDestroy == false)
            {
                //UI프리팹 인스턴스화 (UICamera 게임오브젝트 하위에 생성)
                makeUI = NGUITools.AddChild(UICamera.gameObject, prefabUI);
                //Dictionary에 저장
                DicUI.Add(uIType, makeUI);
            }

            else //DontDestroy UI 처리
            {
                if(SubUIRoot == null)
                {
                    SubRootCreate(); //SubUIRoot 없으면 생성(DontDestroyUI는 SubUIRoot하위에 둘것이므로)
                }

                makeUI = NGUITools.AddChild(SubUIRoot, prefabUI);

                DicSubUI.Add(uIType, makeUI);
            }


        }
        else //prefabUI 로드 실패시
        {
            Debug.Log(uIType.ToString() + "를 불러오는데 실패했습니다.");
            
        }





        return makeUI;
    }

    //UI오브젝트에 접근하여 SetActive(true)로 보이게 함 
    public void ShowUI(eUIType uIType, bool isSub = false)
    {
        GameObject uiObject = GetUI(uIType, isSub);
        if(uiObject != null && uiObject.activeSelf == false) //uiObject가 null이 아니고 현재 활성화상태가 아니라면
        {
            uiObject.SetActive(true); //활성화시킴   
        }
    }

    //UI오브젝트에 접근하여 SetActive(false)로 감춤
    public void HideUI(eUIType uIType, bool isSub = false)
    {
        GameObject uiObject = GetUI(uIType, isSub);
        if (uiObject != null && uiObject.activeSelf == true) //uiObject가 null이 아니고 현재 활성화상태라면
        {
            uiObject.SetActive(false); //비활성화시킴   
        }
    }

    private void SubRootCreate()
    {
        //SubUIRoot 가 없다면 생성
        if(SubUIRoot == null)
        {
            GameObject subRoot = new GameObject();
            subRoot.transform.SetParent(this.transform);

            SubUIRoot = subRoot;
            SubUIRoot.layer = LayerMask.NameToLayer("UI"); //레이어 "UI" 로 설정
        }

        //SubUIRoot의 위치&크기를 UIRoot와 일치시킴(UIRoot와 다르면 UICamera에 의도치않게 나타남)
        UIRoot uiRoot = UICamera.GetComponentInParent<UIRoot>();
        SubUIRoot.transform.position = uiRoot.transform.position;
        SubUIRoot.transform.localScale = uiRoot.transform.localScale;
    }
}
