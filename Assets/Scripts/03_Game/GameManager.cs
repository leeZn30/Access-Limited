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
        // 데이터베이스 단축키
        if (Input.GetKeyDown(KeyCode.D))
        {
            DatabaseManager.Instance.openPopup();
        }


            // 시간정지 단축키
        if (Input.GetKeyDown(KeyCode.Return))
        {

        }

        // 임시
        if (Input.GetKeyDown(KeyCode.S))
        {
        }
    }

}
