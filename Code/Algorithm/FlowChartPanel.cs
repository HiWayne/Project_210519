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
    public Button exitBtn;
    public RectTransform targetRect;

    // ���췶Χ
    public Vector2 widthRange;
    // ����ʱ��
    public float duration;

    // ��ק�������
    public GameObject dragsPanel;
    // �������
    public GameObject codePanel;
    // �»������
    public GameObject linePanel;
    // ��Ϣ��ʾ���
    public GameObject infosPanel;

    [Header("���벿��")]
    public IFData[] inputDatas;
    public Button inputBtn;
    public Text inputText;
    public Scrollbar inputTextSB;
    int currentIFIndex = -1;

    [Header("��ק����")]
    public UIDrag[] drags;

    public bool CorrectAll { get { return ErrorCount == 0 || GameMain.Instance.test; } }

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
                if (!drags[i].IsCorrect)
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

            inputDatas[inputIndex].inputField.onValueChanged.AddListener(data =>
            {
                inputText.text = "  " + data + "    ";

                inputTextSB.value = 1;
            });
        }
        //
        inputBtn.onClick.AddListener(() =>
        {
            inputText.text = "";

            if (currentIFIndex != -1)
            {
                inputDatas[currentIFIndex].inputField.SaveCurrentData();
                inputDatas[currentIFIndex].markObj.SetActive(false);

                currentIFIndex = -1;
            }
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
        codePanel.SetActive(correctAll);
        linePanel.SetActive(!correctAll);
        if (!correctAll)
            UIMain.Instance.ShowErrorCount(ErrorCount);

        zoomOut = targetRect.DOSizeDelta(new Vector2(widthRange.x, targetRect.sizeDelta.y), duration);
        zoomOut.OnComplete(() =>
        {
            zoomInBtn.gameObject.SetActive(true);
            zoomInBtn.interactable = !correctAll;

            infosPanel.SetActive(correctAll);
        });

        // data
        DataBase.Instance.OnExpSubmit(UIMain.Instance.CurrentSortIndex, 100f / TotalCheckPoint * (TotalCheckPoint - errorCount));
    }
}

[System.Serializable]
public struct IFData
{
    public InputFieldExt inputField;
    public string corretContent;

    // ��Ƕ���
    public GameObject markObj;
}