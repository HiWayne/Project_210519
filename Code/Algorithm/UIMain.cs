using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBasic;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    const string ErrorText = "错误个数：";

    // UI层级父对象
    public Transform defaultLayer;
    public Transform topLayer;
    public Transform toolLayer;

    // 排序的数值范围
    public Vector2Int itemValueRange;
    // 信息提示面板
    public GameObject infoPanel;
    public Text infoText;
    public Button infoPanelCloseBtn;

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

    private void Start()
    {
        infoPanelCloseBtn.onClick.AddListener(() => infoPanel.SetActive(false));
    }

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

    public void ShowErrorCount(int errorCount)
    {
        infoText.text = ErrorText + errorCount.ToString();
        infoPanel.SetActive(true);
    }
}