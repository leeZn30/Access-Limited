using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] Button dBtn;
    [SerializeField] Button sBtn;

    bool isCreated = false;
    public bool isMouseOver = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isCreated)
        {
            int layerMask = 1 << LayerMask.NameToLayer("UI");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15f, layerMask);

            if (!hit)
            {
                MapManager.Instance.onInteractiveObject();
                Destroy(gameObject);
            }

        }
    }

    private void LateUpdate()
    {
        isCreated = true;
    }

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

            Destroy(gameObject);
        }
    }

    void sBtnClicked()
    {
        DatabaseManager.Instance.openClosePopup();
        DatabaseManager.Instance.goPage(2);

        Destroy(gameObject);
    }

}
