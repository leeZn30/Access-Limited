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
