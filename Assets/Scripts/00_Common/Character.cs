using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("ĳ���� ����")]
    public string id;
    public string c_name; // Object.name�� ����
    /**
     * illust index
     * 0: �⺻
     * 1: ���, ��Ȳ
     * 2: ���̾���
     * 3: ���
     * 4: ����
     * 5: ȭ��
     * 6: ¥��
     * 
    * */
    public List<Sprite> illusts;
    public int illust_num;
    public int now_illust = 0;

    public void setCharacter(string id, string name, int illust)
    {
        this.id = id;
        c_name = name;

        //setIllustSet();
        setIllust(illust);

    }

    public void setIllust(int illust)
    {
        now_illust = illust;
        string path = "Images/Characters/" + id + "/";

        GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(path + now_illust);
    }

    public void setPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void fadeIn()
    {
        StartCoroutine(fadeInCo());
    }

    // ���� �ִϸ��̼�
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

    // ���� �ִϸ��̼�
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

    // ���� �ִϸ��̼�
    IEnumerator moveLeftCo()
    {
        Vector3 targetPos = new Vector3(-4, 1, 0);

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
        Vector3 targetPos = new Vector3(4, 1, 0);

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

}
