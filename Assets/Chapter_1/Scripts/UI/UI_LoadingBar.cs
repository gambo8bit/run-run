using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingBar : MonoBehaviour
{
    UIProgressBar progressBar;

    private void Awake()
    {
        if (progressBar == null)
            progressBar = this.GetComponentInChildren<UIProgressBar>();
    }

    public void SetProgress(float progressRate)
    {
        if (progressRate >= 0)
            progressBar.value = progressRate;
    }

}
