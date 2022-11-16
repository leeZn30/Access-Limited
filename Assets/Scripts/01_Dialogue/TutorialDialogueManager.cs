using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogueManager : DialogueManager
{
    [Header("Ʃ�丮��Ŵ���")]
    [SerializeField] TutorialManager tutoManager;

    protected override void doMission(int type)
    {
        switch (type)
        {
            case 1: // ������
                getAnswers();
                break;

            case 2: // ���� ����
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
