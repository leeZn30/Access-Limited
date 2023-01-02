using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

abstract public class Info
{
    private string _id;

    public string id
    {
        get { return _id; }
        set { _id = value; }
    }

    private string _name;
    public string name
    {
        get { return _name; }
        set { _name = value; }
    }

    //List<string> contents = new List<string>();
    Dictionary<int, string> contents = new Dictionary<int, string>();
    public int updatedInfo = 0;

    public void addContent(string content, int idx)
    {
        //contents.Insert(idx, content);
        //contents.Add(content);
        content = content.Replace("@", "\n");
        contents[idx] = content;

        updatedInfo++;
    }

    public Dictionary<int, string> getContents()
    {
        return contents;
    }

    public string showContentsTxt()
    {
        string txt = "";

        foreach (var c in contents.OrderBy(k => k.Key))
        {
            txt += c.Value + "\n";
        }

        return txt;
    }

}
