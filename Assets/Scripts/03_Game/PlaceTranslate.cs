using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlaceTranslate : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] bool isClicked = false;

    [Header("��� ����Ʈ")]
    [SerializeField] List<Scene> places = new List<Scene>();

    [Header("������Ʈ")]
    [SerializeField] GameObject box;


    void Start()
    {
        GetComponent<Button>().onClick.AddListener(openClosePlaces);
    }


    void openClosePlaces()
    {
        if (!isClicked)
        {

            foreach(Scene p in places)
            {
                Button btn = ObjectPool.Instance.PlaceQueue.Dequeue().GetComponent<Button>();
                //btn.GetComponentInChildren<TextMeshProUGUI>().text = p;
                btn.gameObject.SetActive(true);
                // ���� �ƴ�
                btn.onClick.AddListener(delegate { SceneManager.LoadScene(p.name); });
            }
            isClicked = true;

        }
        else
        {
            //Button[] places = box.GetComponentsInChildren<Button>();
            GameObject[] places = GameObject.FindGameObjectsWithTag("PlaceBtn");
            foreach(GameObject b in places)
            {
                b.SetActive(false);
                ObjectPool.Instance.PlaceQueue.Enqueue(b);
            }
            isClicked = false;

        }
    }
}
