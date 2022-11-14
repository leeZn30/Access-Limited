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
            case 3:
                tutoManager.startConnectTuto();
                break;

            case 4:
                tutoManager.startFigureTuto();
                break;

            default:
                break;
        }

    }
}
