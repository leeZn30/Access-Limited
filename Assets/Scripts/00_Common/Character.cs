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
    [SerializeField] private Vector3 position = new Vector3(-1, 0, 0); // �ʱ���ġ

    // ���� �ִϸ��̼�

    // ���� �ִϸ��̼�

    // ���� �ִϸ��̼�

    void onDestory()
    {
        // �ٸ� Character�� ã�Ƽ� ���� > ������
        Destroy(GameObject.FindGameObjectWithTag("Character"));
    }

}
