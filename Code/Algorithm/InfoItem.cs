using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoItem : MonoBehaviour
{
    public int Value { get { return itemValue; } }

    public Color normalColor;
    public Color highlightColor;

    public Image infoImage;
    public Text infoText;

    int itemValue;

    public int Init()
    {
        itemValue = Random.Range(UIMain.Instance.itemValueRange.x, UIMain.Instance.itemValueRange.y + 1);

        infoImage.fillAmount = (float)itemValue / UIMain.Instance.itemValueRange.y;
        infoText.text = itemValue.ToString();

        return itemValue;
    }

    public void SetHightlight()
    {
        infoImage.color = highlightColor;
    }

    public void SetNormal()
    {
        infoImage.color = normalColor;
    }
}