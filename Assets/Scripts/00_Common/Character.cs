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

        StartCoroutine(fadeIn());
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

    // ���� �ִϸ��̼�

    void onDestory()
    {
        Debug.Log(c_name + " destroyed!");

        // �ٸ� Character�� ã�Ƽ� ���� > ������
        Destroy(GameObject.FindGameObjectWithTag("Character"));
    }

}
