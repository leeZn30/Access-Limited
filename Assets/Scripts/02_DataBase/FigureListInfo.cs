using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigureListInfo: MonoBehaviour
{
    [Header("인물")]
    public Figure figure;

    [Header("레이아웃")]
    [SerializeField] FigureInfoRayout FIRayout;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { DatabaseManager.Instance.goPage(3); });
        GetComponent<Button>().onClick.AddListener(delegate { DatabaseManager.Instance.pickFigure(figure); });
    }
}
