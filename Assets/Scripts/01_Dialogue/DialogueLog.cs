using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueLog : MonoBehaviour
{
    [Header("��ũ�Ѻ� ������Ʈ")]
    ScrollRect scrollRect;
    RectTransform rectTransform;

    [Header("�α� ������Ʈ")]
    [SerializeField] GameObject dialogue;
    float itemHeight;

    [Header("��ũ�Ѻ� ������")]
    int dataCount;
    // ������ itemList
    readonly List<GameObject> itemList = new List<GameObject>();
    List<List<string>> logs = new List<List<string>>();

    [Header("��ũ�Ѻ� ����")]
    int spacing = 1; // �� �α� ������ ����
    float verticalPadding = 3f; // ��ũ�Ѻ信�� �� �Ʒ��� �� �Ÿ�
    readonly int bufItemCount = 4;
    float updateCount;
    float scrollY = 0f;

    // ���� �ѹ� ���� -> ��Ե� ��ȭ �α׸� Ű�� �ѹ��� �����
    void Awake()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
        rectTransform = GetComponent<RectTransform>();
        itemHeight = dialogue.GetComponent<RectTransform>().rect.height + spacing;

        // scroll content�� ������ ũ�� ����
        int scrollCount = (int)((rectTransform.rect.height - verticalPadding * 2 + spacing)
                                 / itemHeight) + 1 + bufItemCount;

        float pos = -verticalPadding;
        // scorll content�� �����ִ� �α� ����
        for (int i = 0; i < scrollCount; i++)
        {
            var item = Instantiate(dialogue, scrollRect.content);
            itemList.Add(item);
            // item name�̶� line�ٲ��ִ� ó�� ����
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, pos);
            pos -= itemHeight;
        }

        SetContentSize();

        scrollRect.onValueChanged.AddListener(RefreshItemListener);

        // scroll�� �������� ������ data�� ������
        if (dataCount < scrollCount)
        {
            updateCount = float.MaxValue;
            int i;
            for (i = 0; i < dataCount; i++)
            {
                TextMeshProUGUI[] texts = itemList[i].GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = logs[i][0];
                texts[1].text = logs[i][1];

                itemList[i].SetActive(true);
            }
            for (; i < scrollCount; i++)
            {
                itemList[i].SetActive(false);
            }
        }
        else
        {
            updateCount = 1.0f / dataCount;
            RefreshItem();
        }
    }

    // ���� �ʿ��Ѱ�
    void SetContentSize()
    {
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x,
                                                    dataCount * itemHeight - spacing + verticalPadding * 2);
    }

    void RefreshItemListener(Vector2 value)
    {
        Debug.LogFormat("RefreshLinstener Operate!");

        if (value.y < 0f || value.y > 1f) return;

        if (Mathf.Abs(value.y - scrollY) > updateCount)
        {
            scrollY = value.y;
            RefreshItem();
        }
    }

    void RefreshItem()
    {
        int addIdx;

        foreach (GameObject item in itemList)
        {
            if ((addIdx = RelocateItem(item)) != 0)
            {
                int idx = itemList.FindIndex(e => e == item) + addIdx;
                Debug.Log(idx);
            }

            SetItemActive(item);
        }
    }

    int RelocateItem(GameObject log)
    {
        float contentY = scrollRect.content.anchoredPosition.y;
        float y = log.transform.localPosition.y + contentY;
        float top = -verticalPadding + itemHeight * 2;
        float bottom = -rectTransform.rect.height + verticalPadding - itemHeight * 2;

        float changeValue = itemList.Count * itemHeight;
        float rangeB = Mathf.Abs(y - bottom) / changeValue;
        float rangeT = Mathf.Abs(y - top) / changeValue;
        int jumpCount;

        if (rangeB > rangeT) jumpCount = (int)rangeB;
        else jumpCount = (int)rangeT;

        if (y > top)
        {
            log.transform.localPosition -= new Vector3(0, changeValue * jumpCount);
            return jumpCount * itemList.Count;
        }
        else if (y < bottom)
        {
            log.transform.localPosition += new Vector3(0, changeValue * jumpCount);
            return -jumpCount * itemList.Count;
        }

        return 0;
    }

    void SetItemActive(GameObject item)
    {
        int idx = (int)(-(item.transform.localPosition.y
                        + verticalPadding) / itemHeight);

        if (idx < 0 || idx >= dataCount)
        {
            item.SetActive(false);
        }
        else if (!item.gameObject.activeSelf)
        {
            int index = itemList.FindIndex(e => e == item);
            TextMeshProUGUI[] texts = itemList[index].GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = logs[index][0];
            texts[1].text = logs[index][1];

            item.SetActive(true);
        }
    }

    public void addLog(string name, string line)
    {
        ++dataCount;

        List<string> tmp = new List<string>();
        tmp.Add(name);
        tmp.Add(line);
        logs.Add(tmp);

        RefreshItem();

        Debug.LogFormat("Add DataCount: {0}", dataCount);
    }

    /**
    public void clearLog()
    {
        GameObject[] logs = content.GetComponentsInChildren<GameObject>();

        foreach (GameObject log in logs)
        {
            if (log.tag == "DialogueLog")
                Destroy(log.gameObject);
        }
    }
    **/

}
