using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrivisoInfo : MonoBehaviour
{
    [Header("�ܼ�")]
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
            GameObject.Find("�ܼ� �̸�").GetComponent<TextMeshProUGUI>().text = priviso.name;
            GameObject.Find("�ܼ� ����").GetComponent<TextMeshProUGUI>().text = priviso.showContentsTxt();
            GameObject.Find("�ܼ� �̹���").GetComponent<Image>().sprite = priviso.image;
        }
    }
}
