using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Day")]
    [SerializeField] int day;

    /**
    [Header("Day에서 얻어야 하는 모든 증언")]
    [Header("Day에서 얻어야 하는 모든 단서")]
    [Header("Day에서 얻어야 하는 모든 인물정보")]
    **/


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // 데이터베이스 단축키
        if (Input.GetKeyDown(KeyCode.D))
        {
            DatabaseManager.Instance.openClosePopup();
        }


        // 시간정지 단축키
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject.Find("Background").GetComponent<Image>().color = Color.blue;
        }
    }


}
