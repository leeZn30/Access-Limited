using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("������Ʈ")]
    [SerializeField] TextMeshProUGUI name_b;
    [SerializeField] TextMeshProUGUI line_b;

    [Header("���")]
    public string line;
    public string c_name;

    public void showline()
    {
        name_b.text = c_name;
        line_b.text = line;
    }


}
