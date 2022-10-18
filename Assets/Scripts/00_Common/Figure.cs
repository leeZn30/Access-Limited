using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
    private int _id;
    public int id
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

    List<string> contents;

    public void addContents(string content)
    {
        contents.Add(content);
    }

    public List<string> getContents()
    {
        return contents;
    }

}
