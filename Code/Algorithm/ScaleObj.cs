using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObj : MonoBehaviour
{
    public RectTransform targetObj;

    public float minScale;
    public float maxScale;

    private void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(targetObj, Input.mousePosition))
        {
            float scrollValue = Input.GetAxis("Mouse ScrollWheel");

            if (scrollValue != 0)
            {
                Vector3 targetScale = targetObj.localScale + Vector3.one * scrollValue;

                if (targetScale.x < minScale)
                {
                    targetScale = Vector3.one * minScale;
                }
                else if (targetScale.x > maxScale)
                {
                    targetScale = Vector3.one * maxScale;
                }

                targetObj.localScale = targetScale;
            }
        }
    }
}