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

    [CanBeNull] public Queue<GameObject> CharacterQueue = new Queue<GameObject>();
    [CanBeNull] public Queue<GameObject> PlaceQueue = new Queue<GameObject>();
    [CanBeNull] public Queue<GameObject> AnswerQueue = new Queue<GameObject>();

    void Start()
    {
        CharacterQueue = InsertQueue(objectInfos[0]);
        AnswerQueue = InsertQueue(objectInfos[1]);

        if (objectInfos.Length > 2)
        {
            PlaceQueue = InsertQueue(objectInfos[2]);
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
