using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class InputFieldExt : InputField
{
    string originData;

    internal Action onClick;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        onClick?.Invoke();
    }

    public void SaveCurrentData()
    {
        originData = text;
    }

    public void SetToOriginData()
    {
        text = originData;
    }
}