using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeFilter : MonoBehaviour
{
    [Header("크기")]
    RectTransform rect;
    float minWidth = 400f;
    [SerializeField] BoxCollider2D collider;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        sizeFiltering();
    }


    // 사이즈 조절
    void sizeFiltering()
    {
        float width = GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

        if (width > minWidth)
        {
            rect.sizeDelta = new Vector2(width + 40f, rect.sizeDelta.y);
            collider.size = rect.sizeDelta;
        }
        else
        {
            rect.sizeDelta = new Vector2(400f, rect.sizeDelta.y);
            collider.size = rect.sizeDelta;
        }
    }
}
