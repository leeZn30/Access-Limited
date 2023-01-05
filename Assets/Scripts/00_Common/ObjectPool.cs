using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

[System.Serializable]
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count;
    public Transform tfPoolParent;
}

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private ObjectInfo[] objectInfos = null;

    // ÇÊ¼ö
    [CanBeNull] public Queue<GameObject> CharacterQueue = new Queue<GameObject>();
    [CanBeNull] public Queue<GameObject> AnswerQueue = new Queue<GameObject>();
    [CanBeNull] public Queue<GameObject> figureQueue = new Queue<GameObject>();
    [CanBeNull] public Queue<GameObject> privisoQueue = new Queue<GameObject>();

    // ¸Ê ¾ÀÀÏ¶§
    [CanBeNull] public Queue<GameObject> PlaceQueue = new Queue<GameObject>();

    void Awake()
    {
        objectInfos[0].tfPoolParent = Camera.main.transform;
        objectInfos[1].tfPoolParent = GameObject.Find("DialogueUIs").transform.GetChild(1).transform;

        CharacterQueue = InsertQueue(objectInfos[0]);
        AnswerQueue = InsertQueue(objectInfos[1]);
        figureQueue = InsertQueue(objectInfos[2]);
        privisoQueue = InsertQueue(objectInfos[3]);

        if (objectInfos.Length > 4)
        {
            PlaceQueue = InsertQueue(objectInfos[4]);
        }
    }

    Queue<GameObject> InsertQueue(ObjectInfo objectInfo)
    {

        Queue<GameObject> tmpQueue = new Queue<GameObject>();

        for (int i = 0; i < objectInfo.count; i++)
        {
            GameObject tmpClone = Instantiate(
                objectInfo.goPrefab,
                objectInfo.tfPoolParent.position,
                //transform.position,
                Quaternion.identity,
                objectInfo.tfPoolParent
            );

            /**
            GameObject tmpClone = Instantiate(
                objectInfo.goPrefab,
                objectInfo.tfPoolParent
            );
            **/

            tmpClone.SetActive(false);
            tmpQueue.Enqueue(tmpClone);
        }

        return tmpQueue;
    }

}
