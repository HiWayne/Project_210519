using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowChartElement : MonoBehaviour
{
    public Image forntImage;

    public void SetHightlight()
    {
        forntImage.color = Color.red; ;
    }

    public void SetNormal()
    {
        forntImage.color = Color.white;
    }
}