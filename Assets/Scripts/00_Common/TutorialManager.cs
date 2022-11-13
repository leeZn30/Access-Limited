using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [Header("튜토리얼 진행중")]
    [SerializeField] bool isTutorialRunning = false;

    [Header("푸시 알림")]
    [SerializeField] PushEffect push;

    [Header("프리팹")]
    [SerializeField] GameObject outline;

    public void startConnectTuto()
    {
        push.setText("D키를 눌러 데이터베이스를 켜주세요.");
        push.stayInfo();

        StartCoroutine(connectTutoCo());
    }

    IEnumerator connectTutoCo()
    {
        GameObject go = null;
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!isTutorialRunning)
                {
                    isTutorialRunning = true;
                    push.setText("통신 연결을 확인하세요.");

                    GameObject connectInfo = GameObject.Find("통신 연결 이미지");

                    go = Instantiate(outline, connectInfo.transform);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(1700, 80);

                    yield return new WaitForSeconds(2f);

                    push.setText("D키를 누르거나, Exit 버튼을 클릭해 나가주세요.");
                }
                else
                {
                    push.dissapearInfo();
                    Destroy(go);
                    break;
                }
            }

            yield return null;
        }

        isTutorialRunning = false;
        TutorialDialogueManager.Instance.mission = 0;
        yield return null;
    }

    public void startFigureTuto()
    {
        push.setText("D를 눌러 데이터베이스를 켜주세요");
        push.stayInfo();
    }

    IEnumerator figureTutoCo()
    {
        GameObject go = null;
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!isTutorialRunning)
                {
                    isTutorialRunning = true;
                    push.setText("사건 파일을 클릭하세요.");

                    GameObject eventfile = GameObject.Find("사건 파일 이미지");

                    go = Instantiate(outline, eventfile.transform);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);


                }
                else
                {
                    push.dissapearInfo();
                    Destroy(go);
                    break;
                }
            }

            yield return null;
        }

        isTutorialRunning = false;
        TutorialDialogueManager.Instance.mission = 0;
        yield return null;
    }
}
