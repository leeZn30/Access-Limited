using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] Button dBtn;
    [SerializeField] Button sBtn;

    private void Awake()
    {
        dBtn.onClick.AddListener(dBtnClicked);
        sBtn.onClick.AddListener(sBtnClicked);
    }

    void dBtnClicked()
    {
        if (GameManager.Instance.clickedObj != null)
        {
            GameManager.Instance.clickedObj.startDialogue();

            MapManager.Instance.onInteractiveObject();
            Destroy(gameObject);
        }
    }

    void sBtnClicked()
    {
        DatabaseManager.Instance.openClosePopup();
        DatabaseManager.Instance.goPage(2);

        MapManager.Instance.onInteractiveObject();
        Destroy(gameObject);
    }
}
