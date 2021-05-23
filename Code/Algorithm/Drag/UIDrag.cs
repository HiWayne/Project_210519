using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform dragRect;
    //public GameObject activeObj;
    public bool needResetPos = true;

    public DragType dragType;
    internal DragState dragState;

    Action onFinish;

    internal Transform startRoot;
    DragManager manager;
    public int correctIndex;
    internal int currentIndex;

    public bool IsCorrect { get { return correctIndex == currentIndex; } }
    public int CorrectIndex { get { return correctIndex; } }

    void Awake()
    {
        if (dragRect == null)
            dragRect = transform.GetComponent<RectTransform>();

        startRoot = transform.parent;
    }

    public void Init(DragManager manager, Action onFinish = null, bool overBefore = true)
    {
        this.manager = manager;
        this.currentIndex = -1;

        if (overBefore)
            this.onFinish = onFinish;
        else
            this.onFinish += onFinish;
    }

    public void SetToStart()
    {
        transform.SetParent(startRoot);
        dragState = DragState.Normal;

        transform.localPosition = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragState == DragState.Normal)
        {
            GameMain.Instance.OnDrag(this);
            dragState = DragState.Draging;

            transform.SetParent(manager.dragingRoot);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragState == DragState.Draging)
        {
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(dragRect, eventData.position, eventData.pressEventCamera, out globalMousePos))
                dragRect.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragState == DragState.Draging)
        {
            if (IsCorrectArea())
            {
                OnCorrect();

                dragState = DragState.Normal;

                if (onFinish != null)
                    onFinish.Invoke();

                // 
                manager.OnEndDrag();
            }
            else
            {
                SetToStart();
            }
        }
    }

    protected virtual bool IsCorrectArea()
    {
        Rect rect1 = GameMain.Instance.RectTransToScreenPos(dragRect);
        Rect rect2;
        bool isCorrect = false;
        Transform tmpRoot = startRoot;
        int tmpIndex = currentIndex;
        UIDrag childDrag;

        for (int i = 0, length = manager.cols.Length; i < length; i++)
        {
            rect2 = GameMain.Instance.RectTransToScreenPos(manager.cols[i]);
            isCorrect = rect1.Overlaps(rect2);

            if (isCorrect)
            {
                currentIndex = i;

                // 把碰撞对象下的可拖拽对象更换位置
                childDrag = manager.cols[i].GetComponentInChildren<UIDrag>();
                if (childDrag != null)
                {
                    startRoot = childDrag.startRoot;
                    childDrag.startRoot = tmpRoot;
                    childDrag.currentIndex = tmpIndex;
                    childDrag.SetToStart();
                }

                break;
            }
        }

        return isCorrect;
    }

    protected virtual void OnCorrect()
    {
        switch (dragType)
        {
            case DragType.Normal:
            case DragType.SetRoot:
                startRoot = manager.cols[currentIndex];
                dragRect.SetParent(manager.cols[currentIndex]);
                if (needResetPos)
                    dragRect.localPosition = Vector3.zero;
                //dragRect.localScale = Vector3.one;
                break;
            //case DragType.ActiveOther:
            //    dragRect.gameObject.SetActive(false);
            //    activeObj.SetActive(true);
            //    break;
            default:
                break;
        }
    }
}