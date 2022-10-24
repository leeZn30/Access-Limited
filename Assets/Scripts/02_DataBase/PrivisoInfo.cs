using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrivisoInfo : MonoBehaviour
{
    [Header("단서")]
    [SerializeField] Priviso priviso;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(pickPriviso);
        GetComponentInChildren<Image>().sprite = priviso.image;
    }

    public void setProviso(Priviso priviso)
    {
        this.priviso = priviso;
    }

    void pickPriviso()
    {
        if (priviso != null)
        {
            GameObject.Find("단서 이름").GetComponent<TextMeshProUGUI>().text = priviso.name;
            GameObject.Find("단서 설명").GetComponent<TextMeshProUGUI>().text = priviso.showContentsTxt();
            GameObject.Find("단서 이미지").GetComponent<Image>().sprite = priviso.image;
        }
    }
}
