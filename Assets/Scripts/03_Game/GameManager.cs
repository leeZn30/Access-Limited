using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int map;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ObjectTable.setTable();
        // ĳ���� �ؽ����̺� (�Ŀ��� ���� ���� ����÷� ����)
        CharacterTable.setTable();
    }

    void Update()
    {
        // �����ͺ��̽� ����Ű
        if (Input.GetKeyDown(KeyCode.D))
        {
            DatabaseManager.Instance.openClosePopup();
        }


        // �ð����� ����Ű
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject.Find("Background").GetComponent<Image>().color = Color.blue;
        }
    }


}
