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

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //GameObject.Find("Background").transform.position = new Vector3(-9, 0, 0);

            StartCoroutine(moveBackgroundCo(new Vector3(-18,0,0)));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //GameObject.Find("Background").transform.position = new Vector3(9, 0, 0);

            StartCoroutine(moveBackgroundCo(new Vector3(18, 0, 0)));
        }


    }

    IEnumerator moveBackgroundCo(Vector3 plustPos)
    {
        Vector3 targetPos = GameObject.Find("Background").transform.position + plustPos;

        while (GameObject.Find("Background").transform.position != targetPos)
        {
            GameObject.Find("Background").transform.position = Vector3.Lerp(GameObject.Find("Background").transform.position, targetPos, Time.deltaTime / 0.5f);
            yield return null;
        }
        yield return null;
    }

}
