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
    public GameObject bubbleSortPf;
    public GameObject selectSortPf;
    public GameObject quickSortPf;

    // 实例
    StartMenu startMenuInst;
    GameObject bubbleSortInst;
    GameObject selectSortInst;
    GameObject quickSortInst;

    SortType currentSortType;

    public void EnterStartMenu()
    {
        startMenuInst = Instantiate(startMenuPf, defaultLayer);
    }

    public void LeaveStartMenu()
    {
        if (startMenuInst != null)
            Destroy(startMenuInst.gameObject);
    }

    public void EnterSortPanel(SortType sortType)
    {
        this.currentSortType = sortType;

        switch (currentSortType)
        {
            case SortType.BubbleSort:
                EnterBubbleSort();
                break;
            case SortType.SelectSort:
                EnterSelectSort();
                break;
            case SortType.QuickSort:
                EnterQuickSort();
                break;
            default:
                break;
        }
    }

    public void LeaveCurrentSortPanel()
    {
        switch (currentSortType)
        {
            case SortType.BubbleSort:
                LeaveBubbleSort();
                break;
            case SortType.SelectSort:
                LeaveSelectSort();
                break;
            case SortType.QuickSort:
                LeaveQuickSort();
                break;
            default:
                break;
        }
    }

    void EnterBubbleSort()
    {
        bubbleSortInst = Instantiate(bubbleSortPf, defaultLayer);
    }

    void LeaveBubbleSort()
    {
        if (bubbleSortInst != null)
            Destroy(bubbleSortInst.gameObject);
    }

    void EnterSelectSort()
    {
        selectSortInst = Instantiate(selectSortPf, defaultLayer);
    }

    void LeaveSelectSort()
    {
        if (selectSortInst != null)
            Destroy(selectSortInst.gameObject);
    }

    void EnterQuickSort()
    {
        quickSortInst = Instantiate(quickSortPf, defaultLayer);
    }

    void LeaveQuickSort()
    {
        if (quickSortInst != null)
            Destroy(quickSortInst.gameObject);
    }
}