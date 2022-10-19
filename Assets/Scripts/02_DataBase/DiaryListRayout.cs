using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiaryListRayout : MonoBehaviour
{
    [Header("√©≈Õ")]
    [SerializeField] int chapter;
    [SerializeField] int selectChapter;

    [Header("√©≈Õ ø¨∞·")]
    [SerializeField] LinkedList<int> chapters = new LinkedList<int>();

    [Header("ø¿∫Í¡ß∆Æ")]
    [SerializeField] TextMeshProUGUI chapterNumtxt;

    void Start()
    {
        chapter = DatabaseManager.Instance.chapter;
        selectChapter = chapter;

        chapterNumtxt.text = "Chapter. " + chapter;

        setChapterLinks();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectChapter--;
            chapterNumtxt.text = "Chapter. " + selectChapter;
        }
    }

    void setChapterObject()
    {

    }

    void setChapterLinks()
    {
        LinkedListNode<int> node = new LinkedListNode<int>(0);
        chapters.AddFirst(node);

        for (int i = 1; i <= chapter; i++)
        {
            LinkedListNode<int> nowNode = chapters.AddLast(i);
            LinkedListNode<int> prevNode = new LinkedListNode<int>(i-1);
            LinkedListNode<int> nextNode = new LinkedListNode<int>(i+1);

            chapters.AddBefore(nowNode, prevNode);
            chapters.AddAfter(nowNode, nextNode);
        }
    }
}
