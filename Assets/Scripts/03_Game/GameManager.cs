using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int map;

    [Header("��ȣ�ۿ� ������Ʈ")]
    public InteractiveObject clickedObj = null;

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
        }
    }


}
