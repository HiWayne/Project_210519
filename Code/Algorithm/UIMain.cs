using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBasic;

public class UIMain : MonoSingleton<UIMain>
{
    // UI�㼶������
    public Transform defaultLayer;
    public Transform topLayer;
    public Transform toolLayer;

    [Header("Ԥ����")]
    public StartMenu startMenuPf;

    // ʵ��
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