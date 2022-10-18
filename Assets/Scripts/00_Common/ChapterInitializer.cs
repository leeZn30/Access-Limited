using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterInitializer : Singleton<ChapterInitializer>
{
    [Header("챕터 초기 정보")]
    [SerializeField] int chapter;
    [SerializeField] List<Figure> figures;

    [Header("초기 정보를 위한 CSV")]
    [SerializeField] TextAsset[] ffs = new TextAsset[6];

    void Awake()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        setInitialFigure();
    }

    // 챕터별 인물 파일 생성
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
