using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EventFileRayout : MonoBehaviour
{
    [Header("챕터")]
    [SerializeField] int chapter;

    [Header("오브젝트")]
    [SerializeField] Image eventImg;
    [SerializeField] TextMeshProUGUI content;

    void Start()
    {
        chapter = DatabaseManager.Instance.chapter;

        content.text = DatabaseManager.Instance.getCSV(0).Where(line => line["Chapter"].ToString() == chapter.ToString()).ToList()[0]["Content"].ToString();

    }


}
