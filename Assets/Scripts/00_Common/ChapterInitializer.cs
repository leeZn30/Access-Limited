using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterInitializer : Singleton<ChapterInitializer>
{
    [Header("é�� �ʱ� ����")]
    [SerializeField] int chapter;
    [SerializeField] List<Figure> figures;

    [Header("�ʱ� ������ ���� CSV")]
    [SerializeField] TextAsset[] ffs = new TextAsset[6];

    void Awake()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        setInitialFigure();
    }

    // é�ͺ� �ι� ���� ����
    void setInitialFigure()
    {
        List<Dictionary<string, object>> chapterFigures = CSVReader.Read("CSVfiles/02_Database/Figurefiles/" + ffs[chapter].name);

        for (int i = 0; i < chapterFigures.Count; i++)
        {
            Dictionary<string, object> figureInfo = chapterFigures[i];

            Debug.Log(figureInfo["Name"].ToString());

            Figure figure = new Figure(int.Parse(figureInfo["Id"].ToString()), 
                                       figureInfo["Name"].ToString(), 
                                       int.Parse(figureInfo["Age"].ToString()),
                                       figureInfo["Gender"].ToString(),
                                       figureInfo["Content"].ToString());

            GameData.Instance.figures.Add(figure);
        }
    }
}
