using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("캐릭터 정보")]
    public string id;
    public string c_name; // Object.name과 구분
    /**
     * illust index
     * 0: 기본
     * 1: 곤란, 당황
     * 2: 어이없음
     * 3: 놀람
     * 4: 웃음
     * 5: 화남
     * 6: 짜증
     * 
    * */
    public int illust_num;
    public int now_illust = 0;


    public void setCharacter(string id, string name, int illust)
    {
        this.id = id;
        c_name = name;

        setIllust(illust);

    }

    public void setIllust(int illust)
    {
        now_illust = illust;
        string path = "Images/Characters/" + id + "/";

        GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(path + now_illust);
    }

    public void setPosition(int pos)
    {
        switch (pos)
        {
            case 0:
                transform.position = new Vector3(-4, 1);
                break;
            case 1:
                transform.position = new Vector3(0, 1);
                break;
            case 2:
                transform.position = new Vector3(4, 1);
                break;
        }

    }

    public void stopMovingAndPlace(int pos)
    {
        setPosition(pos);
    }

    public void fadeIn()
    {
        StartCoroutine(fadeInCo());
    }

    // 등장 애니메이션
    IEnumerator fadeInCo()
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
    IEnumerator fadeOutCo()
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
        Vector3 targetPos = new Vector3(-4, 1, 0);
        float duration = 0.0f;

        while (gameObject.transform.position != targetPos && duration < 1f)
        {
            duration += Time.deltaTime;

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, Time.deltaTime / 0.2f);
            yield return null;
        }

        transform.position = targetPos;
    }

    public void moveLeft()
    {
        StartCoroutine(moveLeftCo());
    }

    IEnumerator moveRightCo()
    {
        Vector3 targetPos = new Vector3(4, 1, 0);
        float duration = 0.0f;

        while (gameObject.transform.position != targetPos && duration < 1f)
        {
            duration += Time.deltaTime;

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, Time.deltaTime / 0.2f);
            yield return null;
        }

        transform.position = targetPos;
    }

    public void moveRight()
    {
        StartCoroutine(moveRightCo());
    }

    IEnumerator moveMiddleCo()
    {
        Vector3 targetPos = new Vector3(0, 1, 0);
        float duration = 0.0f;

        while (gameObject.transform.position != targetPos && duration < 1f)
        {
            duration += Time.deltaTime;

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, Time.deltaTime / 0.2f);
            yield return null;
        }

        transform.position = targetPos;
    }

    public void moveMiddle()
    {
        StartCoroutine(moveMiddleCo());
    }

}
