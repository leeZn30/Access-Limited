using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FigureListRayout : MonoBehaviour
{
    [Header("Ã©ÅÍ")]
    [SerializeField] int chapter;

    void Start()
    {
        chapter = DatabaseManager.Instance.chapter;

    }

    void OnEnable()
    {
        destroyFigures(); // disalble¿¡´Ù ÇÏ¸é ¾ÈµÊ ºý..
        loadFigures();
    }

    void destroyFigures()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("FigureList");

        foreach(GameObject go in gos)
        {
            go.SetActive(false);
            ObjectPool.Instance.figureQueue.Enqueue(go);
        }
    }

    void loadFigures()
    {
        foreach(Figure figure in GameData.Instance.figures)
        {
            //GameObject each = Instantiate(list_prb, scrollContent.transform);
            GameObject each = ObjectPool.Instance.figureQueue.Dequeue();
            each.GetComponentInChildren<TextMeshProUGUI>().text = "" + figure.name + " / " + figure.age + "(" + figure.gender + ")";
            each.GetComponent<FigureListInfo>().figure = figure;

            each.SetActive(true);

        }
    }

}
