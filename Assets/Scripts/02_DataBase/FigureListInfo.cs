using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigureListInfo: MonoBehaviour
{
    [Header("�ι�")]
    public Figure figure;

    [Header("���̾ƿ�")]
    [SerializeField] FigureInfoRayout FIRayout;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { DatabaseManager.Instance.goPage(3); });
        GetComponent<Button>().onClick.AddListener(delegate { DatabaseManager.Instance.pickFigure(figure); });
    }
}
