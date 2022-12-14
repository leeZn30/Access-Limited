using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Priviso : Info
{

    Sprite _image = null;
    public Sprite image
    {
        get { return _image; }
        set { _image = value; }
    }

    // ??????
    public Priviso(string id, string name, string content, int idx)
    {
        this.id = id;
        this.name = name;
        try
        {
            image = Resources.Load<Sprite>("Images/Privisos/" + GameData.Instance.chapter + "/" + id);
        }
        catch
        {
            image = Resources.Load<Sprite>("Images/Privisos/default");
        }

        addContent(content, idx);
    }


}
