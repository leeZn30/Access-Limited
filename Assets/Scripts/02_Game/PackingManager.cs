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
    public void insertIdx(Block idx, int order)
    {
        nowIdxs[order] = idx;

        checkAnswer();
    }

    public void disconnectUpBlock(int order)
    {
        nowIdxs[order - 1].downChaining = false;
    }

    void checkAnswer()
    {
    }
}
