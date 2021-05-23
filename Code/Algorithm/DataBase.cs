using GameBasic;
using UnityEngine;

public class DataBase : MonoSingleton<DataBase>
{
    // 实验最高分数可调节实验次数，当该实验次数大于该值时，提交不再修改该实验的最高分数
    public const int ExpMaxCount = 10;

    public PlayerData currentPlayer;

    public Request request;

    // 系统是否初始化
    bool inited;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    // 初始化数据系统
    public void Init(PlayerData player)
    {
        this.currentPlayer = player;

        this.inited = true;
    }

    // 实验点击提交按钮时执行
    public void OnExpSubmit(int expIndex, float currentScore)
    {
        if (!inited)
            return;

        if (currentPlayer.expsData[expIndex].expCount < ExpMaxCount)
        {
            currentPlayer.expsData[expIndex].expCount++;

            // SQL data modify
            // count + 1
            request.addCount(currentPlayer.id, currentPlayer.expsData[expIndex].data.id);
            request.setGrade(currentPlayer.id, currentPlayer.expsData[expIndex].data.id, currentScore);

            if (currentScore > currentPlayer.expsData[expIndex].maxScore)
                currentPlayer.expsData[expIndex].maxScore = currentScore;
        }

        // ui
        UIMain.Instance.UpdateCurrentSortPanelUI(currentPlayer.expsData[expIndex].expCount, currentPlayer.expsData[expIndex].maxScore);
    }

    public Vector2 GetCurrentSortData(int sortIndex)
    {
        if (!inited)
            return Vector2.zero;

        return new Vector2(currentPlayer.expsData[sortIndex].expCount, currentPlayer.expsData[sortIndex].maxScore);
    }
}

// 玩家数据
public struct PlayerData
{
    public int id;
    public ExpData[] expsData;
}

// 实验数据
public struct ExpData
{
    public ExperimentEntity data;
    public int expCount;
    public float maxScore;
}