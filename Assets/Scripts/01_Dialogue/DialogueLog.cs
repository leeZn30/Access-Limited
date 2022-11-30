using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueLog : MonoBehaviour
{
    [Header("스크롤뷰 오브젝트")]
    [SerializeField] GameObject content;

    [Header("로그 오브젝트")]
    //[SerializeField] GameObject dialogue;

    public float verticalPadding;
    public float spacing;
    public GameObject dialogue;
    public List<string> logs = new List<string>();

    ScrollRect scrollRect;
    RectTransform rectTransform;
    readonly int bufItemCount = 4;
    readonly List<GameObject> itemList = new List<GameObject>();

    float itemHeight;
    int dataCount;

    float updateCount;
    float scrollU = 0f;

    void Awake()
    {
        scrollRect.GetComponentInChildren<ScrollRect>();
        rectTransform = GetComponent<RectTransform>();

        var prefabRect = dialogue.GetComponent<RectTransform>();
        itemHeight = prefabRect.rect.height + spacing;

        // item 생성 -> 초기엔 필요없음
        var scrollCount = (int)((rectTransform.rect.height - verticalPadding * 2 + spacing) / itemHeight);
        float pos = -(verticalPadding);
        for (int i = 0; i < scrollCount; ++i)
        {
            var item = Instantiate(dialogue, scrollRect.content);
            itemList.Add(item);
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, pos);
            pos -= itemHeight;
        }

        // 데이터개수에 맞게 컨텐츠 높이 설정 및 스크롤이 보일 위치 설정
        SetContentHeight();
        SetDialoguePos();

        scrollRect.onValueChanged.AddListener(RefreshItemListener);
        if (dataCount < scrollCount)
        {
            updateCount = float.MaxValue;
            int i;
            for (i = 0; i < dataCount; ++i)
            {
                itemList[i].SetActive(true);
            }

            for (; i < scrollCount; ++i)
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

    void SetContentHeight()
    {
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, dataCount * itemHeight - spacing + verticalPadding * 2);
    }

    void RefreshItem()
    {
        int addIdx;
        foreach (var item in itemList)
        {
            if ((addIdx = RelocateItem(item)) != 0)
            {
                int idx = itemList.IndexOf(item) + addIdx;
            }

            setItemActive(item);
        }
    }

    int RelocateItem(GameObject item)
    {
        float contentY = scrollRect.content.anchoredPosition.y;
        float y = item.transform.localPosition.y + contentY;
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
            item.transform.localPosition -= new Vector3(0, changeValue * jumpCount);
            return jumpCount * itemList.Count;
        }
        else if (y < bottom)
        {
            item.transform.localPosition += new Vector3(0, changeValue * jumpCount);
            return -jumpCount * itemList.Count;
        }

        return 0;
    }

    void setItemActive(GameObject item)
    {
        int idx = (int)(-(item.transform.localPosition.y + verticalPadding) / itemHeight);
        if (idx < 0 || idx >= dataCount)
        {
            item.SetActive(false);
        }

        else if (!item.gameObject.activeSelf)
            item.gameObject.SetActive(true);
    }

    void RefreshItemListener(Vector2 value)
    {
        if (value.y < 0f || value.y > 1f) return;

        if (Mathf.Abs(value.y - scrollU) > updateCount)
        {
            scrollU = value.y;
            RefreshItem();
        }
    }

    public void addLog(string name, string line)
    {
        GameObject newLog = Instantiate(dialogue, content.transform);

        TextMeshProUGUI[] texts = newLog.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = name;
        texts[1].text = line;
    }

    public void clearLog()
    {
        GameObject[] logs = content.GetComponentsInChildren<GameObject>();

        foreach (GameObject log in logs)
        {
            if (log.tag == "DialogueLog")
                Destroy(log.gameObject);
        }
    }

}
