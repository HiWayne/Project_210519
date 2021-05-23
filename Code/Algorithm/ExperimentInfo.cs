using UnityEngine;
using UnityEngine.UI;

public class ExperimentInfo
{
    public Button exprimentRequire;
    public Button experimentPurpose;
    public Button startExperiment;
    public Button exitExperiment;
    public PlayerData currentPlayer;
    public string currentTitle;
    public string currentDescription;

    private void Start()
    {
        exprimentRequire.onClick.AddListener(() =>
        {
            int sortIndex = UIMain.Instance.CurrentSortIndex;
            currentTitle = currentPlayer.expsData[sortIndex].data.name + "实验要求";
            currentDescription = currentPlayer.expsData[sortIndex].data.require;
        });

        experimentPurpose.onClick.AddListener(() =>
        {
            int sortIndex = UIMain.Instance.CurrentSortIndex;
            currentTitle = currentPlayer.expsData[sortIndex].data.name + "实验目的";
            currentDescription = currentPlayer.expsData[sortIndex].data.purpose;
        });

        // 开始实验按钮
        startExperiment.onClick.AddListener(() =>
        {
            int sortIndex = UIMain.Instance.CurrentSortIndex;
            // 进入相应的模块
            UIMain.Instance.EnterSortPanel(sortIndex);
        });

        // 退出实验按钮
        exitExperiment.onClick.AddListener(() =>
        {
            AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        });
    }
}