using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoItem : MonoBehaviour
{
    public int Value { get { return itemValue; } }

    public Image infoImage;
    public Text infoText;

    int itemValue;

    private void Start()
    {
        itemValue = Random.Range(UIMain.Instance.itemValueRange.x, UIMain.Instance.itemValueRange.y + 1);

        infoImage.fillAmount = (float)itemValue / UIMain.Instance.itemValueRange.y;
        infoText.text = itemValue.ToString();
    }
}