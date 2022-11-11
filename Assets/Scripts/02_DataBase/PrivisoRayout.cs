using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrivisoRayout : MonoBehaviour
{
    [Header("챕터")]
    [SerializeField] int chapter;

    [Header("오브젝트")]
    [SerializeField] Image privisoImage = null;
    [SerializeField] TextMeshProUGUI pName;
    [SerializeField] TextMeshProUGUI pDesc;
    [SerializeField] GameObject scrollview;
    [SerializeField] GameObject priviso_prb;

    [Header("단서")]
    [SerializeField] Priviso pickedPriviso;

    // Start is called before the first frame update
    void Start()
    {
        chapter = DatabaseManager.Instance.chapter;
    }

    void OnEnable()
    {
        destroyPrivisos();
        setPrivisoInventory();
    }

    void destroyPrivisos()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PrivisoList");

        foreach (GameObject go in gos)
        {
            go.SetActive(false);
            ObjectPool.Instance.privisoQueue.Enqueue(go);
        }
    }

    void setPrivisoInventory()
    {
        foreach(Priviso item in GameData.Instance.privisos)
        {
            GameObject go = ObjectPool.Instance.privisoQueue.Dequeue();
            PrivisoInfo p = go.GetComponent<PrivisoInfo>();
            p.setProviso(item);
            go.SetActive(true);
        }
    }

}
