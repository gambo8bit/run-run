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

    //UI 프리팹을 담을 딕셔너리
    Dictionary<eUIType, GameObject> DicUI = new Dictionary<eUIType, GameObject>();

    //자주 사용할 UI용(즉 DontDestroy 상태로 한번 생성후 삭제하지않고 반복사용할 UI)
    Dictionary<eUIType, GameObject> DicSubUI = new Dictionary<eUIType, GameObject>();

    GameObject GetUI(eUIType uIType, bool isDontDestroy = false)
    {
        GameObject makeUI = null;




        return makeUI;
    }
}
