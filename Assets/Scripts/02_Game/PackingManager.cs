using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackingManager : Singleton<PackingManager>
{
    [Header("스테이지 정보")]
    [SerializeField] int[] answerIdxs = { 1, 2, 3};
    [SerializeField] Block[] nowIdxs = new Block[10];

    [Header("게임 진행 정보")]
    public bool isRunning = true;

    void Start()
    {
        // 임시
    }

    // 순서 넣기
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
