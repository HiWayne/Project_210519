using GameBasic;
using GameBasic.EntityV1;
using UnityEngine;

// 游戏主要脚本，充当管理员
public class GameMain : MonoSingleton<GameMain>
{
    // 对象池，未使用
    internal EntityPool entityPool;

    UIDrag dragingObj;
    TaskRunner taskRunner;

    protected override void Awake()
    {
        base.Awake();

        // pool
        GameObject poolRoot = new GameObject("Pool");
        poolRoot.transform.SetParent(transform);
        entityPool = new EntityPool();
        entityPool.Init();
        entityPool.root = poolRoot.transform;

        // task runner
        taskRunner = new TaskRunner();
        TaskRunner.SetDefault(taskRunner);

        // thread
        UnityThread.Create();
        UnityThread.Instance.mono = this;
    }

    private void Start()
    {
        UIMain.Instance.EnterStartMenu();
    }

    private void Update()
    {
        // 更新基础组件
        taskRunner.Update();
        UnityThread.Instance.Update();
    }

    protected override void OnDestroy()
    {
        TaskRunner.SetDefault(null);

        base.OnDestroy();
    }

    void OnApplicationFocus()
    {
        if (!Application.isFocused)
            EndDrag();
    }

    public void StartGame()
    {
    }

    #region 拖拽相关
    public void OnDrag(UIDrag dragObj)
    {
        this.dragingObj = dragObj;
    }

    public void EndDrag()
    {
        if (dragingObj != null)
            dragingObj.SetToStart();
    }
    #endregion

    #region 辅助功能
    /// <summary>
    /// 转换 rect
    /// </summary>
    /// <param name="rt"></param>
    /// <param name="cam"></param>
    /// <returns></returns>
    public Rect RectTransToScreenPos(RectTransform rt, Camera cam = null)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector2 v0 = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
        Vector2 v1 = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
        Rect rect = new Rect(v0, v1 - v0);
        return rect;
    }
    #endregion
}