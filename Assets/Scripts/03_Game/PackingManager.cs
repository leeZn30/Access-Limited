using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackingManager : Singleton<PackingManager>
{
    [Header("�������� ����")]
    [SerializeField] int[] answerIdxs = { 1, 2, 3};
    [SerializeField] Block[] nowIdxs = new Block[10];

    [Header("���� ���� ����")]
    public bool isRunning = true;

    void Start()
    {
        // �ӽ�
    }

    // ���� �ֱ�
    public void insertIdx()
    {
    }

    public void disconnectUpBlock(int order)
    {
    }

    void checkAnswer()
    {
    }
}
