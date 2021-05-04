using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBasic;

public class UIMain : MonoSingleton<UIMain>
{
    // UI层级父对象
    public Transform defaultLayer;
    public Transform topLayer;
    public Transform toolLayer;

    [Header("预制体")]
    public StartMenu startMenuPf;

    // 实例
    StartMenu startMenuInst;

    public void EnterStartMenu()
    {
        startMenuInst = Instantiate(startMenuPf, defaultLayer);
    }

    public void LeaveStartMenu()
    {
        if (startMenuInst != null)
            Destroy(startMenuInst.gameObject);
    }
}