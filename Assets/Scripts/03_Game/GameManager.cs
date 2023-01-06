using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int map;

    [Header("상호작용 오브젝트")]
    public InteractiveObject clickedObj = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ObjectTable.setTable();
        // 캐릭터 해시테이블 (후에는 게임 최초 실행시로 변경)
        CharacterTable.setTable();
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
        }
    }


}
