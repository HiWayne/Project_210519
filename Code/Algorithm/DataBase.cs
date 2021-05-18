using GameBasic;
using UnityEngine;

public class DataBase : MonoSingleton<DataBase>
{
    // ʵ����߷����ɵ���ʵ�����������ʵ��������ڸ�ֵʱ���ύ�����޸ĸ�ʵ�����߷���
    public const int ExpMaxCount = 10;

    public PlayerData currentPlayer;

    // ϵͳ�Ƿ��ʼ��
    bool inited;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    // ��ʼ������ϵͳ
    public void Init(PlayerData player)
    {
        this.currentPlayer = player;

        this.inited = true;
    }

    // ʵ�����ύ��ťʱִ��
    public void OnExpSubmit(int expIndex, float currentScore)
    {
        if (!inited)
            return;

        if (currentPlayer.expsData[expIndex].expCount < ExpMaxCount)
        {
            currentPlayer.expsData[expIndex].expCount++;

            if (currentScore > currentPlayer.expsData[expIndex].maxScore)
                currentPlayer.expsData[expIndex].maxScore = currentScore;
        }

        // TODO
        // SQL data modify

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

// �������
public struct PlayerData
{
    public ExpData[] expsData;
}

// ʵ������
public struct ExpData
{
    public int expCount;
    public float maxScore;
}