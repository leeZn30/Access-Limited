using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("캐릭터 정보")]
    public int id;
    public string c_name; // Object.name과 구분
    /**
     * illust index
     * 0: 기본
     * 1: 곤란
     * 2: 짜증
     * 3: 놀람
     * 4: 미소
     * 5: 화남
     * 6: 
     * 
    * */
    public List<Sprite> illusts;
    public int now_illust = 0;

    [Header("위치")]
    [SerializeField] private Vector3 position = new Vector3(-1, 0, 0); // 초기위치

    // 등장 애니메이션

    // 퇴장 애니메이션

    // 무브 애니메이션

    void onDestory()
    {
        // 다른 Character도 찾아서 삭제 > 연쇄적
        Destroy(GameObject.FindGameObjectWithTag("Character"));
    }

}
