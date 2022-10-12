using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("캐릭터 정보")]
    public int id;
    public string c_name; // Object.name과 구분
    /**
     * illust index
     * 0: 기본
     * 1: 곤란
     * 2: 짜증
     * 3: 놀람
     * 4: 미소
     * 5: 화남
     * 6: 
     * 
    * */
    public List<Sprite> illusts;
    public int now_illust = 0;

    [Header("위치")]
    [SerializeField] private Vector3 position; // 초기위치


    void Start()
    {
        position = gameObject.transform.position;
    }

    public void setIllust(int illust)
    {
        now_illust = illust;

        //GetComponent<SpriteRenderer>().sprite = illusts[now_illust];
    }


    // 등장 애니메이션
    IEnumerator fadeIn()
    {
        Color color = GetComponentInChildren<SpriteRenderer>().color;
        color.a = 0;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime / 1f;
            GetComponentInChildren<SpriteRenderer>().color = color;
            yield return null;
        }

    }

    // 퇴장 애니메이션
    IEnumerator fadeOut()
    {
        Color color = GetComponentInChildren<SpriteRenderer>().color;
        color.a = 1f;

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / 1f;
            GetComponentInChildren<SpriteRenderer>().color = color;
            yield return null;
        }

    }

    // 무브 애니메이션
    IEnumerator moveLeftCo()
    {
        Vector3 targetPos = new Vector3(-5, 1, 0);

        while (gameObject.transform.position != targetPos)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, Time.deltaTime / 0.2f);
            yield return null;
        }

    }

    public void moveLeft()
    {
        StartCoroutine(moveLeftCo());
    }

    IEnumerator moveRightCo()
    {
        Vector3 targetPos = new Vector3(5, 1, 0);

        while (gameObject.transform.position != targetPos)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, Time.deltaTime / 0.2f);
            yield return null;
        }
    }

    public void moveRight()
    {
        StartCoroutine(moveRightCo());
    }

    IEnumerator moveMiddleCo()
    {
        Vector3 targetPos = new Vector3(0, 1, 0);

        while (gameObject.transform.position != targetPos)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, Time.deltaTime / 0.2f);
            yield return null;
        }
    }

    public void moveMiddle()
    {
        StartCoroutine(moveMiddleCo());
    }


    void onDestory()
    {
        Debug.Log(c_name + " destroyed!");

        // 다른 Character도 찾아서 삭제 > 연쇄적
        Destroy(GameObject.FindGameObjectWithTag("Character"));
    }

}
