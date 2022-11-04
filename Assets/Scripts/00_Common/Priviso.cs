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

    // »ý¼ºÀÚ
    public Priviso(string id, string name, string content, int idx)
    {
        this.id = id;
        this.name = name;
        image = Resources.Load<Sprite>("Images/Privisos/" + GameData.Instance.chapter + "/" + id);

        addContent(content, idx);
    }


}
