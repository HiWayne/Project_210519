using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragManager : MonoBehaviour
{
    public RectTransform[] cols;

    internal List<UIDrag> dragInsts;

    Action onFinish;

    public virtual void Init(Action onFinish = null)
    {
        dragInsts = new List<UIDrag>();
        this.onFinish = onFinish;
    }

    public virtual void OnEndDrag()
    {
        bool finish = true;

        for (int i = 0, length = dragInsts.Count; i < length; i++)
        {
            finish &= dragInsts[i].IsCorrect;
        }

        if (finish)
            onFinish?.Invoke();
    }
}