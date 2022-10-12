using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("ĳ���� ����")]
    public int id;
    public string c_name; // Object.name�� ����
    /**
     * illust index
     * 0: �⺻
     * 1: ���
     * 2: ¥��
     * 3: ���
     * 4: �̼�
     * 5: ȭ��
     * 6: 
     * 
    * */
    public List<Sprite> illusts;
    public int now_illust = 0;

    [Header("��ġ")]
    [SerializeField] private Vector3 position; // �ʱ���ġ


    void Start()
    {
        position = gameObject.transform.position;
    }

    public void setIllust(int illust)
    {
        now_illust = illust;

        //GetComponent<SpriteRenderer>().sprite = illusts[now_illust];
    }


    // ���� �ִϸ��̼�
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

    // ���� �ִϸ��̼�
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

    // ���� �ִϸ��̼�
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

        // �ٸ� Character�� ã�Ƽ� ���� > ������
        Destroy(GameObject.FindGameObjectWithTag("Character"));
    }

}
