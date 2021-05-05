using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button exitBtn;

    public StartMenuBtnData[] btnDatas;

    private void Start()
    {
        exitBtn.onClick.AddListener(() => Application.Quit());

        for (int i = 0, length = btnDatas.Length; i < length; i++)
        {
            int btnIndex = i;

            btnDatas[btnIndex].switchPanelBtn.onClick.AddListener(() =>
            {
                btnDatas[btnIndex].targetPanel.transform.SetAsLastSibling();
            });

            btnDatas[btnIndex].enterExpBtn.onClick.AddListener(() =>
            {
                // ������Ӧ��ģ��
                switch (btnDatas[btnIndex].sortType)
                {
                    case SortType.BubbleSort:
                        UIMain.Instance.EnterBubbleSort();
                        break;
                    case SortType.SelectSort:
                        UIMain.Instance.EnterSelectSort();
                        break;
                    case SortType.QuickSort:
                        UIMain.Instance.EnterQuickSort();
                        break;
                    default:
                        break;
                }

                // �뿪��ʼ�˵�
                UIMain.Instance.LeaveStartMenu();
            });
        }
    }
}

[System.Serializable]
public struct StartMenuBtnData
{
    public Button switchPanelBtn;
    public Button enterExpBtn;
    public GameObject targetPanel;
    public SortType sortType;
}