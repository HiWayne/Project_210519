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
    public SortManager[] sortPanelPfs;

    // 实例
    StartMenu startMenuInst;
    SortManager currentSortPanel;

    public int CurrentSortIndex
    {
        get
        {
            if (currentSortPanel == null)
                return 0;
            return (int)currentSortPanel.sortType;
        }
    }

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

    public void EnterSortPanel(int sortIndex)
    {
        Vector2 sortInfo = DataBase.Instance.GetCurrentSortData(sortIndex);

        currentSortPanel = Instantiate(sortPanelPfs[sortIndex], defaultLayer);


        UpdateCurrentSortPanelUI((int)sortInfo.x, sortInfo.y);
    }

    public void LeaveCurrentSortPanel()
    {
        if (currentSortPanel != null)
            Destroy(currentSortPanel.gameObject);
    }

    public void UpdateCurrentSortPanelUI(int expCount, float expMaxScore)
    {
        if (currentSortPanel != null)
            currentSortPanel.UpdateUI(expCount, expMaxScore);
    }

    public void ShowErrorCount(int errorCount)
    {
        infoText.text = ErrorText + errorCount.ToString();
        infoPanel.SetActive(true);
    }
}