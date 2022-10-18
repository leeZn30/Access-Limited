using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EventFileRayout : MonoBehaviour
{

    [Header("��� ����")]
    [SerializeField] int _chapter;
    [SerializeField] string content;
    [SerializeField] TextAsset csv;


    public int chapter
    {
        get { return _chapter; }
        set { _chapter = value; }
    }

    [Header("������Ʈ")]
    [SerializeField] Button exitbtn;
    [SerializeField] Button figurebtn;
    [SerializeField] Button provisobtn;
    [SerializeField] TextMeshProUGUI contenttxt;

    [Header("���̾ƿ�")]
    [SerializeField] GameObject eventFileRayout;
    [SerializeField] GameObject figureListRayout;

    void Awake()
    {
        List<Dictionary<string, object>> infos = CSVReader.Read("CSVfiles/02_Database/Eventfiles/" + csv.name);
        content = infos.Where(info => info["Chapter"].ToString() == chapter.ToString()).ToList()[0]["Content"].ToString();
        contenttxt.text = content;
    }

    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> infos = CSVReader.Read("CSVfiles/02_Database/Eventfiles/" + csv.name);
        content = infos.Where(info => info["Chapter"].ToString() == chapter.ToString()).ToList().First()["Content"].ToString();
        contenttxt.text = content;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
