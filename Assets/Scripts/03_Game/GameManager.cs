using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // �����ͺ��̽� ����Ű
        if (Input.GetKeyDown(KeyCode.D))
        {
            DatabaseManager.Instance.openPopup();
        }


        // �ð����� ����Ű
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject.Find("Background").GetComponent<Image>().color = Color.blue;
        }
    }


}
