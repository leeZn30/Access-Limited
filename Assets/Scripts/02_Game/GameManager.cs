using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            openPopup();
        }

        // �ð����� ����Ű
        if (Input.GetKeyDown(KeyCode.Return))
        {

        }

        // �ӽ�
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void openPopup(int mode = 0)
    {

    }

}
