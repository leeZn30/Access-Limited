using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeFilter : MonoBehaviour
{
    [Header("ũ��")]
    RectTransform rect;
    [SerializeField] float minWidth;

    [Header("�ݶ��̴�")]
    [SerializeField] BoxCollider2D collider;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        collider = GetComponent<BoxCollider2D>();

        sizeFiltering();
        colliderSizeFiltering();
    }

    void Update()
    {
        //sizeFiltering();
    }


    // ������ ����
    void sizeFiltering()
    {
        float width = GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

        if (width > minWidth)
        {
            rect.sizeDelta = new Vector2(width + 40f, rect.sizeDelta.y);
        }
        else
        {
            rect.sizeDelta = new Vector2(400f, rect.sizeDelta.y);
        }
    }

    void colliderSizeFiltering()
    {
        if (collider != null)
        {
            collider.size = rect.sizeDelta;
        }
    }
}
