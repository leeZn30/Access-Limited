using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("현재 모드")]
    /*
     * 0: game
     * 1: dialogue
     * 2: database
     */
    public int mode;

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
            GameObject.Find("Background").GetComponent<Image>().color = Color.blue;
        }
    }


}
