using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlowChartPanel : DragManager
{
    public const int TotalCheckPoint = 10;

    public Button zoomInBtn;
    public Button zoomOutBtn;
    public Button submitBtn;
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
    // 下划线面板
    public GameObject linePanel;
    // 信息演示面板
    public GameObject infosPanel;

    [Header("输入部分")]
    public IFData[] inputDatas;
    public Button inputBtn;
    public InputField inputText;
    int currentIFIndex = -1;

    [Header("拖拽部分")]
    public UIDrag[] drags;

    public bool CorrectAll { get { return ErrorCount == 0 || GameMain.Instance.test; } }

    // 所有空不能为空
    public bool HasEmpty
    {
        get
        {
            bool hasEmpty = false;

            for (int i = 0, length = inputDatas.Length; i < length; i++)
            {
                int index = i;

                if (inputDatas[index].inputField.text.Replace(" ", "") == "")
                    hasEmpty = true;
            }
            // 每种排序的流程图拼接数量都是6个，所有的drags中有6个index != -1，则说明没有空缺
            int count = 0;
            for (int i = 0, length = drags.Length; i < length; i++)
            {
                if (drags[i].currentIndex != -1)
                    count++;
            }
            if (count < 6)
            {
                hasEmpty = true;
            }
            return hasEmpty;
        }
    }

    public int ErrorCount
    {
        get
        {
            int count = 0;

            for (int i = 0, length = inputDatas.Length; i < length; i++)
            {
                int index = i;

                if (inputDatas[index].inputField.text.Replace(" ", "") != inputDatas[index].corretContent.Replace(" ", ""))
                    count++;
            }
            for (int i = 0, length = drags.Length; i < length; i++)
            {
                // drag可能有多余的，被移动的drag才应该计算错误
                if (!drags[i].IsCorrect && drags[i].currentIndex != -1)
                    count++;
            }

            return count;
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
        submitBtn.onClick.AddListener(() => SubmitAnswer());
        exitBtn.onClick.AddListener(() =>
        {
            UIMain.Instance.LeaveCurrentSortPanel();
            UIMain.Instance.EnterStartMenu();
        });

        //
        for (int i = 0, length = inputDatas.Length; i < length; i++)
        {
            int inputIndex = i;

            inputDatas[inputIndex].inputField.onClick += () =>
            {
                if (currentIFIndex != -1)
                {
                    inputDatas[currentIFIndex].inputField.SetToOriginData();
                    inputDatas[currentIFIndex].markObj.SetActive(false);
                }

                currentIFIndex = inputIndex;
                inputText.text = inputDatas[currentIFIndex].inputField.text;
                inputDatas[currentIFIndex].markObj.SetActive(true);
            };
        }
        //
        inputBtn.onClick.AddListener(() =>
        {
            if (currentIFIndex != -1)
            {
                inputDatas[currentIFIndex].inputField.text = inputText.text;
                inputDatas[currentIFIndex].inputField.SaveCurrentData();
                inputDatas[currentIFIndex].markObj.SetActive(false);

                currentIFIndex = -1;
            }

            inputText.text = "";
        });

        for (int i = 0, length = drags.Length; i < length; i++)
        {
            drags[i].Init(this);
        }
    }

    public void ZoomOut()
    {
        int errorCount = ErrorCount;
        bool correctAll = ErrorCount == 0;

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
        });
    }

    public void SubmitAnswer()
    {
        // 如果有空缺，提示，并不做任何操作
        if (HasEmpty)
        {
            UIMain.Instance.ShowHasEmpty(HasEmpty);
            return;
        }
        int errorCount = ErrorCount;
        bool correctAll = ErrorCount == 0;

        codePanel.SetActive(correctAll);
        linePanel.SetActive(!correctAll);
        int LimitedErrorCount = TotalCheckPoint;
        if (!correctAll)
            LimitedErrorCount = ErrorCount <= TotalCheckPoint ? ErrorCount : TotalCheckPoint;
        UIMain.Instance.ShowErrorCount(LimitedErrorCount);

        infosPanel.SetActive(correctAll);

        // data
        DataBase.Instance.OnExpSubmit(UIMain.Instance.CurrentSortIndex, 100f / TotalCheckPoint * (TotalCheckPoint - errorCount));
    }
}

[System.Serializable]
public struct IFData
{
    public InputFieldExt inputField;
    public string corretContent;

    // 标记对象
    public GameObject markObj;
}