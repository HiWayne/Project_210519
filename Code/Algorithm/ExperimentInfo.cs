using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentInfo : MonoBehaviour
{
    public Button experimentPurpose;
    public Button exprimentRequire;
    public Button startExperiment;
    public Button exitExperiment;
    public Text currentTitle;
    public Text currentDescription;

    private void Start()
    {
        // 入口(StartMenu)选中的index
        int sortIndex = UIMain.Instance.CurrentSortIndex;
        // 全局数据
        PlayerData currentPlayer = DataBase.Instance.currentPlayer;

        // 默认标题
        currentTitle.text = currentPlayer.expsData[sortIndex].data.name + "-实验目的";
        // 默认描述
        currentDescription.text = currentPlayer.expsData[sortIndex].data.purpose;

        experimentPurpose.onClick.AddListener(() =>
        {
            Debug.Log(currentPlayer.expsData[sortIndex]);
            currentTitle.text = currentPlayer.expsData[sortIndex].data.name + "-实验目的";
            currentDescription.text = currentPlayer.expsData[sortIndex].data.purpose;
        });

        exprimentRequire.onClick.AddListener(() =>
        {
            currentTitle.text = currentPlayer.expsData[sortIndex].data.name + "-实验要求";
            currentDescription.text = currentPlayer.expsData[sortIndex].data.require;
        });

        // 开始实验按钮
        startExperiment.onClick.AddListener(() =>
        {
            // 进入相应的模块
            UIMain.Instance.EnterSortPanel(sortIndex);
            // 销毁本页面
            UIMain.Instance.LeaveExperimentInfoPage();
        });

        // 退出实验按钮
        exitExperiment.onClick.AddListener(() =>
        {
            // 进入开始菜单页
            UIMain.Instance.EnterStartMenu();
            // 销毁本页面
            UIMain.Instance.LeaveExperimentInfoPage();
        });
    }
}