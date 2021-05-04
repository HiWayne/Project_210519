/// <summary>
    /// 拖拽的类型
    /// </summary>
    [System.Serializable]
    public enum DragType
    {
        /// <summary>
        /// 正常型，目标位置与触发位置相同，拖拽到目标位置就粘贴到上面
        /// </summary>
        Normal,

        /// <summary>
        /// 目标位置与触发不相同，拖拽到目标区域就粘贴到设置好的根位置
        /// </summary>
        SetRoot,

        /// <summary>
        /// 拖拽到目标位置反激活自己，激活别的对象
        /// </summary>
        //ActiveOther,
    }