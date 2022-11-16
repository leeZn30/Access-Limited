using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogueManager : DialogueManager
{
    [Header("튜토리얼매니저")]
    [SerializeField] TutorialManager tutoManager;

    protected override void doMission(int type)
    {
        switch (type)
        {
            case 1: // 선택지
                getAnswers();
                break;

            case 2: // 증거 제출
                break;

            case 3:
                tutoManager.startConnectTuto();
                break;

            case 4:
                tutoManager.startFigureTuto();
                break;

            case 5:
                tutoManager.startMapSliding();
                break;

            default:
                break;
        }

    }
}
