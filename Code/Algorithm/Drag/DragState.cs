/// <summary>
/// 拖拽的状态
/// </summary>
[System.Serializable]
public enum DragState
{
    /// <summary>
    /// 未拖拽
    /// </summary>
    Normal,

    /// <summary>
    /// 拖拽中
    /// </summary>
    Draging,

    /// <summary>
    /// 已到目标位置，结束拖拽
    /// </summary>
    End,
}
