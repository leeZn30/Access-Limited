using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FigureListRayout : MonoBehaviour
{
    [Header("é��")]
    [SerializeField] int chapter;

    [Header("������Ʈ")]
    [SerializeField] GameObject list_prb;
    [SerializeField] GameObject scrollContent;

    void Start()
    {
        chapter = DatabaseManager.Instance.chapter;

        loadFigures();
    }

    void loadFigures()
    {
        foreach(Figure figure in GameData.Instance.figures)
        {
            GameObject each = Instantiate(list_prb, scrollContent.transform);

            each.GetComponentInChildren<TextMeshProUGUI>().text = "" + figure.name + "/ " + figure.age + "(" + figure.gender + ")";
            each.GetComponent<Button>().onClick.AddListener(delegate { DatabaseManager.Instance.goPage(3); });
        }
    }

}
