using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlowChartPanel : DragManager
{
    public Button zoomInBtn;
    public Button zoomOutBtn;
    public Button exitBtn;
    public RectTransform targetRect;

    // 拉伸范围
    public Vector2 widthRange;
    // 持续时间
    public float duration;

    // 拖拽内容面板
    public GameObject dragsPanel;
    // 代码面板
    public GameObject codePanel;
    // 信息演示面板
    public GameObject infosPanel;

    [Header("输入部分")]
    public IFData[] inputDatas;
    [Header("拖拽部分")]
    public UIDrag[] drags;

    public bool CorrectAll
    {
        get
        {
            bool corret = true;

            for (int i = 0, length = inputDatas.Length; i < length; i++)
            {
                int index = i;

                corret &= inputDatas[index].inputField.text == inputDatas[index].corretContent;
            }
            for (int i = 0, length = drags.Length; i < length; i++)
            {
                corret &= drags[i].IsCorrect;
            }

            return corret;
        }
    }

    Tween zoomIn;
    Tween zoomOut;

    private void Start()
    {
        zoomInBtn.onClick.AddListener(() =>
        {
            zoomInBtn.gameObject.SetActive(false);

            zoomIn = targetRect.DOSizeDelta(new Vector2(widthRange.y, targetRect.sizeDelta.y), duration);
            zoomIn.OnComplete(() =>
            {
                dragsPanel.SetActive(true);
            });
        });
        zoomOutBtn.onClick.AddListener(() => ZoomOut());
        exitBtn.onClick.AddListener(() =>
        {
            UIMain.Instance.LeaveCurrentSortPanel();
            UIMain.Instance.EnterStartMenu();
        });

        for (int i = 0, length = drags.Length; i < length; i++)
        {
            drags[i].Init(this);
        }
    }

    public void ZoomOut()
    {
        bool correctAll = CorrectAll;

        if (zoomIn != null)
            zoomIn.Kill();
        if (zoomOut != null)
            zoomOut.Kill();

        dragsPanel.SetActive(false);

        zoomOut = targetRect.DOSizeDelta(new Vector2(widthRange.x, targetRect.sizeDelta.y), duration);
        zoomOut.OnComplete(() =>
        {
            zoomInBtn.gameObject.SetActive(true);
            zoomInBtn.interactable = !correctAll;

            codePanel.SetActive(correctAll);
            infosPanel.SetActive(correctAll);
        });
    }
}

[System.Serializable]
public struct IFData
{
    public InputField inputField;
    public string corretContent;
}