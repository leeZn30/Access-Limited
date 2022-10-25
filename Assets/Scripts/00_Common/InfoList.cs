using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoList
{

    private List<Info> list;
    public int Count { get { return list.Count; } }
    public bool isEmpty { set { setEmpty(); } }

    // »ý¼ºÀÚ
    public InfoList()
    {
        list = new List<Info>();
    }

    public void Add(Info info)
    {
        list.Add(info);
        list.Sort((x, y) => x.id.CompareTo(y.id));
    }

    public void Remove(Info info)
    {
        list.Remove(info);
        list.Sort((x, y) => x.id.CompareTo(y.id));
    }

    private bool setEmpty()
    {
        if (list.Count == 0)
            return true;
        else
            return false;
    }

    public void Clear()
    {
        list.Clear();
    }

    public Info Find(string id)
    {
        var obj = list.Find(l => l.id == id);
        return obj;
    }

    public Info Find(Info info)
    {
        var obj = list.Find(l => l.id == info.id);
        return obj;
    }
    public IEnumerator<Info> GetEnumerator()
    {
        // int position = 0; // state
        foreach (var item in list)
        {
            // position++;
            // yield return position;
            yield return item;
        }
    }

}
