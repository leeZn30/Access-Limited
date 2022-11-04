using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterInitializer : Singleton<ChapterInitializer>
{
    [Header("é�� �ʱ� ����")]
    [SerializeField] int chapter;
    [SerializeField] List<Figure> figures;

    [Header("�ʱ� ������ ���� CSV")]
    [SerializeField] string path = "CSVfiles/02_Database/Figurefiles/";

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
        TextAsset nowFF = Resources.Load<TextAsset>(path + chapter);

        List<Dictionary<string, object>> chapterFigures 
            = CSVReader.Read("CSVfiles/02_Database/Figurefiles/" + nowFF.name + "(tmp)");
            //= CSVReader.Read("CSVfiles/02_Database/Figurefiles/" + nowFF.name);

        for (int i = 0; i < chapterFigures.Count; i++)
        {
            Dictionary<string, object> figureInfo = chapterFigures[i];

            Figure figure = new Figure(figureInfo["Id"].ToString(), 
                                       figureInfo["Name"].ToString(), 
                                       int.Parse(figureInfo["Age"].ToString()),
                                       figureInfo["Gender"].ToString(),
                                       figureInfo["Content"].ToString(),
                                       0);

            GameData.Instance.figures.Add(figure);
        }
    }
}
