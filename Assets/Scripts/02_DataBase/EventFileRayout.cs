using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EventFileRayout : MonoBehaviour
{
    [Header("é��")]
    [SerializeField] int chapter;

    [Header("������Ʈ")]
    [SerializeField] Image eventImg;
    [SerializeField] TextMeshProUGUI content;

    void Start()
    {
        chapter = DatabaseManager.Instance.chapter;

        content.text = DatabaseManager.Instance.getCSV(0).Where(line => line["Chapter"].ToString() == chapter.ToString()).ToList()[0]["Content"].ToString();

    }


}
