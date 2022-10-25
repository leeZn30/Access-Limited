using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<string> contents = new List<string>();
    public int updatedInfo = 0;

    public void addContent(string content)
    {
        contents.Add(content);
        updatedInfo++;
    }

    public List<string> getContents()
    {
        return contents;
    }

    public string showContentsTxt()
    {
        string txt = "";

        foreach (string c in contents)
        {
            txt += c + "\n";
        }

        return txt;
    }

}
