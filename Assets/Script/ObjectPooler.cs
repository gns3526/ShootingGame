using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPooler : MonoBehaviourPun
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler OP;
    void Awake() => OP = this;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public GameObject[] allOfPools;

    //public int a;
    GameObject spawnedOb;
    Sprite spriteA;
    Vector2 scaleA;
    Vector2 boxSizeA;
    Vector2 boxOffsetA;
    bool playerAttack;
    public void PrePoolInstantiate()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = PhotonNetwork.Instantiate(pool.tag, new Vector3(4, 4, 0), Quaternion.identity);


                obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, false, -2, -1, 0, true);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject PoolInstantiate(string tag, Vector3 position, Quaternion rotation, int bulletIndex, int bulletAniCode, int bulletSpeedIndex, bool isPlayerAttack)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't excist");
            return null;
        }

        spawnedOb = poolDictionary[tag].Dequeue();//Take out First as Num

        spawnedOb.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, true, bulletIndex, bulletAniCode, bulletSpeedIndex, isPlayerAttack);
        spawnedOb.transform.position = position;
        spawnedOb.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(spawnedOb);

        return spawnedOb;
    }

    public void PoolDestroy(GameObject obj)
    {
        obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, false, -2, -1, 0, true);
    }
}
