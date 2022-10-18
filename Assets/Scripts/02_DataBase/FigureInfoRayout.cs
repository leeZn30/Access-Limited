using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FigureInfoRayout : MonoBehaviour
{
    [Header("é��")]
    [SerializeField] int chapter;
    public Figure pickedFigure;

    [Header("������Ʈ")]
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI figureInfo;
    [SerializeField] TextMeshProUGUI content;


    public void setContent()
    {
        figureInfo.text = pickedFigure.name + " / " + pickedFigure.age + "(" + pickedFigure.gender + ")";
        content.text = pickedFigure.showContentsTxt();
    }
}
