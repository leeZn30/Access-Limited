using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FigureInfoRayout : MonoBehaviour
{
    [Header("챕터")]
    [SerializeField] int chapter;
    public Figure pickedFigure;

    [Header("오브젝트")]
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI figureInfo;
    [SerializeField] TextMeshProUGUI content;


    public void setContent()
    {
        figureInfo.text = pickedFigure.name + " / " + pickedFigure.age + "(" + pickedFigure.gender + ")";
        content.text = pickedFigure.showContentsTxt();
    }
}
