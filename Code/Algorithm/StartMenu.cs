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
        exitBtn.onClick.AddListener(() =>
        {
            // 返回登录页面
            AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        });

        for (int i = 0, length = btnDatas.Length; i < length; i++)
        {
            int btnIndex = i;

            btnDatas[btnIndex].switchPanelBtn.onClick.AddListener(() =>
            {
                btnDatas[btnIndex].targetPanel.transform.SetAsLastSibling();
            });

            btnDatas[btnIndex].enterExpBtn.onClick.AddListener(() =>
            {
                // 保存选择的index
                UIMain.Instance.ChangeCurrentSortIndex(btnIndex);

                // 进入实验信息页面
                UIMain.Instance.EnterExperimentInfoPage();

                // 离开开始菜单
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
}