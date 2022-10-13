using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeFilter : MonoBehaviour
{
    [Header("크기")]
    RectTransform rect;
    float minWidth;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        sizeFiltering();
    }


    // 사이즈 조절
    void sizeFiltering()
    {
        float width = GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

        if (width > 400f)
        {
            rect.sizeDelta = new Vector2(width + 40f, 100);
        }
        else
        {
            rect.sizeDelta = new Vector2(400f, 100);
        }
    }
}
