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

    void Start()
    {
        CharacterQueue = InsertQueue(objectInfos[0]);
    }

    Queue<GameObject> InsertQueue(ObjectInfo objectInfo)
    {

        Queue<GameObject> tmpQueue = new Queue<GameObject>();
        for (int i = 0; i < objectInfo.count; i++)
        {
            GameObject tmpClone = Instantiate(
                objectInfo.goPrefab,
                transform.position,
                Quaternion.identity,
                objectInfo.tfPoolParent
            );

            tmpClone.SetActive(false);
            tmpQueue.Enqueue(tmpClone);
        }

        return tmpQueue;
    }

}
